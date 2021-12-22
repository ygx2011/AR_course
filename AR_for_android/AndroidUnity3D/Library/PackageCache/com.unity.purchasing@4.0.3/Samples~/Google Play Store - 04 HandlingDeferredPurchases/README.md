## README - In-App Purchasing Sample Scenes - Google Play Store - Handling Deferred Purchases

This sample showcases how to handle deferred purchases using the `IGooglePlayConfiguration.SetDeferredPurchaseListener`
and the `GooglePlayStoreExtensions.IsPurchasedProductDeferred` APIs.

## Instructions to test this sample:

1. Have in-app purchasing correctly configured with
   the [Google Play Store](https://docs.unity3d.com/Packages/com.unity.purchasing@3.1/manual/UnityIAPGoogleConfiguration.html).
2. Configure a product.
3. Set your own product's id in
   the `InAppPurchasing game object > CHandling Deferred Purchases script > Gold Product Id field`
   or change the `goldProductId` field in the `HandlingDeferredPurchases.cs` script.
4. Build your project for `Android` and make sure the `GooglePlayStore` is selected*.

###### *You can change the currently selected store under `Services > In-App Purchasing > Configure` and changing the `Current Targeted Store` field.

## Google Deferred Purchases

From [Google's documentation](https://developer.android.com/google/play/billing/integrate#pending):

> Google Play supports deferred transactions, or transactions that require one or more additional steps between when a
> user initiates a purchase and when the payment method for the purchase is processed. Your app should not grant
> entitlement to these types of purchases until Google notifies you that the user's payment method was successfully
> charged.
>
> For example, a user can create a deferred purchase of an in-app item by choosing cash as their form of payment. The user
> can then choose a physical store where they will complete the transaction and receive a code through both notification
> and email. When the user arrives at the physical store, they can redeem the code with the cashier and pay with cash.
> Google then notifies both you and the user that cash has been received. Your app can then grant entitlement to the user.



