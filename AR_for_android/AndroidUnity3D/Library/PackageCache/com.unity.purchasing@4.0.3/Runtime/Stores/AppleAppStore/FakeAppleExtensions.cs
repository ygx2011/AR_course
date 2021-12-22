using System;
using System.Collections.Generic;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Provides fake functionality for Apple specific APIs.
    ///
    /// Refresh receipt calls alternate between success and failure.
    /// </summary>
    internal class FakeAppleExtensions : IAppleExtensions
    {
        private bool m_FailRefresh;

        public void RefreshAppReceipt(Action<string> successCallback, Action errorCallback)
        {
            if (m_FailRefresh)
                errorCallback();
            else
                successCallback("A fake refreshed receipt!");
            m_FailRefresh = !m_FailRefresh;
        }

        public void RestoreTransactions(Action<bool> callback)
        {
            callback(true);
        }

        public void RegisterPurchaseDeferredListener(Action<Product> callback)
        {
        }

        public bool simulateAskToBuy {
            get;
            set;
        }

        public void SetStorePromotionOrder(List<Product> products)
        {
        }

        public void SetStorePromotionVisibility(Product product, AppleStorePromotionVisibility visible)
        {
        }

        public void SetApplicationUsername (string applicationUsername)
        {
        }

        public string GetTransactionReceiptForProduct (Product product)
        {
            return "";
        }

        public void ContinuePromotionalPurchases()
        {
        }

        public Dictionary<string, string> GetIntroductoryPriceDictionary() {
            return new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetProductDetails() {
            return new Dictionary<string, string>();
        }

        public void PresentCodeRedemptionSheet()
        {
        }
    }
}
