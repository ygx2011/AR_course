using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Interfaces;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing
{
    class GoogleFetchPurchases : IGoogleFetchPurchases
    {
        IGooglePlayStoreService m_GooglePlayStoreService;
        IGooglePlayStoreFinishTransactionService m_TransactionService;
        IStoreCallback m_StoreCallback;
        internal GoogleFetchPurchases(IGooglePlayStoreService googlePlayStoreService, IGooglePlayStoreFinishTransactionService transactionService)
        {
            m_GooglePlayStoreService = googlePlayStoreService;
            m_TransactionService = transactionService;
        }

        public void SetStoreCallback(IStoreCallback storeCallback)
        {
            m_StoreCallback = storeCallback;
        }

        public void FetchPurchases()
        {
            m_GooglePlayStoreService.FetchPurchases(OnFetchedPurchase);
        }

        public void FetchPurchases(Action<List<Product>> onQueryPurchaseSucceed)
        {
            m_GooglePlayStoreService.FetchPurchases(
                googlePurchases =>
                {
                    onQueryPurchaseSucceed(FillProductsWithPurchases(googlePurchases));
                });
        }

        List<Product> FillProductsWithPurchases(IEnumerable<GooglePurchase> purchases)
        {
            var purchasedProducts = new List<Product>();

            foreach (var purchase in purchases.Where(purchase => purchase != null).ToList())
            {
                var product = m_StoreCallback?.FindProductById(purchase.sku);
                if (product != null)
                {
                    var updatedProduct = new Product(product.definition, product.metadata, purchase.receipt)
                    {
                        transactionID = purchase.purchaseToken
                    };
                    purchasedProducts.Add(updatedProduct);
                }
            }

            return purchasedProducts;
        }

        void OnFetchedPurchase(List<GooglePurchase> purchases)
        {
            if (purchases != null)
            {
                var purchasedProducts = FillProductsWithPurchases(purchases);
                if (purchasedProducts.Count > 0)
                {
                    m_StoreCallback?.OnAllPurchasesRetrieved(purchasedProducts);
                }
            }
        }

        void FinishTransaction(GooglePurchase purchase)
        {
            Product product = m_StoreCallback.FindProductById(purchase.sku);
            if (product != null)
            {
                m_TransactionService.FinishTransaction(product.definition, purchase.purchaseToken);
            }
            else
            {
                m_StoreCallback.OnPurchaseFailed(new PurchaseFailureDescription(purchase.sku, PurchaseFailureReason.ProductUnavailable, "Product was not found but was purchased"));
            }
        }
    }
}
