using System;
using UnityEngine.Purchasing.Models;
using UnityEngine.Scripting;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// This is C# representation of the Java Class ConsumeResponseListener
    /// <a href="https://developer.android.com/reference/com/android/billingclient/api/ConsumeResponseListener">See more</a>
    /// </summary>
    class GoogleConsumeResponseListener : AndroidJavaProxy
    {
        const string k_AndroidConsumeResponseListenerClassName = "com.android.billingclient.api.ConsumeResponseListener";

        ProductDefinition m_Product;
        GooglePurchase m_Purchase;
        Action<ProductDefinition, GooglePurchase, IGoogleBillingResult, string> m_OnConsumeResponse;

        internal GoogleConsumeResponseListener(ProductDefinition product, GooglePurchase purchase, Action<ProductDefinition, GooglePurchase, IGoogleBillingResult, string> onConsumeResponseAction)
            : base(k_AndroidConsumeResponseListenerClassName)
        {
            m_Product = product;
            m_Purchase = purchase;
            m_OnConsumeResponse = onConsumeResponseAction;
        }

        [Preserve]
        void onConsumeResponse(AndroidJavaObject billingResult, string purchaseToken)
        {
            m_OnConsumeResponse(m_Product, m_Purchase, new GoogleBillingResult(billingResult), purchaseToken);
        }
    }
}
