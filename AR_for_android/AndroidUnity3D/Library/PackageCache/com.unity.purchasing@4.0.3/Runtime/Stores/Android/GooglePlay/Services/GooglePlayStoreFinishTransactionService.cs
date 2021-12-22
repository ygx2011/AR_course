using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Interfaces;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing
{
    class GooglePlayStoreFinishTransactionService : IGooglePlayStoreFinishTransactionService
    {
        HashSet<string> m_ProcessedPurchaseToken;
        IGooglePlayStoreService m_GooglePlayStoreService;
        IStoreCallback m_StoreCallback;
        internal GooglePlayStoreFinishTransactionService(IGooglePlayStoreService googlePlayStoreService)
        {
            m_ProcessedPurchaseToken = new HashSet<string>();
            m_GooglePlayStoreService = googlePlayStoreService;
        }

        public void SetStoreCallback(IStoreCallback storeCallback)
        {
            m_StoreCallback = storeCallback;
        }

        public void FinishTransaction(ProductDefinition product, string purchaseToken)
        {
            m_GooglePlayStoreService.FinishTransaction(product, purchaseToken, OnConsume, OnAcknowledge);
        }

        public void OnConsume(ProductDefinition product, GooglePurchase googlePurchase, IGoogleBillingResult billingResult, string purchaseToken)
        {
            HandleFinishTransaction(product, googlePurchase, billingResult, purchaseToken);
        }

        public void OnAcknowledge(ProductDefinition product, GooglePurchase googlePurchase, IGoogleBillingResult billingResult)
        {
            HandleFinishTransaction(product, googlePurchase, billingResult, googlePurchase.purchaseToken);
        }

        public void HandleFinishTransaction(ProductDefinition product, GooglePurchase googlePurchase, IGoogleBillingResult billingResult, string purchaseToken)
        {
            if (!m_ProcessedPurchaseToken.Contains(purchaseToken))
            {
                if (billingResult.responseCode == GoogleBillingResponseCode.Ok)
                {
                    m_ProcessedPurchaseToken.Add(purchaseToken);
                    CallPurchaseSucceededUpdateReceipt(product, googlePurchase, purchaseToken);
                }
                else if (IsResponseCodeInRecoverableState(billingResult))
                {
                    FinishTransaction(product, purchaseToken);
                }
                else
                {
                    m_StoreCallback?.OnPurchaseFailed(
                        new PurchaseFailureDescription(
                            product.storeSpecificId,
                            PurchaseFailureReason.Unknown,
                            billingResult.debugMessage + " {code: " + billingResult.responseCode + ", M: GPSFTS.HFT}"
                        )
                    );
                }
            }
        }

        void CallPurchaseSucceededUpdateReceipt(ProductDefinition product, GooglePurchase googlePurchase, string purchaseToken)
        {
            m_StoreCallback?.OnPurchaseSucceeded(
                product.storeSpecificId,
                googlePurchase.receipt,
                purchaseToken
            );
        }

        static bool IsResponseCodeInRecoverableState(IGoogleBillingResult billingResult)
        {
            // DeveloperError is only a possible recoverable state because of this
            // https://github.com/android/play-billing-samples/issues/337
            // usually works like a charm next acknowledge
            return billingResult.responseCode == GoogleBillingResponseCode.ServiceUnavailable ||
                billingResult.responseCode == GoogleBillingResponseCode.DeveloperError ||
                billingResult.responseCode == GoogleBillingResponseCode.FatalError;
        }
    }
}
