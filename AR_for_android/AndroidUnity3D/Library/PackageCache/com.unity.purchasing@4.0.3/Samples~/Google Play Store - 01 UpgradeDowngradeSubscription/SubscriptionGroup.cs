using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Samples.Purchasing.GooglePlay.UpgradeDowngradeSubscription
{
    /// <summary>
    /// A subscription group is a list of subscriptions where a user can only be subscribed to a single subscription in the group at one time.
    /// The subscriptions in the group are ordered by their tier, meaning a user can upgrade or downgrade from one subscription to another in the group.
    /// </summary>
    public class SubscriptionGroup
    {
        IStoreController m_StoreController;
        IExtensionProvider m_ExtensionsProvider;

        string[] m_SubscriptionIds;

        GooglePlayProrationMode m_UpgradeSubscriptionProrationMode;
        GooglePlayProrationMode m_DowngradeSubscriptionProrationMode;

        public SubscriptionGroup(IStoreController storeController, IExtensionProvider extensionsProvider, GooglePlayProrationMode upgradeSubscriptionProrationMode,
            GooglePlayProrationMode downgradeSubscriptionProrationMode, params string[] subscriptionIds)
        {
            m_StoreController = storeController;
            m_UpgradeSubscriptionProrationMode = upgradeSubscriptionProrationMode;
            m_DowngradeSubscriptionProrationMode = downgradeSubscriptionProrationMode;
            m_ExtensionsProvider = extensionsProvider;
            m_SubscriptionIds = subscriptionIds;
        }

        public void BuySubscription(string newSubscriptionId)
        {
            var currentSubscriptionId = CurrentSubscriptionId();
            if (IsASubscriptionChange(currentSubscriptionId, newSubscriptionId))
            {
                ChangeSubscriptionTier(currentSubscriptionId, newSubscriptionId);
            }
            else
            {
                m_StoreController.InitiatePurchase(newSubscriptionId);
            }
        }

        static bool IsASubscriptionChange(string previousSubscriptionId, string newSubscriptionId)
        {
            return previousSubscriptionId != null && previousSubscriptionId != newSubscriptionId;
        }

        void ChangeSubscriptionTier(string currentSubscriptionId, string newSubscriptionId)
        {
            Debug.Log($"Change Subscription from {currentSubscriptionId} to {newSubscriptionId}");
            var prorationMode = (int) DetermineProrationMode(currentSubscriptionId, newSubscriptionId);

            var googlePlayStoreExtension = m_ExtensionsProvider.GetExtension<IGooglePlayStoreExtensions>();
            googlePlayStoreExtension.UpgradeDowngradeSubscription(currentSubscriptionId, newSubscriptionId, prorationMode);
        }

        GooglePlayProrationMode DetermineProrationMode(string previousSubscriptionId, string newSubscriptionId)
        {
            //In this sample, we use different proration modes depending on if the new subscription is an upgrade compared to the current one.
            var isNewSubscriptionAnUpgrade = IsSubscriptionAnUpgrade(previousSubscriptionId, newSubscriptionId);
            return isNewSubscriptionAnUpgrade ? m_UpgradeSubscriptionProrationMode : m_DowngradeSubscriptionProrationMode;
        }

        bool IsSubscriptionAnUpgrade(string currentSubscription, string newSubscription)
        {
            return SubscriptionTier(newSubscription) > SubscriptionTier(currentSubscription);
        }

        int SubscriptionTier(string subscriptionId)
        {
            return Array.IndexOf(m_SubscriptionIds, subscriptionId);
        }

        public string CurrentSubscriptionId()
        {
            return m_SubscriptionIds.FirstOrDefault(IsSubscribedTo);
        }

        bool IsSubscribedTo(string subscriptionId)
        {
            var subscriptionProduct = m_StoreController.products.WithID(subscriptionId);
            return IsSubscribedTo(subscriptionProduct);
        }

        static bool IsSubscribedTo(Product subscription)
        {
            // If the product doesn't have a receipt, then it wasn't purchased and the user is therefore not subscribed.
            if (subscription?.receipt == null)
            {
                return false;
            }

            var subscriptionManager = new SubscriptionManager(subscription, null);
            var subscriptionInfo = subscriptionManager.getSubscriptionInfo();
            return subscriptionInfo.isSubscribed() == Result.True;
        }
    }
}
