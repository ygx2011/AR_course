## README - In-App Purchasing Sample Scenes - Google Play Store - Restoring Transactions

This sample showcases how to use the Google Play Store extensions to restore transactions. This allows users to be
granted Non-Consumable and Subscription products they already own after reinstalling the application.

## Instructions to test this sample:

1. Have in-app purchasing correctly configured with
   the [Google Play Store](https://docs.unity3d.com/Packages/com.unity.purchasing@3.1/manual/UnityIAPGoogleConfiguration.html).
2. Configure a non-consumable product.
3. Set your own product's id in the `InAppPurchasing game object > Restoring Transaction script > No Ads Product Id field`
   or change the `noAdsProductId` field in the `RestoringTransaction.cs` script.
4. Build your project for `Android` and make sure the `GooglePlayStore` is selected*.

###### *You can change the currently selected store under `Services > In-App Purchasing > Configure` and changing the `Current Targeted Store` field.

## Restoring Transactions

When a user reinstalls your application they should be granted any Non-Consumable or renewable Subscription products
they already own. App stores maintain a permanent record of each user's Non-Consumable and renewable Subscription
products which Unity IAP can retrieve.

On the Google Play Store, Unity IAP automatically restores any products the user owns during the first initialization
following reinstallation; the ``ProcessPurchase`` method of your ``IStoreListener`` will be called for each owned item.

See the
[documentation](https://docs.unity3d.com/Manual/UnityIAPRestoringTransactions.html)
on the topic for more information.
