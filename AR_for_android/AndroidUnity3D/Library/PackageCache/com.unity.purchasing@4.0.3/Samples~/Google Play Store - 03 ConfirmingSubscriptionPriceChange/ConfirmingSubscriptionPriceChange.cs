using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Samples.Purchasing.GooglePlay.ConfirmingSubscriptionPriceChange
{
    [RequireComponent(typeof(UserWarningGooglePlayStore))]
    public class ConfirmingSubscriptionPriceChange : MonoBehaviour, IStoreListener
    {
        IStoreController m_StoreController;
        IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;

        public string subscriptionProductId = "com.mycompany.mygame.my_vip_pass_subscription";

        public Text isSubscribedText;

        void Start()
        {
            InitializePurchasing();
            UpdateWarningMessage();
        }

        void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(subscriptionProductId, ProductType.Subscription);

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("In-App Purchasing successfully initialized");

            m_StoreController = controller;
            m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

            UpdateUI();
        }

        public void ConfirmPriceChange()
        {
            m_GooglePlayStoreExtensions.ConfirmSubscriptionPriceChange(subscriptionProductId, OnConfirmedPriceChange);
        }

        static void OnConfirmedPriceChange(bool result)
        {
            if (result)
            {
                //The subscription price change was confirmed.
                //The subscription will therefore be renewed.
                Debug.Log("Confirm Price Change Successful");
            }
            else
            {
                Debug.Log("Confirm Price Change Failed");
            }
        }

        public void BuySubscription()
        {
            m_StoreController.InitiatePurchase(subscriptionProductId);
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
            isSubscribedText.text = IsSubscribedTo(subscriptionProductId) ? "You are subscribed" : "You are not subscribed";
        }

        bool IsSubscribedTo(string subscriptionId)
        {
            var subscription = m_StoreController.products.WithStoreSpecificID(subscriptionId);
            if (subscription.receipt == null)
            {
                return false;
            }

            var subscriptionManager = new SubscriptionManager(subscription, null);
            var info = subscriptionManager.getSubscriptionInfo();
            return info.isSubscribed() == Result.True;
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
            GetComponent<UserWarningGooglePlayStore>().UpdateWarningText();
        }
    }
}
