using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.MiniJSON;

namespace UnityEngine.Purchasing
{
    internal class UDPImpl : JSONStore, IUDPExtensions
    {
        private INativeUDPStore m_Bindings;
        private object m_UserInfo = null; //UDP UserInfo via Reflection
        private string m_LastInitError;
        private const string k_Unknown = "Unknown"; // From PurchaseFailureReason
        private bool m_Initialized = false;
        private const int PURCHASE_PENDING_CODE = -303;
        private Action<Product> m_DeferredCallback;
        private const string k_Errorcode = "errorCode"; // From UDP sdk purchase pending state

        public void SetNativeStore(INativeUDPStore nativeUdpStore)
        {
            m_Bindings = nativeUdpStore;
        }

        #region IStore

        /// <summary>
        /// This function only save the IStoreCallback.
        /// The initialization of UDP is put into <see cref="RetrieveProducts"/>,
        /// because in <see cref="PurchasingManager"/>, <see cref="Initialize"/> and <see cref="RetrieveProducts"/>
        /// will be called together.
        /// However, for UDP, <see cref="RetrieveProducts"/> must been done AFTER <see cref="Initialize"/> succeeds.
        /// If the <see cref="Initialize"/> involves login, it may take a long time and <see cref="RetrieveProducts"/>
        /// will fail.
        /// </summary>
        /// <param name="callback"></param>
        public override void Initialize(IStoreCallback callback)
        {
            unity = callback;
        }

        public override void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products)
        {
            Action<bool, string> retrieveCallback = (success, json) =>
            {
                if (success && !string.IsNullOrEmpty(json))
                {
                    OnProductsRetrieved(json);
                }
                else
                {
                    m_Logger.LogWarning("Unity IAP", "RetrieveProducts failed: " + json);
                }
            };

            if (!m_Initialized)
            {
                m_Bindings.Initialize((success, message) =>
                {
                    // Clear the message and userInfo when an Initialized is called successfully.
                    m_LastInitError = "";
                    m_UserInfo = null;

                    if (success)
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            var dic = message.HashtableFromJson();
                            if (dic.ContainsKey("Channel"))
                            {
                                Type udpUserInfo = UserInfoInterface.GetClassType();
                                if (udpUserInfo != null)
                                {
                                    m_UserInfo = Activator.CreateInstance(udpUserInfo);
                                    DictionaryToStringProperty(dic, m_UserInfo);
                                }
                            }
                        }

                        m_Initialized = true;
                        // IStoreCallback do not have initialize success callback.
                        m_Bindings.RetrieveProducts(products, retrieveCallback);
                    }
                    else
                    {
                        m_LastInitError = message;
                        unity.OnSetupFailed(InitializationFailureReason.AppNotKnown);
                    }
                });
            }
            else
            {
                m_Bindings.RetrieveProducts(products, retrieveCallback);
            }
        }

        public override void Purchase(ProductDefinition product, string developerPayload)
        {
            m_Bindings.Purchase(product.storeSpecificId, (success, message) =>
            {
                var dic = message.HashtableFromJson();
                if (success)
                {
                    var transactionId = dic.GetString("GameOrderId");
                    var storeSpecificId = dic.GetString("ProductId");
                    if (!string.IsNullOrEmpty(transactionId))
                    {
                        dic["transactionId"] = transactionId;
                    }

                    if (!string.IsNullOrEmpty(storeSpecificId))
                    {
                        dic["storeSpecificId"] = storeSpecificId;
                    }

                    if (!product.storeSpecificId.Equals(storeSpecificId))
                    {
                        m_Logger.LogFormat(LogType.Error,
                            "UDPImpl received mismatching product Id for purchase. Expected {0}, received {1}",
                            product.storeSpecificId, storeSpecificId);
                    }

                    var data = dic.toJson();
                    unity.OnPurchaseSucceeded(product.storeSpecificId, data, transactionId);
                }
                else
                {
                    if (dic.ContainsKey(k_Errorcode) && Convert.ToInt32(dic[k_Errorcode]) == PURCHASE_PENDING_CODE)
                    {
                        if (null != m_DeferredCallback)
                        {
                            OnPurchaseDeferred(product.storeSpecificId);
                        }
                        return;
                    }

                    PurchaseFailureReason reason = (PurchaseFailureReason) Enum.Parse(typeof(PurchaseFailureReason),
                        k_Unknown);

                    var reasonString = reason.ToString();
                    var errDic = new Dictionary<string, object> {["error"] = reasonString};

                    if (dic.ContainsKey("purchaseInfo"))
                    {
                        errDic["purchaseInfo"] = dic["purchaseInfo"];
                    }

                    var errData = errDic.toJson();
                    var pfd = new PurchaseFailureDescription(product.storeSpecificId, reason, message);
                    unity.OnPurchaseFailed(pfd);
                }
            }, developerPayload);
        }

        private void OnPurchaseDeferred(string productId)
        {
            if (null == productId || productId == "")
            {
                return;
            }

            var product = unity.products?.WithStoreSpecificID(productId);

            if (null != product)
            {
                m_DeferredCallback(product);
            }
        }

        public override void FinishTransaction(ProductDefinition product, string transactionId)
        {
            if (product == null || transactionId == null)
            {
                return;
            }

            if (product.type == ProductType.Consumable)
            {
                m_Bindings.FinishTransaction(product, transactionId);
            }
        }

        #endregion

        #region extension

        public string GetLastInitializationError()
        {
            return m_LastInitError;
        }

        public object GetUserInfo()
        {
            return m_UserInfo;
        }

        public void RegisterPurchaseDeferredListener(Action<Product> callback)
        {
            this.m_DeferredCallback = callback;
        }

        public void EnableDebugLog(bool enable)
        {
            Type storeServiceInfo = StoreServiceInterface.GetClassType();
            if (storeServiceInfo != null)
            {
                var enableDebugLogging = StoreServiceInterface.GetEnableDebugLoggingMethod();
                enableDebugLogging.Invoke(null, new object[] {enable});
            }
            else
            {
                Debug.LogError("Failed to access StoreService for UDP, cannot enable logging");
            }
        }

        #endregion

        #region helper functions

        private static void DictionaryToStringProperty(Dictionary<string, object> dic, object info)
        {
            var properties = info.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                    property.SetValue(info, (string) dic.GetString(property.Name), null);
            }
        }

        #endregion
    }
}
