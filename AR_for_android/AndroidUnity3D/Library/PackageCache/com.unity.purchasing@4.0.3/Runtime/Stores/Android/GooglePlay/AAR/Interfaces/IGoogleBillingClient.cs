using System;
using System.Collections.Generic;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing.Interfaces
{
    interface IGoogleBillingClient
    {
        void StartConnection(IBillingClientStateListener billingClientStateListener);
        void EndConnection();
        AndroidJavaObject QueryPurchase(string skuType);
        void QuerySkuDetailsAsync(List<string> skus, string type, Action<IGoogleBillingResult, List<AndroidJavaObject>> onSkuDetailsResponseAction);
        AndroidJavaObject LaunchBillingFlow(AndroidJavaObject sku, string oldSku, string oldPurchaseToken, GooglePlayProrationMode? prorationMode);
        void ConsumeAsync(string purchaseToken, ProductDefinition product, GooglePurchase googlePurchase, Action<ProductDefinition, GooglePurchase, IGoogleBillingResult, string> onConsume);
        void AcknowledgePurchase(string purchaseToken, ProductDefinition product, GooglePurchase googlePurchase, Action<ProductDefinition, GooglePurchase, IGoogleBillingResult> onAcknowledge);
        void SetObfuscationAccountId(string obfuscationAccountId);
        void SetObfuscationProfileId(string obfuscationProfileId);
        void LaunchPriceChangeConfirmationFlow(AndroidJavaObject skuDetails, GooglePriceChangeConfirmationListener listener);
    }
}
