## README - In-App Purchasing Sample Scenes - Google Play Store - Upgrading and Downgrading Subscriptions

This sample showcases how to use the Google Play Store extensions to upgrade and downgrade subscriptions. This allows
players to change their subscription, and pay a different amount of money for a different level of service.

You can offer users different subscription tiers, such as a base tier and a premium tier or monthly and yearly
subscriptions. A user that is already subscribed should be able to pay a different amount of money to upgrade or
downgrade his subscription's tier.

## Instructions to test this sample:

1. Have in-app purchasing correctly configured with
   the [Google Play Store](https://docs.unity3d.com/Packages/com.unity.purchasing@3.1/manual/UnityIAPGoogleConfiguration.html)
   .
2. Configure two subscriptions of different tiers in the Google Play Console. This can be as simple as having a "normal"
   subscription and a "VIP" subscription.
3. Set your own product's id in
   the `InAppPurchasing game object > Upgrade Downgrade Subscription script > Normal Subscription Id field / Vip Subscription Id field`
   or change the `normalSubscriptionId` and `vipSubscriptionId` fields in the `UpgradeDowngradeSubscription.cs` script.
4. Build your project for `Android` and make sure the `GooglePlayStore` is selected*.

###### *You can change the currently selected store under `Services > In-App Purchasing > Configure` and changing the `Current Targeted Store` field.

## Proration Modes

When upgrading or downgrading a subscription, you can set the proration mode, or how the change affects your
subscribers.

The following table lists available proration modes:

* **IMMEDIATE_WITH_TIME_PRORATION (1):** The subscription is upgraded or downgraded immediately. Any time remaining is
  adjusted based on the price difference, and credited toward the new subscription by pushing forward the next billing
  date. This is the default behavior.
* **IMMEDIATE_AND_CHARGE_PRORATED_PRICE (2):** The subscription is upgraded immediately, and the billing cycle remains
  the same. The price difference for the remaining period is then charged to the user.
* **IMMEDIATE_WITHOUT_PRORATION (3):** The subscription is upgraded or downgraded immediately, and the new price is
  charged when the subscription renews. The billing cycle remains the same.
* **DEFERRED (4):** The subscription is upgraded or downgraded only when the subscription renews.
* **IMMEDIATE_AND_CHARGE_FULL_PRICE (5):** The subscription is upgraded or downgraded and the user is charged full price
  for the new entitlement immediately. The remaining value from the previous subscription is either carried over for the
  same entitlement, or prorated for time when switching to a different entitlement.

See
[Google's documentation](https://developer.android.com/google/play/billing/subscriptions#change)
on the topic for more information.

Certain proration modes are recommended in certain scenarios. See
[Google's recommended proration modes](https://developer.android.com/google/play/billing/subscriptions#proration-recommendations)
for more information.

