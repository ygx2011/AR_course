## README - In-App Purchasing Sample Scenes - Google Play Store - Fraud Detection

This sample showcases how to provide to the Google Play Store your user's identifiers to help prevent fraud using the
Google Play Store extension.

## Instructions to test this sample:

1. Have in-app purchasing correctly configured with
   the [Google Play Store](https://docs.unity3d.com/Packages/com.unity.purchasing@3.1/manual/UnityIAPGoogleConfiguration.html).
2. Configure a product.
3. Set your own product's id in the `InAppPurchasing game object > Fraud Detection script > Gold Product Id field`
   or change the `goldProductId` field in the `FraudDetection.cs` script.
4. Build your project for `Android` and make sure the `GooglePlayStore` is selected*.

###### *You can change the currently selected store under `Services > In-App Purchasing > Configure` and changing the `Current Targeted Store` field.

## Google Fraud Detection

To help prevent fraud, it is useful to provide to Google the in-app account and/or profile identifiers of your user. This helps Google
map the user's Google Play account to their in-app account.

The account and profile ids must not contain personally identifiable information such as emails in cleartext. To prevent
this, Google Play recommends that you use either encryption or a one-way hash to generate an obfuscated identifier to
send to Google Play.

For more information see [Google's documentation](https://developer.android.com/google/play/billing/security#fraud) on
the subject.
