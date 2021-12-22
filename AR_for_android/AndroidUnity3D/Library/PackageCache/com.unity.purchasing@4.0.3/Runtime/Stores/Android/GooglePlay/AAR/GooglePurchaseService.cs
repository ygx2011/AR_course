using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Interfaces;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing
{
    class GooglePurchaseService : IGooglePurchaseService
    {
        IGoogleBillingClient m_BillingClient;
        IGooglePurchaseCallback m_GooglePurchaseCallback;
        IQuerySkuDetailsService m_QuerySkuDetailsService;

        internal GooglePurchaseService(IGoogleBillingClient billingClient, IGooglePurchaseCallback googlePurchaseCallback, IQuerySkuDetailsService querySkuDetailsService)
        {
            m_BillingClient = billingClient;
            m_GooglePurchaseCallback = googlePurchaseCallback;
            m_QuerySkuDetailsService = querySkuDetailsService;
        }

        public void Purchase(ProductDefinition product, Product oldProduct, GooglePlayProrationMode? desiredProrationMode)
        {
            m_QuerySkuDetailsService.QueryAsyncSku(product,
                skus =>
                {
                    OnQuerySkuDetailsResponse(skus, product, oldProduct, desiredProrationMode);
                });
        }

        void OnQuerySkuDetailsResponse(List<AndroidJavaObject> skus, ProductDefinition productToBuy, Product oldProduct, GooglePlayProrationMode? desiredProrationMode)
        {
            if (skus?.Count > 0)
            {
                AndroidJavaObject sku = skus[0];
                VerifyAndWarnIfMoreThanOneSku(skus, sku);
                AndroidJavaObject billingResult = m_BillingClient.LaunchBillingFlow(sku, oldProduct?.definition?.storeSpecificId, oldProduct?.transactionID, desiredProrationMode);
                HandleBillingFlowResult(new GoogleBillingResult(billingResult), sku);
            }
            else
            {
                m_GooglePurchaseCallback.OnPurchaseFailed(
                    new PurchaseFailureDescription(
                        productToBuy.id,
                        PurchaseFailureReason.ProductUnavailable,
                        "SKU does not exist in the store."
                    )
                );
            }
        }

        static void VerifyAndWarnIfMoreThanOneSku(List<AndroidJavaObject> skus, AndroidJavaObject sku)
        {
            if (skus.Count > 1)
            {
                Debug.LogWarning(GoogleBillingStrings.getWarningMessageMoreThanOneSkuFound(sku.Call<string>("getSku")));
            }
        }

        void HandleBillingFlowResult(IGoogleBillingResult billingResult, AndroidJavaObject sku)
        {
            if (billingResult.responseCode != GoogleBillingResponseCode.Ok)
            {
                m_GooglePurchaseCallback.OnPurchaseFailed(
                    new PurchaseFailureDescription(
                        sku.Call<string>("getSku"),
                        PurchaseFailureReason.PurchasingUnavailable,
                        billingResult.debugMessage
                    )
                );
            }
        }
    }
}
