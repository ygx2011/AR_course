using System.Collections.Generic;
using System.Linq;
using UnityEngine.Purchasing.Utils;

namespace UnityEngine.Purchasing.Models
{
    /// <summary>
    /// This is C# representation of the Java Class PurchasesResult
    /// <a href="https://developer.android.com/reference/com/android/billingclient/api/Purchase.PurchasesResult">See more</a>
    /// </summary>
    class GooglePurchaseResult
    {
        internal GoogleBillingResponseCode m_ResponseCode;
        internal List<GooglePurchase> m_Purchases = new List<GooglePurchase>();

        internal GooglePurchaseResult(AndroidJavaObject purchaseResult, IGoogleCachedQuerySkuDetailsService cachedQuerySkuDetailsService)
        {
            m_ResponseCode = (GoogleBillingResponseCode) purchaseResult.Call<int>("getResponseCode");
            FillPurchases(purchaseResult, cachedQuerySkuDetailsService);
        }

        void FillPurchases(AndroidJavaObject purchaseResult, IGoogleCachedQuerySkuDetailsService cachedQuerySkuDetailsService)
        {
            AndroidJavaObject purchaseList = purchaseResult.Call<AndroidJavaObject>("getPurchasesList");

            var purchases = purchaseList.Enumerate<AndroidJavaObject>().ToList();
            for (var index = 0; index < purchases.Count; index++)
            {
                var purchase = purchases[index];
                if (purchase != null)
                {
                    m_Purchases.Add(GooglePurchaseHelper.MakeGooglePurchase(cachedQuerySkuDetailsService.GetCachedQueriedSkus().ToList(), purchase));
                }
                else
                {
                    Debug.LogWarning("Failed to retrieve Purchase from Purchase List at index " + index + " of " + purchases.Count + ". FillPurchases will skip this item");
                }
            }
        }
    }
}
