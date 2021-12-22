using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.MiniJSON;

namespace UnityEngine.Purchasing
{
    internal class UDPBindings : INativeUDPStore
    {
        private object m_Bridge;
        private Action<bool, string> m_RetrieveProductsCallbackCache;

        public UDPBindings()
        {
            Type udpIapBridge = UdpIapBridgeInterface.GetClassType();
            if (udpIapBridge != null)
            {
                m_Bridge = Activator.CreateInstance(udpIapBridge);
            }
            else
            {
                Debug.LogError("Failed to access UDP. Please make sure your UDP package is installed and up-to-date");
                throw new NotImplementedException();
            }
        }

        public void Initialize(Action<bool, string> callback)
        {
            if (m_Bridge != null)
            {
                var initMethod = UdpIapBridgeInterface.GetInitMethod();
                initMethod.Invoke(m_Bridge, new object[] {callback});
            }
            else
            {
                Debug.LogError("Cannot Initialize UDP store module. Please make sure your UDP package is installed and up-to-date");
                throw new NotImplementedException();
            }
        }

        public void Purchase(string productId, Action<bool, string> callback, string developerPayload = null)
        {
            if (m_Bridge != null)
            {
                var purchaseMethod = UdpIapBridgeInterface.GetPurchaseMethod();
                purchaseMethod.Invoke(m_Bridge, new object[] {productId, callback, developerPayload});
            }
            else
            {
                Debug.LogError("Cannot Purchase via UDP. Please make sure your UDP package is installed and up-to-date");
                throw new NotImplementedException();
            }
        }

        public void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products, Action<bool, string> callback)
        {
            if (m_Bridge != null)
            {

                if (m_RetrieveProductsCallbackCache != null)
                {
                    callback(false, "{ \"error\" : \"already retrieving products\" }");
                    return;
                }

                m_RetrieveProductsCallbackCache = callback;
                Action<bool, object> retrieveCallback = OnInventoryQueried;

                var retrieveProductsMethod = UdpIapBridgeInterface.GetRetrieveProductsMethod();
                List<string> ids = new List<String>();
                foreach (var product in products)
                {
                    ids.Add(product.storeSpecificId);
                }
                retrieveProductsMethod.Invoke(m_Bridge, new object[] {new ReadOnlyCollection<string>(ids), retrieveCallback});
            }
            else
            {
                Debug.LogError("Cannot Retrieve Products from UDP. Please make sure your UDP package is installed and up-to-date");
                throw new NotImplementedException();
            }
        }

        public void FinishTransaction(ProductDefinition productDefinition, string transactionID)
        {
            if (m_Bridge != null)
            {
                var finishTransactionMethod = UdpIapBridgeInterface.GetFinishTransactionMethod();
                finishTransactionMethod.Invoke(m_Bridge, new object[] {transactionID});
            }
            else
            {
                Debug.LogError("Cannot Complete transaction for UDP. Please make sure your UDP package is installed and up-to-date");
                throw new NotImplementedException();
            }
        }

        private void OnInventoryQueried(bool success, object payload)
        {
            bool actualSuccess = success;
            string parsedPayload;
            Type inventoryType = InventoryInterface.GetClassType();

            if (success)
            {
                if (inventoryType != null)
                {
                    object inventory = payload;
                    if (inventory != null && inventory.GetType() == inventoryType)
                    {
                        HashSet<ProductDescription> fetchedProducts = new HashSet<ProductDescription>();

                        var getProductList = InventoryInterface.GetProductListMethod();
                        var products = (IEnumerable) getProductList.Invoke(inventory, null);
                        var productList = products.Cast<object>().ToList();

                        foreach (var productInfo in productList)
                        {
                            var priceProp = ProductInfoInterface.GetPriceProp();
                            var titleProp = ProductInfoInterface.GetTitleProp();
                            var descProp = ProductInfoInterface.GetDescriptionProp();
                            var currencyProp = ProductInfoInterface.GetCurrencyProp();
                            var microsProp = ProductInfoInterface.GetPriceAmountMicrosProp();

                            ProductMetadata metadata = new ProductMetadata(
                                (string) priceProp.GetValue(productInfo, null),
                                (string) titleProp.GetValue(productInfo, null),
                                (string) descProp.GetValue(productInfo, null),
                                (string) currencyProp.GetValue(productInfo, null),
                                Convert.ToDecimal((long) microsProp.GetValue(productInfo, null)) / 1000000);


                            var idProp = ProductInfoInterface.GetProductIdProp();
                            var productId = (string) idProp.GetValue(productInfo, null);

                            ProductDescription desc = new ProductDescription(productId, metadata);

                            var hasPurchase = InventoryInterface.HasPurchaseMethod();
                            if ((bool) hasPurchase.Invoke(inventory, new object[] {productId}))
                            {
                                var getPurchaseInfo = InventoryInterface.GetPurchaseInfoMethod();
                                object purchase = getPurchaseInfo.Invoke(inventory, new object[] {productId});

                                var dic = StringPropertyToDictionary(purchase);
                                string transactionId = dic["GameOrderId"];
                                var storeSpecificId = dic["ProductId"];

                                if (!string.IsNullOrEmpty(transactionId))
                                {
                                    dic["transactionId"] = transactionId;
                                }

                                if (!string.IsNullOrEmpty(storeSpecificId))
                                {
                                    dic["storeSpecificId"] = storeSpecificId;
                                }

                                desc = new ProductDescription(productId, metadata, dic.toJson(), transactionId);
                            }

                            fetchedProducts.Add(desc);
                        }

                        parsedPayload = JSONSerializer.SerializeProductDescs(fetchedProducts);
                    }
                    else
                    {
                        actualSuccess = false;
                        parsedPayload = "{ \"error\" : \"Cannot load inventory from UDP. Please make sure your UDP package is installed and up-to-date\" }";
                    }
                }
                else
                {
                    actualSuccess = false;
                    parsedPayload = "{ \"error\" : \"Cannot parse inventory type for UDP. Please make sure your UDP package is installed and up-to-date\" }";
                }
            }
            else
            {
                parsedPayload = (string) payload;
            }

            m_RetrieveProductsCallbackCache(actualSuccess, parsedPayload);
            m_RetrieveProductsCallbackCache = null;
        }

        #region INativeStore - Unused

        public void RetrieveProducts(string json)
        {
            throw new NotImplementedException();
        }

        public void Purchase(string productJSON, string developerPayload)
        {
            throw new NotImplementedException();
        }

        public void FinishTransaction(string productJSON, string transactionID)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region helper functions

        /// <summary>
        /// Put the string property of <see cref="info"/> into a dictionary if the property is not empty string.
        /// </summary>
        /// <param name="info">Model object, namely <see cref="PurchaseInfo"/> or <see cref="UserInfo"/></param>
        /// <returns></returns>
        internal static Dictionary<string, string> StringPropertyToDictionary(object info)
        {
            var dictionary = new Dictionary<string, string>();
            if (info == null){
                return dictionary;
            }

            var properties = info.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = (string)property.GetValue(info, null);
                    if (!string.IsNullOrEmpty(value))
                        dictionary[property.Name] = value;
                }
            }

            return dictionary;
        }

        #endregion
    }
}
