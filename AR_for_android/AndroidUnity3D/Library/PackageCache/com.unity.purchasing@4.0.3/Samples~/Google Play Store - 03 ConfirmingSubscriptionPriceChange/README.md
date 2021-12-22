## README - In-App Purchasing Sample Scenes - Google Play Store - Confirming Subscription Price Change

This sample showcases how to use the Google Play Store extensions to confirm subscription price changes.

## Instructions to test this sample:

1. Have in-app purchasing correctly configured with
   the [Google Play Store](https://docs.unity3d.com/Packages/com.unity.purchasing@3.1/manual/UnityIAPGoogleConfiguration.html)
   .
2. Configure a subscription product.
3. Set your own subscription product's id in
   the `InAppPurchasing game object > Confirming Subscription Price Change script > Subscription Product Id field`
   or change the `subscriptionProductId` field in the `ConfirmingSubscriptionPriceChange.cs` script.
4. Build your project for `Android` and make sure the `GooglePlayStore` is selected*.

###### *You can change the currently selected store under `Services > In-App Purchasing > Configure` and changing the `Current Targeted Store` field.

## Confirming Subscription Price Change

From [Google's documentation](https://developer.android.com/google/play/billing/subscriptions#price-change):

> Sometimes, due to regional costs or currency fluctuations, you may decide you need to change the price of your
> subscription. If you are willing to keep existing subscribers on the existing price, you can create a new SKU with the
> updated price and offer that to new subscribers.
>
> If you are not able to continue supporting subscribers with the price that was offered when they signed up for your
> subscription and are willing to cancel all subscriptions for users that decided not to accept the new price, you can
> enforce a mandatory price change. If the user does not agree to the new price, their subscription is cancelled. To
> maximize opt-in rates and encourage your users to take action, your app should display messaging to your users about the
> upcoming price change.
>
> When you increase the price of a subscription, you have at least seven days to notify your existing subscribers about
> the price change before Google Play can start notifying them.


