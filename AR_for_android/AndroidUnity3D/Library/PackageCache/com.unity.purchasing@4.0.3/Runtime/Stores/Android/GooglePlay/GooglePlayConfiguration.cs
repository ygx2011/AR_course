using System;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Interfaces;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Access Google Play store specific configurations.
    /// </summary>
    class GooglePlayConfiguration: IGooglePlayConfiguration, IGooglePlayConfigurationInternal
    {
        Action m_InitializationConnectionLister;

        IGooglePlayStoreService m_GooglePlayStoreService;
        Action<Product> m_DeferredPurchaseAction;
        Action<Product> m_DeferredProrationUpgradeDowngradeSubscriptionAction;

        public GooglePlayConfiguration(IGooglePlayStoreService googlePlayStoreService)
        {
            m_GooglePlayStoreService = googlePlayStoreService;
        }

        /// <summary>
        /// Set an optional listener for failures when connecting to the base Google Play Billing service. This may be called
        /// after <typeparamref name="UnityPurchasing.Initialize"/> if a user does not have a Google account added to their
        /// Android device.
        /// </summary>
        /// <param name="action">Will be called when <typeparamref name="UnityPurchasing.Initialize"/>
        ///     is interrupted by a disconnection from the Google Play Billing service.</param>
        public void SetServiceDisconnectAtInitializeListener(Action action)
        {
            m_InitializationConnectionLister = action;
        }

        /// <summary>
        /// Internal API, do not use.
        /// </summary>
        public void NotifyInitializationConnectionFailed()
        {
            m_InitializationConnectionLister?.Invoke();
        }

        /// <summary>
        /// Set listener for deferred purchasing events.
        /// Deferred purchasing is enabled by default and cannot be changed.
        /// </summary>
        /// <param name="action">Deferred purchasing successful events. Do not grant the item here. Instead, record the purchase and remind the user to complete the transaction in the Play Store. </param>
        public void SetDeferredPurchaseListener(Action<Product> action)
        {
            m_DeferredPurchaseAction = action;
        }

        public void NotifyDeferredProrationUpgradeDowngradeSubscription(IStoreCallback storeCallback, string productId)
        {
            Product product = storeCallback.FindProductById(productId);
            if (product != null)
            {
                m_DeferredProrationUpgradeDowngradeSubscriptionAction?.Invoke(product);
            }
        }

        public void NotifyDeferredPurchase(IStoreCallback storeCallback, string productId, string receipt, string transactionId)
        {
            Product product = storeCallback.FindProductById(productId);
            if (product != null)
            {
                ProductPurchaseUpdater.UpdateProductReceiptAndTransactionID(product, receipt, transactionId, GooglePlay.Name);
                m_DeferredPurchaseAction?.Invoke(product);
            }
        }

        /// <summary>
        /// Set listener for deferred subscription change events.
        /// Deferred subscription changes only take effect at the renewal cycle and no transaction is done immediately, therefore there is no receipt nor token.
        /// </summary>
        /// <param name="action">Deferred subscription change event. No payout is granted here. Instead, notify the user that the subscription change will take effect at the next renewal cycle. </param>
        public void SetDeferredProrationUpgradeDowngradeSubscriptionListener(Action<Product> action)
        {
            m_DeferredProrationUpgradeDowngradeSubscriptionAction = action;
        }

        /// <summary>
        /// Optional obfuscation string to detect irregular activities when making a purchase.
        /// For more information please visit <a href="https://developer.android.com/google/play/billing/security">https://developer.android.com/google/play/billing/security</a>
        /// </summary>
        /// <param name="accountId">The obfuscated account id</param>
        public void SetObfuscatedAccountId(string accountId)
        {
            m_GooglePlayStoreService.SetObfuscatedAccountId(accountId);
        }

        /// <summary>
        /// Optional obfuscation string to detect irregular activities when making a purchase
        /// For more information please visit <a href="https://developer.android.com/google/play/billing/security">https://developer.android.com/google/play/billing/security</a>
        /// </summary>
        /// <param name="profileId">The obfuscated profile id</param>
        public void SetObfuscatedProfileId(string profileId)
        {
            m_GooglePlayStoreService.SetObfuscatedProfileId(profileId);
        }
    }
}
