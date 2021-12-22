using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Interfaces;
using UnityEngine.Purchasing.Models;
using UnityEngine.Purchasing.Utils;
using UnityEngine.Scripting;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// This is C# representation of the Java Class PurchasesUpdatedListener
    /// <a href="https://developer.android.com/reference/com/android/billingclient/api/PurchasesUpdatedListener">See more</a>
    /// </summary>
    class GooglePurchaseUpdatedListener: AndroidJavaProxy, IGooglePurchaseUpdatedListener
    {
        const string k_AndroidPurchaseListenerClassName = "com.android.billingclient.api.PurchasesUpdatedListener";

        IGoogleLastKnownProductService m_LastKnownProductService;
        IGooglePurchaseCallback m_GooglePurchaseCallback;
        IGoogleCachedQuerySkuDetailsService m_GoogleCachedQuerySkuDetailsService;
        internal GooglePurchaseUpdatedListener(IGoogleLastKnownProductService googleLastKnownProductService, IGooglePurchaseCallback googlePurchaseCallback, IGoogleCachedQuerySkuDetailsService googleCachedQuerySkuDetailsService): base(k_AndroidPurchaseListenerClassName)
        {
            m_LastKnownProductService = googleLastKnownProductService;
            m_GooglePurchaseCallback = googlePurchaseCallback;
            m_GoogleCachedQuerySkuDetailsService = googleCachedQuerySkuDetailsService;
        }

        /// <summary>
        /// Implementation of com.android.billingclient.api.PurchasesUpdatedListener#onPurchasesUpdated
        /// </summary>
        /// <param name="billingResult"></param>
        /// <param name="javaPurchasesList"></param>
        [Preserve]
        void onPurchasesUpdated(AndroidJavaObject billingResult, AndroidJavaObject javaPurchasesList)
        {
            IGoogleBillingResult result = new GoogleBillingResult(billingResult);
            var purchases = javaPurchasesList.Enumerate<AndroidJavaObject>();

            if (result.responseCode == GoogleBillingResponseCode.Ok)
            {
                HandleResultOkCases(result, purchases);
            }
            else if (result.responseCode == GoogleBillingResponseCode.UserCanceled && purchases.Any())
            {
                ApplyOnPurchases(purchases, OnPurchaseCanceled);
            }
            else if (result.responseCode == GoogleBillingResponseCode.ItemAlreadyOwned && purchases.Any())
            {
                ApplyOnPurchases(purchases, OnPurchaseAlreadyOwned);
            }
            else
            {
                HandleErrorCases(result, purchases);
            }
        }

        void HandleResultOkCases(IGoogleBillingResult result, IEnumerable<AndroidJavaObject> purchases)
        {
            if (purchases.Any())
            {
                ApplyOnPurchases(purchases, OnPurchaseOk);
            }
            else if (IsLastProrationModeDeferred())
            {
                OnDeferredProrationUpgradeDowngradeSubscriptionOk();
            }
            else
            {
                HandleErrorCases(result, purchases);
            }
        }

        void HandleErrorCases(IGoogleBillingResult billingResult, IEnumerable<AndroidJavaObject> purchases)
        {
            if (!purchases.Any())
            {
                if (billingResult.responseCode == GoogleBillingResponseCode.ItemAlreadyOwned)
                {
                    m_GooglePurchaseCallback.OnPurchaseFailed(
                        new PurchaseFailureDescription(
                            m_LastKnownProductService.GetLastKnownProductId(),
                            PurchaseFailureReason.DuplicateTransaction,
                            billingResult.debugMessage
                        )
                    );
                }
                else if (billingResult.responseCode == GoogleBillingResponseCode.UserCanceled)
                {
                    m_GooglePurchaseCallback.OnPurchaseFailed(
                        new PurchaseFailureDescription(
                            m_LastKnownProductService.GetLastKnownProductId(),
                            PurchaseFailureReason.UserCancelled,
                            billingResult.debugMessage
                        )
                    );
                }
                else
                {
                    m_GooglePurchaseCallback.OnPurchaseFailed(
                        new PurchaseFailureDescription(
                            m_LastKnownProductService.GetLastKnownProductId(),
                            PurchaseFailureReason.Unknown,
                            billingResult.debugMessage + " {M: GPUL.HEC} - Response Code = " + billingResult.responseCode
                        )
                    );
                }
            }
            else
            {
                ApplyOnPurchases(purchases, billingResult, OnPurchaseFailed);
            }
        }

        void ApplyOnPurchases(IEnumerable<AndroidJavaObject> purchases, Action<GooglePurchase> action)
        {
            foreach (var purchase in purchases)
            {
                GooglePurchase googlePurchase = GooglePurchaseHelper.MakeGooglePurchase(m_GoogleCachedQuerySkuDetailsService.GetCachedQueriedSkus().ToList(), purchase);
                action(googlePurchase);
            }

        }

        void ApplyOnPurchases(IEnumerable<AndroidJavaObject> purchases, IGoogleBillingResult billingResult, Action<GooglePurchase, string> action)
        {
            foreach (var purchase in purchases)
            {
                GooglePurchase googlePurchase = GooglePurchaseHelper.MakeGooglePurchase(m_GoogleCachedQuerySkuDetailsService.GetCachedQueriedSkus().ToList(), purchase);
                action(googlePurchase, billingResult.debugMessage);
            }
        }

        bool IsLastProrationModeDeferred()
        {
            return m_LastKnownProductService.GetLastKnownProrationMode() == GooglePlayProrationMode.Deferred;
        }

        void OnPurchaseOk(GooglePurchase googlePurchase)
        {
            if (googlePurchase.purchaseState == GooglePurchaseStateEnum.Purchased())
            {
                m_GooglePurchaseCallback.OnPurchaseSuccessful(googlePurchase.sku, googlePurchase.receipt, googlePurchase.purchaseToken);
            }
            else if (googlePurchase.purchaseState == GooglePurchaseStateEnum.Pending())
            {
                m_GooglePurchaseCallback.NotifyDeferredPurchase(googlePurchase.sku, googlePurchase.receipt, googlePurchase.purchaseToken);
            }
            else
            {
                m_GooglePurchaseCallback.OnPurchaseFailed(
                    new PurchaseFailureDescription(
                        googlePurchase.sku,
                        PurchaseFailureReason.Unknown,
                        GoogleBillingStrings.errorPurchaseStateUnspecified + " {M: GPUL.OPO}"
                    )
                );
            }
        }
        void OnDeferredProrationUpgradeDowngradeSubscriptionOk()
        {
            m_GooglePurchaseCallback.NotifyDeferredProrationUpgradeDowngradeSubscription(m_LastKnownProductService.GetLastKnownProductId());
        }

        void OnPurchaseCanceled(GooglePurchase googlePurchase)
        {
            m_GooglePurchaseCallback.OnPurchaseFailed(
                new PurchaseFailureDescription(
                    googlePurchase.sku,
                    PurchaseFailureReason.UserCancelled,
                    GoogleBillingStrings.errorUserCancelled
                )
            );
        }

        void OnPurchaseAlreadyOwned(GooglePurchase googlePurchase)
        {
            m_GooglePurchaseCallback.OnPurchaseFailed(
                new PurchaseFailureDescription(
                    googlePurchase.sku,
                    PurchaseFailureReason.DuplicateTransaction,
                    GoogleBillingStrings.errorItemAlreadyOwned
                )
            );
        }

        void OnPurchaseFailed(GooglePurchase googlePurchase, string debugMessage)
        {
            m_GooglePurchaseCallback.OnPurchaseFailed(
                new PurchaseFailureDescription(
                    googlePurchase.sku,
                    PurchaseFailureReason.Unknown,
                    debugMessage + " {M: GPUL.OPF}"
                )
            );
        }
    }
}
