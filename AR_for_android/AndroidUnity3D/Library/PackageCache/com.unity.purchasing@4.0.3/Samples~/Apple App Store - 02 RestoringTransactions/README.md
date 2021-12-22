## README - In-App Purchasing Sample Scenes - Apple App Store - Restoring Transactions

This sample showcases how to use Apple extensions to restore transactions. This allows users to be
granted Non-Consumable and Subscription products they already own after reinstalling the application.

## Instructions to test this sample:

1. Have in-app purchasing correctly configured with
   the [Apple App Store](https://docs.unity3d.com/Packages/com.unity.purchasing@3.2/manual/UnityIAPAppleConfiguration.html).
2. Configure a non-consumable product.
3. Set your own product's id in the `InAppPurchasing game object > Refreshing App Receipt script > No Ads Product Id field`
   or change the `noAdsProductId` field in the `RestoringTransactions.cs` script.
4. Build your project for `iOS`.
   1. If you are using a simulator with Xcode 12+, follow these [instructions](https://developer.apple.com/documentation/xcode/setting-up-storekit-testing-in-xcode)
      to set up StoreKit Testing.

## Restoring Transactions

When a user reinstalls your application they should be granted any Non-Consumable or renewable Subscription products
they already own. App stores maintain a permanent record of each user's Non-Consumable and renewable Subscription
products which Unity IAP can retrieve.

Users should be given the option to restore their transactions via a button, as RestoreTransactions may prompt
the user to enter their password and interrupt app flow.

See the
[Unity documentation](https://docs.unity3d.com/Packages/com.unity.purchasing@3.2/manual/UnityIAPRestoringTransactions.html)
as well as the [Apple documentation](https://developer.apple.com/documentation/storekit/original_api_for_in-app_purchase/restoring_purchased_products)
on the topic for more information.
