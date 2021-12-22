using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Samples.Purchasing.AppleAppStore.RestoringTransactions
{
    [RequireComponent(typeof(UserWarningAppleAppStore))]
    public class RestoringTransactions : MonoBehaviour, IStoreListener
    {
        IStoreController m_StoreController;
        IAppleExtensions m_AppleExtensions;

        public string noAdsProductId = "com.mycompany.mygame.no_ads";

        public Text hasNoAdsText;

        public Text restoreStatusText;

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

        public void Restore()
        {
            m_AppleExtensions.RestoreTransactions(OnRestore);
        }

        void OnRestore(bool success)
        {
            var restoreMessage = "";
            if (success)
            {
                // This does not mean anything was restored,
                // merely that the restoration process succeeded.
                restoreMessage = "Restore Successful";
            }
            else
            {
                // Restoration failed.
                restoreMessage = "Restore Failed";
            }

            Debug.Log(restoreMessage);
            restoreStatusText.text = restoreMessage;
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
