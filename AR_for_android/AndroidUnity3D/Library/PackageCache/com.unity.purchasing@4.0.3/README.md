# About In App Purchasing

Unity In App Purchasing, setting up in-app purchases for your game across multiple app stores has never been easier.

This package provides:
- One common API to access all stores for free so you can fully understand and optimize your in-game economy
- Automatic coupling with Unity Analytics to enable monitoring and decision-making based on trends in your revenue and purchase data across multiple platforms
- Support for iOS, Mac, tvOS, Google Play, Windows, and Amazon app stores
- Support to work with the Unity Distribution Portal to synchronize catalogs and transactions with other app stores
- Client-side receipt validation for Apple App Store and Google Play



# IMPORTANT UPGRADE NOTES

If updating from Unity IAP (com.unity.purchasing + the Asset Store plugin) versions 2.x to version 3.x, complete the following actions in order to resolve compilation errors:
1. Move IAPProductCatalog.json and BillingMode.json
	FROM: Assets/Plugins/UnityPurchasing/Resources/
	TO: Assets/Resources/
2. Move AppleTangle.cs and GooglePlayTangle.cs
	FROM: Assets/Plugins/UnityPurchasing/generated
	TO: Assets/Scripts/UnityPurchasing/generated
3. Remove all remaining Asset Store plugin folders and files in Assets/Plugins/UnityPurchasing from your project.

# Installing In App Purchasing

To install this package, follow the instructions in the [Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html).

After installing this package, open the Services Window to enable In-App Purchasing to use these features.

# Using In App Purchasing

The In App Purchasing Manual can be found [here](https://docs.unity3d.com/2019.4/Documentation/Manual/UnityIAP.html)

# Technical details
## Requirements

This version of In App Purchasing is compatible with the following versions of the Unity Editor:

* 2019.4 and later (recommended)
