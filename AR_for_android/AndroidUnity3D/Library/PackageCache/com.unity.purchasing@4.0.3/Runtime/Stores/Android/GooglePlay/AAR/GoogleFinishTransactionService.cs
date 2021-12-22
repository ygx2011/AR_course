using System;
using System.Linq;
using UnityEngine.Purchasing.Interfaces;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing
{
    class GoogleFinishTransactionService : IGoogleFinishTransactionService
    {
        IGoogleBillingClient m_BillingClient;
        IGoogleQueryPurchasesService m_GoogleQueryPurchasesService;
        internal GoogleFinishTransactionService(IGoogleBillingClient billingClient, IGoogleQueryPurchasesService googleQueryPurchasesService)
        {
            m_BillingClient = billingClient;
            m_GoogleQueryPurchasesService = googleQueryPurchasesService;
        }

        public void FinishTransaction(ProductDefinition product, string purchaseToken, Action<ProductDefinition, GooglePurchase, IGoogleBillingResult, string> onConsume, Action<ProductDefinition, GooglePurchase, IGoogleBillingResult> onAcknowledge)
        {
            m_GoogleQueryPurchasesService.QueryPurchases(purchases =>
            {
                foreach (var purchase in purchases.Where(PurchaseToFinishTransaction(product)))
                {
                    if (product.type == ProductType.Consumable)
                    {
                        m_BillingClient.ConsumeAsync(purchaseToken, product, purchase, onConsume);
                    }
                    else if (!purchase.IsAcknowledged())
                    {
                        m_BillingClient.AcknowledgePurchase(purchaseToken, product, purchase, onAcknowledge);
                    }
                }
            });
        }

        static Func<GooglePurchase, bool> PurchaseToFinishTransaction(ProductDefinition product)
        {
            return purchase => purchase != null && purchase.sku == product.storeSpecificId && purchase.IsPurchased();
        }
    }
}
