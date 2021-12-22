using UnityEngine.Purchasing.Utils;

namespace UnityEngine.Purchasing.Models
{
    /// <summary>
    /// This is C# representation of the Java Class Purchase
    /// <a href="https://developer.android.com/reference/com/android/billingclient/api/Purchase">See more</a>
    /// </summary>
    class GooglePurchase
    {
        public AndroidJavaObject javaPurchase;
        public int purchaseState;
        public string sku;
        public string orderId;
        public string receipt;
        public string signature;
        public string originalJson;
        public string purchaseToken;

        internal GooglePurchase() { }

        internal GooglePurchase(AndroidJavaObject purchase, AndroidJavaObject skuDetails)
        {
            if (purchase != null)
            {
                javaPurchase = purchase;
                purchaseState = purchase.Call<int>("getPurchaseState");
                sku = purchase.Call<string>("getSku");
                orderId = purchase.Call<string>("getOrderId");
                originalJson = purchase.Call<string>("getOriginalJson");
                signature = purchase.Call<string>("getSignature");
                purchaseToken = purchase.Call<string>("getPurchaseToken");
                string encodedReceipt = GoogleReceiptEncoder.EncodeReceipt(
                    purchaseToken,
                    originalJson,
                    signature,
                    skuDetails.Call<string>("getOriginalJson")
                );
                receipt = encodedReceipt;
            }
        }

        public virtual bool IsAcknowledged()
        {
            return javaPurchase != null && javaPurchase.Call<bool>("isAcknowledged");
        }

        public virtual bool IsPurchased()
        {
            return javaPurchase != null && purchaseState == GooglePurchaseStateEnum.Purchased();
        }
    }
}
