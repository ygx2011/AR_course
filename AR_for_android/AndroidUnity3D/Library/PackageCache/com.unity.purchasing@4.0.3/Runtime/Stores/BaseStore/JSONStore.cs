using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Purchasing.Extension;
using System.Text;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Internal store implementation passing store requests from the user through to the underlaying
    /// native store system, and back again. Binds a native store system binding to a callback.
    /// </summary>
    internal class JSONStore : AbstractStore, IUnityCallback, IStoreInternal, ITransactionHistoryExtensions
    {
        public Product[] storeCatalog {
            get {
                var result = new List<Product>();
                if (m_StoreCatalog != null && unity.products.all != null)
                {
                    foreach (var catalogProduct in m_StoreCatalog)
                    {
                        foreach (var controllerProduct in unity.products.all)
                        {
                            // Ensure owned products are excluded from list (except when consumable)
                            bool isProductOwned = false;
                            if (controllerProduct.definition.type != ProductType.Consumable)
                            {
                                if (controllerProduct.hasReceipt || !String.IsNullOrEmpty(controllerProduct.transactionID)) {
                                    isProductOwned = true;
                                }
                            }
                            // TODO: Update Engine Code so Product Definition comparision Equals checks against storeSpecificId
                            if (controllerProduct.availableToPurchase &&
                                !isProductOwned &&
                                controllerProduct.definition.storeSpecificId == catalogProduct.storeSpecificId)
                            {
                                result.Add(controllerProduct);
                            }
                        }
                    }
                }
                return result.ToArray();
            }
        }

        protected IStoreCallback unity;
        private INativeStore m_Store;
        private List<ProductDefinition> m_StoreCatalog;
        private bool m_IsRefreshing;

        private Action m_RefreshCallback;

        // m_Module is our StandardPurchasingModule, added via reflection to avoid core changes etc.
        private StandardPurchasingModule m_Module;

        protected ILogger m_Logger;

        // ITransactionHistoryExtensions stuff
        //
        // Enhanced error information
        protected PurchaseFailureDescription m_LastPurchaseFailureDescription;
        private StoreSpecificPurchaseErrorCode m_LastPurchaseErrorCode = StoreSpecificPurchaseErrorCode.Unknown;

        const string k_StoreSpecificErrorCodeKey = "storeSpecificErrorCode";

        /// <summary>
        /// No arg constructor due to cyclical dependency on IUnityCallback.
        /// </summary>
        public JSONStore()
        {
        }

        public void SetNativeStore(INativeStore native) {
            this.m_Store = native;
        }

        void IStoreInternal.SetModule(StandardPurchasingModule module)
        {
            if(module == null)
            {
                return;
            }
            this.m_Module = module;
            if(module.logger != null)
            {
                this.m_Logger = module.logger;
            }
            else
            {
                this.m_Logger = UnityEngine.Debug.unityLogger;
            }
        }

        public override void Initialize (IStoreCallback callback)
        {
            this.unity = callback;

            if(m_Module != null)
            {
                var storeName = m_Module.storeInstance.storeName;
            }
            else
            {
                if(m_Logger != null)
                {
                    m_Logger.LogWarning("UnityIAP", "JSONStore init has no reference to SPM, can't start managed store");
                }
            }
        }

        public override void RetrieveProducts (ReadOnlyCollection<ProductDefinition> products)
        {
            m_Store.RetrieveProducts(JSONSerializer.SerializeProductDefs(products));
        }

        internal void ProcessManagedStoreResponse(List<ProductDefinition> storeProducts)
        {
            m_StoreCatalog = storeProducts;
            if (m_IsRefreshing)
            {
                m_IsRefreshing = false;
                // Skip native store layer during refresh if catalog contains no information
                if (storeCatalog.Length == 0 && m_RefreshCallback != null)
                {
                    m_RefreshCallback();
                    m_RefreshCallback = null;
                    return;
                }
            }
            var products = new HashSet<ProductDefinition>();
            if (storeProducts != null) {
                products.UnionWith(storeProducts);
            }
            m_Store.RetrieveProducts (JSONSerializer.SerializeProductDefs (products));
        }

        public override void Purchase (UnityEngine.Purchasing.ProductDefinition product, string developerPayload)
        {
            m_Store.Purchase (JSONSerializer.SerializeProductDef (product), developerPayload);
        }

        public override void FinishTransaction (UnityEngine.Purchasing.ProductDefinition product, string transactionId)
        {
            // Product definitions may be null if a store tells Unity IAP about an unknown product;
            // Unity IAP will not have a corresponding definition but will still finish the transaction.
            var def = product == null ? null : JSONSerializer.SerializeProductDef (product);
            m_Store.FinishTransaction (def, transactionId);
        }

        public void OnSetupFailed (string reason)
        {
            var r = (InitializationFailureReason) Enum.Parse (typeof(InitializationFailureReason), reason, true);
            unity.OnSetupFailed (r);
        }

        public virtual void OnProductsRetrieved (string json)
        {
            // NB: AppleStoreImpl overrides this completely and does not call the base.
            unity.OnProductsRetrieved (JSONSerializer.DeserializeProductDescriptions (json));
        }

        public virtual void OnPurchaseSucceeded (string id, string receipt, string transactionID)
        {
            unity.OnPurchaseSucceeded (id, receipt, transactionID);
        }

        public void OnPurchaseFailed (string json)
        {
            OnPurchaseFailed(JSONSerializer.DeserializeFailureReason(json), json);
        }

        public void OnPurchaseFailed (PurchaseFailureDescription failure, string json = null)
        {
            m_LastPurchaseFailureDescription = failure;
            m_LastPurchaseErrorCode = ParseStoreSpecificPurchaseErrorCode(json);

            unity.OnPurchaseFailed(failure);
        }

        public PurchaseFailureDescription GetLastPurchaseFailureDescription()
        {
            return m_LastPurchaseFailureDescription;
        }

        public StoreSpecificPurchaseErrorCode GetLastStoreSpecificPurchaseErrorCode()
        {
            return m_LastPurchaseErrorCode;
        }

        private StoreSpecificPurchaseErrorCode ParseStoreSpecificPurchaseErrorCode(string json)
        {
            // If we didn't get any JSON just return Unknown.
            if (json == null)
            {
                return StoreSpecificPurchaseErrorCode.Unknown;
            }

            // If the dictionary contains a storeSpecificErrorCode, return it, otherwise return Unknown.
            var purchaseFailureDictionary = MiniJson.JsonDecode(json) as Dictionary<string, object>;
            if (purchaseFailureDictionary != null && purchaseFailureDictionary.ContainsKey(k_StoreSpecificErrorCodeKey) && Enum.IsDefined(typeof(StoreSpecificPurchaseErrorCode), (string) purchaseFailureDictionary[k_StoreSpecificErrorCodeKey]))
            {
                string storeSpecificErrorCodeString = (string) purchaseFailureDictionary[k_StoreSpecificErrorCodeKey];
                return (StoreSpecificPurchaseErrorCode) Enum.Parse(typeof(StoreSpecificPurchaseErrorCode),
                    storeSpecificErrorCodeString);
            }
            return StoreSpecificPurchaseErrorCode.Unknown;
        }
    }
}
