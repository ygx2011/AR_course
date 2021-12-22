using System;
using UnityEngine.Purchasing.Models;
using UnityEngine.Scripting;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// This is C# representation of the Java Class AcknowledgePurchaseResponseListener
    /// <a href="https://developer.android.com/reference/com/android/billingclient/api/AcknowledgePurchaseResponseListener">See more</a>
    /// </summary>
    class GoogleAcknowledgePurchaseListener : AndroidJavaProxy
    {
        const string k_AndroidAcknowledgePurchaseResponseListenerClassName = "com.android.billingclient.api.AcknowledgePurchaseResponseListener";

        Action<ProductDefinition, GooglePurchase, IGoogleBillingResult> m_OnAcknowledgePurchaseResponse;

        ProductDefinition m_Product;
        GooglePurchase m_Purchase;
        internal GoogleAcknowledgePurchaseListener(ProductDefinition product, GooglePurchase purchase, Action<ProductDefinition, GooglePurchase, IGoogleBillingResult> onAcknowledgePurchaseResponseAction)
            : base(k_AndroidAcknowledgePurchaseResponseListenerClassName)
        {
            m_Product = product;
            m_Purchase = purchase;
            m_OnAcknowledgePurchaseResponse = onAcknowledgePurchaseResponseAction;
        }

        [Preserve]
        void onAcknowledgePurchaseResponse(AndroidJavaObject billingResult)
        {
            m_OnAcknowledgePurchaseResponse(m_Product, m_Purchase, new GoogleBillingResult(billingResult));
        }
    }
}
