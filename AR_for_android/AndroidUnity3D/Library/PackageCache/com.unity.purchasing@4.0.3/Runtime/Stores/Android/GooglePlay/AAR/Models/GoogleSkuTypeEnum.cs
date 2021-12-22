using UnityEngine;

namespace UnityEngine.Purchasing.Models
{
    /// <summary>
    /// This is C# representation of the Java Class SkuType
    /// <a href="https://developer.android.com/reference/com/android/billingclient/api/BillingClient.SkuType">See more</a>
    /// </summary>
    static class GoogleSkuTypeEnum
    {
        internal static string InApp()
        {
            return "inapp";
        }

        internal static string Sub()
        {
            return "subs";
        }
    }
}
