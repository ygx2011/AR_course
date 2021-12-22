namespace UnityEngine.Purchasing.Models
{
    /// <summary>
    /// This is C# representation of the Java Class PurchaseState
    /// <a href="https://developer.android.com/reference/com/android/billingclient/api/Purchase.PurchaseState">See more</a>
    /// </summary>
    class GooglePurchaseStateEnum
    {
        const string k_AndroidPurchaseStateClassName = "com.android.billingclient.api.Purchase$PurchaseState";

        static AndroidJavaObject GetPurchaseStateJavaObject()
        {
            return new AndroidJavaClass(k_AndroidPurchaseStateClassName);
        }

        internal static int Purchased()
        {
            return GetPurchaseStateJavaObject().GetStatic<int>("PURCHASED");
        }

        internal static int Pending()
        {
            return GetPurchaseStateJavaObject().GetStatic<int>("PENDING");
        }
    }
}
