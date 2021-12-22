using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Samples.Purchasing.GooglePlay.UpgradeDowngradeSubscription
{
    [RequireComponent(typeof(UserWarningGooglePlayStore))]
    public class UpgradeDowngradeSubscription : MonoBehaviour, IStoreListener
    {
        //Your products IDs. They should match the ids of your products in your store.
        public string normalSubscriptionId = "com.mycompany.mygame.my_normal_pass_subscription";
        public string vipSubscriptionId = "com.mycompany.mygame.my_vip_pass_subscription";

        //These proration modes are the ones recommended by the Google Play Store, but you may want to use different modes for your specific situation.
        //https://developer.android.com/google/play/billing/subscriptions#proration-recommendations
        public GooglePlayProrationMode upgradeSubscriptionProrationMode = GooglePlayProrationMode.ImmediateAndChargeProratedPrice;
        public GooglePlayProrationMode downgradeSubscriptionProrationMode = GooglePlayProrationMode.Deferred;

        public Text currentSubscriptionText;
        public Text deferredSubscriptionChangeText;

        SubscriptionGroup m_SubscriptionGroup;

        void Start()
        {
            InitializePurchasing();
            UpdateWarningText();
        }

        void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            //When a subscription change has the DEFERRED proration mode, the subscription is upgraded or downgraded only when the subscription renews.
            //This sets a listener for when the subscription change is requested but hasn't occured yet.
            //We notify the user that the subscription change will take effect at the next renewal cycle.
            builder.Configure<IGooglePlayConfiguration>()
                .SetDeferredProrationUpgradeDowngradeSubscriptionListener(NotifyUserOfOnDeferredSubscriptionChange);

            builder.AddProduct(normalSubscriptionId, ProductType.Subscription);
            builder.AddProduct(vipSubscriptionId, ProductType.Subscription);

            UnityPurchasing.Initialize(this, builder);
        }

        void NotifyUserOfOnDeferredSubscriptionChange(Product product)
        {
            var msg = $"Subscription change to {product.definition.id} is deferred until the next renewal cycle.";
            deferredSubscriptionChangeText.text = msg;
            Debug.Log(msg);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("In-App Purchasing successfully initialized");

            m_SubscriptionGroup = new SubscriptionGroup(controller, extensions, upgradeSubscriptionProrationMode, downgradeSubscriptionProrationMode,
                normalSubscriptionId, vipSubscriptionId);
            UpdateUI(m_SubscriptionGroup.CurrentSubscriptionId());
        }

        public void BuyNormalSubscription()
        {
            m_SubscriptionGroup.BuySubscription(normalSubscriptionId);
        }

        public void BuyVipSubscription()
        {
            m_SubscriptionGroup.BuySubscription(vipSubscriptionId);
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
            //Unlock content here
            UpdateUI(product.definition.id);
        }

        void UpdateUI(string subscriptionId)
        {
            currentSubscriptionText.text = $"Current Subscription: {subscriptionId ?? "None"}";
            deferredSubscriptionChangeText.text = "";
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log($"In-App Purchasing initialize failed: {error}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
        }

        void UpdateWarningText()
        {
            GetComponent<UserWarningGooglePlayStore>().UpdateWarningText();
        }
    }
}
