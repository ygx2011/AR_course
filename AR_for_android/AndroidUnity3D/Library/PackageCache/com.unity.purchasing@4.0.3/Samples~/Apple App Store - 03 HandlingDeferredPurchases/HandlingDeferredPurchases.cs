using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Samples.Purchasing.AppleAppStore.HandlingDeferredPurchases
{
    [RequireComponent(typeof(UserWarningAppleAppStore))]
    public class HandlingDeferredPurchases : MonoBehaviour, IStoreListener
    {
        IStoreController m_StoreController;
        IAppleExtensions m_AppleExtensions;

        public string goldProductId = "com.mycompany.mygame.gold1";
        public ProductType goldType = ProductType.Consumable;

        public Text goldCountText;

        int m_GoldCount;

        void Start()
        {
            InitializePurchasing();
            UpdateWarningMessage();
        }

        void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(goldProductId, goldType);

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("In-App Purchasing successfully initialized");

            m_StoreController = controller;
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
            m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferredPurchase);

            SetupAskToBuy();

            UpdateUI();
        }

        void SetupAskToBuy()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                // Only applies to Sandbox testing
                m_AppleExtensions.simulateAskToBuy = true;
            }
        }

        public void BuyGold()
        {
            m_StoreController.InitiatePurchase(goldProductId);
        }

        void OnDeferredPurchase(Product product)
        {
            Debug.Log($"Purchase of {product.definition.id} is deferred");
            UpdateUI();
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            var product = args.purchasedProduct;

            Debug.Log($"Processing Purchase: {product.definition.id}");

            UnlockContent(product);

            return PurchaseProcessingResult.Complete;
        }

        void UnlockContent(Product product)
        {
            Debug.Log($"Unlock Content: {product.definition.id}");

            if (product.definition.id == goldProductId)
            {
                AddGold();
            }
            UpdateUI();
        }

        void AddGold()
        {
            m_GoldCount++;
        }

        void UpdateUI()
        {
            goldCountText.text = $"Your Gold: {m_GoldCount}";
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log($"In-App Purchasing initialize failed: {error}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // A declined Ask to Buy transaction will not result in a OnPurchaseFailed call.
            Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
        }

        void UpdateWarningMessage()
        {
            GetComponent<UserWarningAppleAppStore>().UpdateWarningText();
        }
    }
}
