using System;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Access Google Play store specific configurations.
    /// </summary>
    public class FakeGooglePlayStoreConfiguration : IGooglePlayConfiguration
    {
        /// <summary>
        /// THIS IS A FAKE, NO CODE WILL BE EXECUTED!
        ///
        /// Set an optional listener for failures when connecting to the base Google Play Billing service.
        /// </summary>
        /// <param name="action">Will never be called because this is a fake.</param>
        public void SetServiceDisconnectAtInitializeListener(Action action) { }

        /// <summary>
        /// THIS IS A FAKE, NO CODE WILL BE EXECUTED!
        ///
        /// Set listener for deferred purchasing events.
        /// Deferred purchasing is enabled by default and cannot be changed.
        /// </summary>
        /// <param name="action">Deferred purchasing successful events. Do not grant the item here. Instead, record the purchase and remind the user to complete the transaction in the Play Store. </param>
        public void SetDeferredPurchaseListener(Action<Product> action) { }

        /// <summary>
        /// THIS IS A FAKE, NO CODE WILL BE EXECUTED!
        ///
        /// Set listener for deferred subscription change events.
        /// Deferred subscription changes only take effect at the renewal cycle and no transaction is done immediately, therefore there is no receipt nor token.
        /// </summary>
        /// <param name="action">Deferred subscription change event. No payout is granted here. Instead, notify the user that the subscription change will take effect at the next renewal cycle. </param>
        public void SetDeferredProrationUpgradeDowngradeSubscriptionListener(Action<Product> action) { }

        /// <summary>
        /// THIS IS A FAKE, NO CODE WILL BE EXECUTED!
        ///
        /// Optional obfuscation string to detect irregular activities when making a purchase.
        /// For more information please visit <a href="https://developer.android.com/google/play/billing/security">https://developer.android.com/google/play/billing/security</a>
        /// </summary>
        /// <param name="accountId">The obfuscated account id</param>
        public void SetObfuscatedAccountId(string accountId) { }

        /// <summary>
        /// THIS IS A FAKE, NO CODE WILL BE EXECUTED!
        ///
        /// Optional obfuscation string to detect irregular activities when making a purchase
        /// For more information please visit <a href="https://developer.android.com/google/play/billing/security">https://developer.android.com/google/play/billing/security</a>
        /// </summary>
        /// <param name="profileId">The obfuscated profile id</param>
        public void SetObfuscatedProfileId(string profileId) { }
    }
}
