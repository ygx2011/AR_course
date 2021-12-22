using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Interfaces;

namespace UnityEngine.Purchasing
{
    class GooglePlayPurchaseCallback: IGooglePurchaseCallback
    {
        IStoreCallback m_StoreCallback;
        IGooglePlayConfigurationInternal m_GooglePlayConfigurationInternal;

        public void SetStoreCallback(IStoreCallback storeCallback)
        {
            m_StoreCallback = storeCallback;
        }

        public void SetStoreConfiguration(IGooglePlayConfigurationInternal configuration)
        {
            m_GooglePlayConfigurationInternal = configuration;
        }

        public void OnPurchaseSuccessful(string sku, string receipt, string purchaseToken)
        {
            m_StoreCallback?.OnPurchaseSucceeded(sku, receipt, purchaseToken);
        }

        public void OnPurchaseFailed(PurchaseFailureDescription purchaseFailureDescription)
        {
            m_StoreCallback?.OnPurchaseFailed(purchaseFailureDescription);
        }

        public void NotifyDeferredPurchase(string sku, string receipt, string purchaseToken)
        {
            m_GooglePlayConfigurationInternal?.NotifyDeferredPurchase(m_StoreCallback, sku, receipt, purchaseToken);
        }

        public void NotifyDeferredProrationUpgradeDowngradeSubscription(string sku)
        {
            m_GooglePlayConfigurationInternal?.NotifyDeferredProrationUpgradeDowngradeSubscription(m_StoreCallback, sku);
        }
    }
}
