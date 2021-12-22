## README - In-App Purchasing Sample Scenes - App Store - Refreshing App Receipts

This sample showcases how to use the Apple App Store extensions to refresh app receipts. This allows developers to
manually check for new purchases without creating new transactions, in contrast to RestoreTransactions.

## Instructions to test this sample:

1. Have in-app purchasing correctly configured with
   the [Apple App Store](https://docs.unity3d.com/Packages/com.unity.purchasing@3.2/manual/UnityIAPAppleConfiguration.html).
2. Configure a non-consumable product.
3. Set your own product's id in the `InAppPurchasing game object > Refreshing App Receipt script > No Ads Product Id field`
   or change the `noAdsProductId` field in the `RefreshingAppReceipt.cs` script.
4. Build your project for `iOS`.
   1. If you are using a simulator with Xcode 12+, follow these [instructions](https://developer.apple.com/documentation/xcode/setting-up-storekit-testing-in-xcode)
   to set up StoreKit Testing.
      
## Refreshing App Receipts

Using `RefreshAppReceipt` will prompt the user to enter their Apple login password.

See the documentation for
[receipt validation](https://docs.unity3d.com/Packages/com.unity.purchasing@3.2/manual/UnityIAPValidatingReceipts.html)
and the [iOS & Mac App Stores](https://docs.unity3d.com/Packages/com.unity.purchasing@3.2/manual/UnityIAPiOSMAS.html)
for more additional information.
