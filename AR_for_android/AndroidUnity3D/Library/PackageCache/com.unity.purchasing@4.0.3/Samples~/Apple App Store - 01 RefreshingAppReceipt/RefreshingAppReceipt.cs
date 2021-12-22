using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Samples.Purchasing.AppleAppStore.RefreshingAppReceipt
{
    [RequireComponent(typeof(UserWarningAppleAppStore))]
    public class RefreshingAppReceipt : MonoBehaviour, IStoreListener
    {
        IStoreController m_StoreController;
        IAppleExtensions m_AppleExtensions;

        public string noAdsProductId = "com.mycompany.mygame.no_ads";

        public Text hasNoAdsText;

        public Text refreshReceiptText;

        void Start()
        {
            InitializePurchasing();
            UpdateWarningMessage();
        }

        void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(noAdsProductId, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("In-App Purchasing successfully initialized");

            m_StoreController = controller;
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();

            UpdateUI();
        }

        public void Refresh()
        {
            m_AppleExtensions.RefreshAppReceipt(OnRefreshSuccess, OnRefreshFailure);
        }

        void OnRefreshSuccess(string receipt)
        {
            // This does not mean anything was modified,
            // merely that the refresh process succeeded.
            // For information on parsing receipts, see:
            // https://developer.apple.com/library/archive/releasenotes/General/ValidateAppStoreReceipt/Chapters/ValidateLocally.html#//apple_ref/doc/uid/TP40010573-CH1-SW2
            // as well as:
            // https://docs.unity3d.com/Manual/UnityIAPValidatingReceipts.html
            Debug.Log("Refresh Successful");
            refreshReceiptText.text = "Refresh Successful";
        }

        void OnRefreshFailure()
        {
            Debug.Log("Refresh Failed");
            refreshReceiptText.text = "Refresh Failed";
        }

        public void BuyNoAds()
        {
            m_StoreController.InitiatePurchase(noAdsProductId);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            var product = args.purchasedProduct;

            Debug.Log($"Processing Purchase: {product.definition.id}");
            UpdateUI();

            return PurchaseProcessingResult.Complete;
        }

        void UpdateUI()
        {
            hasNoAdsText.text = HasNoAds() ? "No ads will be shown" : "Ads will be shown";
        }

        bool HasNoAds()
        {
            var noAdsProduct = m_StoreController.products.WithID(noAdsProductId);
            return noAdsProduct != null && noAdsProduct.hasReceipt;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log($"In-App Purchasing initialize failed: {error}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
        }

        void UpdateWarningMessage()
        {
            GetComponent<UserWarningAppleAppStore>().UpdateWarningText();
        }
    }
}
