namespace UnityEngine.Purchasing
{
    /// <summary>
    /// The Google Play proration mode used when upgrading and downgrading subscription.
    /// <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingFlowParams.ProrationMode"> See more </a>
    /// </summary>
    public enum GooglePlayProrationMode
    {
        /// <summary>
        /// Unknown subscription upgrade downgrade policy
        /// </summary>
        /// <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingFlowParams.ProrationMode#UNKNOWN_SUBSCRIPTION_UPGRADE_DOWNGRADE_POLICY"> See more </a>
        UnknownSubscriptionUpgradeDowngradePolicy = 0,

        /// <summary>
        /// Replacement takes effect immediately, and the remaining time will be prorated and credited to the user. This is the current default behavior.
        /// <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingFlowParams.ProrationMode#IMMEDIATE_WITH_TIME_PRORATION"> See more </a>
        /// </summary>
        ImmediateWithTimeProration = 1,

        /// <summary>
        /// Replacement takes effect immediately, and the billing cycle remains the same. The price for the remaining period will be charged. This option is only available for subscription upgrade.
        /// <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingFlowParams.ProrationMode#IMMEDIATE_AND_CHARGE_PRORATED_PRICE"> See more </a>
        /// </summary>
        ImmediateAndChargeProratedPrice = 2,

        /// <summary>
        /// Replacement takes effect immediately, and the new price will be charged on next recurrence time. The billing cycle stays the same.
        /// <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingFlowParams.ProrationMode#IMMEDIATE_WITHOUT_PRORATION"> See more </a>
        /// </summary>
        ImmediateWithoutProration = 3,

        /// <summary>
        /// Replacement takes effect when the old plan expires, and the new price will be charged at the same time.
        /// <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingFlowParams.ProrationMode#DEFERRED"> See more </a>
        /// </summary>
        Deferred = 4,

        /// <summary>
        /// Replacement takes effect immediately, and the user is charged full price of new plan and is given a full billing cycle of subscription, plus remaining prorated time from the old plan.
        /// <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingFlowParams.ProrationMode#IMMEDIATE_AND_CHARGE_FULL_PRICE"> See more </a>
        /// </summary>
        ImmediateAndChargeFullPrice = 5,
    }
}
