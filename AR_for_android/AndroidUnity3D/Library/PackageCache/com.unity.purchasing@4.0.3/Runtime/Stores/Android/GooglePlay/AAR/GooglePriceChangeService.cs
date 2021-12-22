using System;
using UnityEngine.Purchasing.Interfaces;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing
{
    class GooglePriceChangeService : IGooglePriceChangeService
    {
        IGoogleBillingClient m_BillingClient;
        IQuerySkuDetailsService m_QuerySkuDetailsService;

        internal GooglePriceChangeService(IGoogleBillingClient billingClient, IQuerySkuDetailsService querySkuDetailsService)
        {
            m_BillingClient = billingClient;
            m_QuerySkuDetailsService = querySkuDetailsService;
        }

        public void PriceChange(ProductDefinition product, Action<IGoogleBillingResult> onPriceChangedListener)
        {
            m_QuerySkuDetailsService.QueryAsyncSku(product, skuDetailsList =>
            {
                foreach (var skuDetails in skuDetailsList)
                {
                    m_BillingClient.LaunchPriceChangeConfirmationFlow(skuDetails, new GooglePriceChangeConfirmationListener(onPriceChangedListener));
                }
            });
        }
    }
}
