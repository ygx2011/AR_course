using System;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Access Google Play store specific configurations.
    /// </summary>
    public interface IGooglePlayConfiguration: IStoreConfiguration
    {
        /// <summary>
        /// Set an optional listener for failures when connecting to the base Google Play Billing service. This may be called
        /// after <typeparamref name="UnityPurchasing.Initialize"/> if a user does not have a Google account added to their
        /// Android device.
        ///
        /// This listener can be used to learn that initialization is paused, and the user must add a Google account
        /// in order to be able to purchase and to download previous purchases. Adding a valid account will allow
        /// the initialization to resume.
        /// </summary>
        /// <param name="action">Will be called when <typeparamref name="UnityPurchasing.Initialize"/>
        ///     is interrupted by a disconnection from the Google Play Billing service.</param>
        void SetServiceDisconnectAtInitializeListener(Action action);

        /// <summary>
        /// Set listener for deferred purchasing events.
        /// Deferred purchasing is enabled by default and cannot be changed.
        /// </summary>
        /// <param name="action">Deferred purchasing successful events. Do not grant the item here. Instead, record the purchase and remind the user to complete the transaction in the Play Store. </param>
        void SetDeferredPurchaseListener(Action<Product> action);

        /// <summary>
        /// Set listener for deferred subscription change events.
        /// Deferred subscription changes only take effect at the renewal cycle and no transaction is done immediately, therefore there is no receipt nor token.
        /// </summary>
        /// <param name="action">Deferred subscription change event. No payout is granted here. Instead, notify the user that the subscription change will take effect at the next renewal cycle. </param>
        void SetDeferredProrationUpgradeDowngradeSubscriptionListener(Action<Product> action);

        /// <summary>
        /// Optional obfuscation string to detect irregular activities when making a purchase.
        /// For more information please visit <a href="https://developer.android.com/google/play/billing/security">https://developer.android.com/google/play/billing/security</a>
        /// </summary>
        /// <param name="accountId">The obfuscated account id</param>
        void SetObfuscatedAccountId(string accountId);

        /// <summary>
        /// Optional obfuscation string to detect irregular activities when making a purchase
        /// For more information please visit <a href="https://developer.android.com/google/play/billing/security">https://developer.android.com/google/play/billing/security</a>
        /// </summary>
        /// <param name="profileId">The obfuscated profile id</param>
        void SetObfuscatedProfileId(string profileId);
    }
}
