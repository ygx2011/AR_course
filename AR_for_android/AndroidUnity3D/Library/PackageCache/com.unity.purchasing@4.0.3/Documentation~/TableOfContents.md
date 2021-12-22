* Introduction to Unity IAP
    * [About Unity IAP](index.md)
    * [Stores supported by Unity IAP](StoresSupported.md)
* Set up and integrating Unity IAP
    * [Overview](SetupIAP/Overview.md)
    * [Getting Started](SetupIAP/GettingStarted.md)
    * Defining products
        * [Overview](SetupIAP/DefiningProduct/DefiningProductsOverview.md)
        * [Coded](SetupIAP/DefiningProduct/DefiningProductsCoded.md)
        * [IAP Catalog](SetupIAP/DefiningProduct/UnityIAPDefiningProducts.md)
    * [Initialize IAP](SetupIAP/UnityIAPInitialization.md)
    * [Fetching Additional Products](SetupIAP/UnityIAPFetchingProductsIncrementally.md)
    * Creating a Purchasing Button
        * [Browsing Product Metadata](SetupIAP/PurchasingButton/UnityIAPBrowsingMetadata.md)
        * [IAP Button](SetupIAP/PurchasingButton/IAPButton.md)
        * [Coded](SetupIAP/PurchasingButton/UnityIAPInitiatingPurchases.md)
    * The Purchasing Flow
        * Processing Purchases
            * [Coded](SetupIAP/PurchasingFlow/ProcessingPurchase/UnityIAPProcessingPurchases.md)
            * [IAP Listener](SetupIAP/PurchasingFlow/ProcessingPurchase/IAPListener.md)
            * [Handling Purchase Failures](SetupIAP/PurchasingFlow/ProcessingPurchase/UnityIAPHandlingPurchaseFailures.md)
        * [Purchase Receipts](SetupIAP/PurchasingFlow/UnityIAPPurchaseReceipts.md)
        * [Restoring Transactions](SetupIAP/PurchasingFlow/UnityIAPRestoringTransactions.md)
    * Securing Transactions
        * [Backend Receipt Validation](SetupIAP/SecuringTransactions/BackendReceiptValidation.md)
        * [Receipt Obfuscation](SetupIAP/SecuringTransactions/UnityIAPValidatingReceipts.md)
    * [Store Selector](SetupIAP/StoreSelector.md)
    * Testing the Integration
        * [What is Fake Store?](SetupIAP/TestingIntegration/WhatIsFakeStore.md)
        * [How to Test](SetupIAP/TestingIntegration/HowToTest.md)
* Stores
    * Google
        * [How to Set Up](Stores/Google/UnityIAPGoogleConfiguration.md)
        * [Setting Google Public Key](Stores/Google/GooglePublicKey.md)
        * [Extensions and Configuration](Stores/Google/UnityIAPGooglePlay.md)
        * [Purchase Receipt](Stores/Google/GoogleReceipt.md)
        * [Testing Integration](Stores/Google/Testing.md)
    * Amazon
        * [How to Set Up](Stores/Amazon/UnityIAPAmazonConfiguration.md)
        * [Extensions](Stores/Amazon/UnityIAPAmazonExtendedFunctionality.md)
        * [Testing Integration](Stores/Amazon/AmazonTesting.md)
    * Apple Store iOS, MacOS & tvOS
        * [How to Set Up](Stores/Apple/UnityIAPAppleConfiguration.md)
        * [Extensions and Configuration](Stores/Apple/UnityIAPiOSMAS.md)
        * [Purchase Receipt](Stores/Apple/AppleReceipt.md)
        * [Testing](Stores/Apple/AppleTesting.md)
    * Microsoft Store (UWP)
        * [How to Set Up](Stores/Microsoft/UnityIAPWindowsConfiguration.md)
        * [Purchase Receipt](Stores/Microsoft/MicrosoftReceipt.md)
        * [Testing](Stores/Microsoft/UnityIAPUniversalWindows.md)
    * Implement Custom Store
        * [What is a Custom Store](Stores/CustomStore/WhatCustomStore.md)
        * [Implementing](Stores/CustomStore/UnityIAPImplementingAStore.md)
        * [Initialization](Stores/CustomStore/UnityIAPIStoreInitialization.md)
        * [Retrieving products](Stores/CustomStore/UnityIAPIStoreRetrievingProducts.md)
        * [Handling purchases](Stores/CustomStore/UnityIAPIStoreHandlingPurchases.md)
        * [Store Modules](Stores/CustomStore/UnityIAPModules.md)
            * [Registering your store](Stores/CustomStore/UnityIAPModuleRegistration.md)
            * [Store Configuration](Stores/CustomStore/UnityIAPModuleConfiguration.md)
            * [Store Extensions](Stores/CustomStore/UnityIAPModuleExtension.md)
