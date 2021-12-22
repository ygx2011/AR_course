using System;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Product definition used by Google Play Store.
    /// This is a representation of SkuDetails
    /// <a href="https://developer.android.com/reference/com/android/billingclient/api/SkuDetails">Google documentation</a>
    /// </summary>
    public class GoogleProductMetadata : ProductMetadata
    {
        /// <summary>
        /// Returns a String in JSON format that contains SKU details.
        /// </summary>
        public string originalJson { get; internal set; }
        /// <summary>
        /// Subscription period, specified in ISO 8601 format.
        /// </summary>
        public string subscriptionPeriod { get; internal set; }
        /// <summary>
        /// Trial period configured in Google Play Console, specified in ISO 8601 format.
        /// </summary>
        public string freeTrialPeriod { get; internal set; }
        /// <summary>
        /// Formatted introductory price of a subscription, including its currency sign, such as €3.99.
        /// </summary>
        public string introductoryPrice { get; internal set; }
        /// <summary>
        /// The billing period of the introductory price, specified in ISO 8601 format.
        /// </summary>
        public string introductoryPricePeriod { get; internal set; }
        /// <summary>
        /// The number of subscription billing periods for which the user will be given the introductory price, such as 3.
        /// </summary>
        public int introductoryPriceCycles { get; internal set; }

        internal GoogleProductMetadata(string priceString, string title, string description, string currencyCode, decimal localizedPrice)
            : base(priceString, title, description, currencyCode, localizedPrice)
        { }
    }
}
