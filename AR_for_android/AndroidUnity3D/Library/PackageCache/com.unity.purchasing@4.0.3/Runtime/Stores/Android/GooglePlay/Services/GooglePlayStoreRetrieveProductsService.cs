using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Interfaces;

namespace UnityEngine.Purchasing
{
    class GooglePlayStoreRetrieveProductsService : IGooglePlayStoreRetrieveProductsService
    {
        IGooglePlayStoreService m_GooglePlayStoreService;
        IGoogleFetchPurchases m_GoogleFetchPurchases;
        IStoreCallback m_StoreCallback;
        IGooglePlayConfigurationInternal m_GooglePlayConfigurationInternal;

        internal GooglePlayStoreRetrieveProductsService(IGooglePlayStoreService googlePlayStoreService, IGoogleFetchPurchases googleFetchPurchases, IGooglePlayConfigurationInternal googlePlayConfigurationInternal)
        {
            m_GooglePlayStoreService = googlePlayStoreService;
            m_GoogleFetchPurchases = googleFetchPurchases;
            m_GooglePlayConfigurationInternal = googlePlayConfigurationInternal;
        }

        public void SetStoreCallback(IStoreCallback storeCallback)
        {
            m_StoreCallback = storeCallback;
        }

        public void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products)
        {
            if (m_StoreCallback != null)
            {
                m_GooglePlayStoreService.RetrieveProducts(products, retrievedProducts =>
                {
                    m_GoogleFetchPurchases.FetchPurchases(purchaseProducts =>
                    {
                        var mergedProducts = MakePurchasesIntoProducts(retrievedProducts, purchaseProducts);
                        m_StoreCallback.OnProductsRetrieved(mergedProducts);
                    });
                }, () =>
                {
                    m_GooglePlayConfigurationInternal.NotifyInitializationConnectionFailed();
                });
            }
        }

        public void ResumeConnection()
        {
            m_GooglePlayStoreService.ResumeConnection();
        }

        static List<ProductDescription> MakePurchasesIntoProducts(List<ProductDescription> retrievedProducts, IEnumerable<Product> purchaseProducts)
        {
            var updatedProducts = new List<ProductDescription>(retrievedProducts);
            if (purchaseProducts != null)
            {
                foreach (var purchaseProduct in purchaseProducts)
                {
                    var retrievedProductIndex = updatedProducts.FindLastIndex(product => product.storeSpecificId == purchaseProduct.definition.storeSpecificId);
                    if (retrievedProductIndex != -1)
                    {
                        var retrievedProduct = updatedProducts[retrievedProductIndex];
                        updatedProducts[retrievedProductIndex] = new ProductDescription(retrievedProduct.storeSpecificId, retrievedProduct.metadata, purchaseProduct.receipt, purchaseProduct.transactionID, retrievedProduct.type);
                    }
                }
            }
            return updatedProducts;
        }
    }
}
