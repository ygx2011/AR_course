using System;
using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing
{
	/// <summary>
	/// Access GooglePlay store specific functionality.
	/// </summary>
	public interface IGooglePlayStoreExtensions : IStoreExtension
    {
        /// <summary>
        /// Upgrade or downgrade subscriptions, with proration mode `IMMEDIATE_WITHOUT_PRORATION` by default
        /// <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingFlowParams.ProrationMode">See more</a>
        /// </summary>
        /// <param name="oldSku">current subscription</param>
        /// <param name="newSku">new subscription to subscribe</param>
        void UpgradeDowngradeSubscription(string oldSku, string newSku);

        /// <summary>
        /// Upgrade or downgrade subscriptions
        /// </summary>
        /// <param name="oldSku">current subscription</param>
        /// <param name="newSku">new subscription to subscribe</param>
        /// <param name="desiredProrationMode">Specifies the mode of proration.
        /// <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingFlowParams.ProrationMode">See more</a>
        /// </param>
        void UpgradeDowngradeSubscription(string oldSku, string newSku, int desiredProrationMode);

        /// <summary>
        /// Upgrade or downgrade subscriptions
        /// </summary>
        /// <param name="oldSku">current subscription</param>
        /// <param name="newSku">new subscription to subscribe</param>
        /// <param name="desiredProrationMode">Specifies the mode of proration.
        /// <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingFlowParams.ProrationMode">See more</a>
        /// </param>
        void UpgradeDowngradeSubscription(string oldSku, string newSku, GooglePlayProrationMode desiredProrationMode);

        /// <summary>
        /// Async call to the google store to <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingClient#querypurchases">`queryPurchases`</a>
        /// using all the different support sku types.
        /// </summary>
        /// <param name="callback">Will be called as often as many purchases the queryPurchases finds. (the IStoreCallback will be called as well)</param>
        void RestoreTransactions(Action<bool> callback);

        /// <summary>
        /// Initiate a flow to confirm the change of price for an item subscribed by the user.
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <param name="callback">Price changed event finished successfully</param>
        void ConfirmSubscriptionPriceChange(string productId, Action<bool> callback);

        /// <summary>
        /// Determines if the purchase of a product in the Google Play Store is deferred based on its receipt. This indicates if there is an additional step to complete a transaction in between when a user initiates a purchase and when the payment method for the purchase is processed.
        /// <a href="https://developer.android.com/google/play/billing/integrate#pending">Handling pending transactions</a>
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns><c>true</c>if the input contains a receipt for a deferred or a pending transaction for a Google Play billing purchase, and <c>false</c> otherwise.</returns>
        bool IsPurchasedProductDeferred(Product product);
    }
}
