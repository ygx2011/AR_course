namespace UnityEngine.Purchasing.Models
{
    /// <summary>
    /// Values from Java Class BillingResponseCode
    /// <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingClient.BillingResponseCode">See more</a>
    /// </summary>
    enum GoogleBillingResponseCode
    {
        Ok = 0,
        UserCanceled = 1,
        ServiceUnavailable = 2,
        DeveloperError = 5,
        FatalError = 6,
        ItemAlreadyOwned = 7,
    }
}
