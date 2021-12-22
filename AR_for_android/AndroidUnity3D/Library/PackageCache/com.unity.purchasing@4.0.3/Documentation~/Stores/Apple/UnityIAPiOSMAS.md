Extensions and Configuration
====================

Extended functionality
----------------------

### Reading the App Receipt

An [App Receipt](https://developer.apple.com/library/ios/releasenotes/General/ValidateAppStoreReceipt/Chapters/ValidateRemotely.html) is stored on the device's local storage and can be read as follows:

````
var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
string receipt = builder.Configure<IAppleConfiguration>().appReceipt;
````

### Checking for payment restrictions

In App Purchases may be restricted in a device's settings, which can be checked for as follows:

````
var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
bool canMakePayments = builder.Configure<IAppleConfiguration>().canMakePayments;
````

### Restoring transactions

On Apple platforms users must enter their password to retrieve previous transactions so your application must provide users with a button letting them do so. During this process the `ProcessPurchase` method of your `IStoreListener` will be invoked for any items the user already owns.

````
/// <summary>
/// Your IStoreListener implementation of OnInitialized.
/// </summary>
public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
{
	extensions.GetExtension<IAppleExtensions> ().RestoreTransactions (result => {
		if (result) {
			// This does not mean anything was restored,
			// merely that the restoration process succeeded.
		} else {
			// Restoration failed.
		}
	});
}
````

### Refreshing the App Receipt

Apple provides a mechanism to fetch a new App Receipt from their servers, typically used when no receipt is currently cached in local storage; [SKReceiptRefreshRequest](https://developer.apple.com/library/ios/documentation/StoreKit/Reference/SKReceiptRefreshRequest_ClassRef/index.html#/apple_ref/occ/cl/SKReceiptRefreshRequest).

Note that this will prompt the user for their password.

Unity IAP makes this method available as follows:

````
/// <summary>
/// Your IStoreListener implementation of OnInitialized.
/// </summary>
public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
{
	extensions.GetExtension<IAppleExtensions> ().RefreshAppReceipt (receipt => {
		// This handler is invoked if the request is successful.
		// Receipt will be the latest app receipt.
		Console.WriteLine(receipt);
	},
	() => {
		// This handler will be invoked if the request fails,
		// such as if the network is unavailable or the user
		// enters the wrong password.
	});
}
````

### Ask to Buy

iOS 8 introduced a new parental control feature called [Ask to Buy](https://developer.apple.com/library/ios/technotes/tn2259/_index.html#/apple_ref/doc/uid/DTS40009578-CH1-UPDATE_YOUR_APP_FOR_ASK_TO_BUY). 

Ask to Buy purchases defer for parent approval. When this occurs, Unity IAP sends your app a notification as follows:

````
/// This is called when Unity IAP has finished initialising.
public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
{
    extensions.GetExtension<IAppleExtensions>().RegisterPurchaseDeferredListener(product => {
        Console.WriteLine(product.definition.id);
    });
}
````

#### Enable "Ask-to-Buy" simulation in the Sandbox App Store

The sample class below demonstrates how to access `IAppleExtensions` in order to enable Ask-to-Buy simulation in the Sandbox App Store:

```
using UnityEngine;
using UnityEngine.Purchasing;

public class AppleSimulateAskToBuy : MonoBehaviour {
	public void SetSimulateAskToBuy(bool shouldSimulateAskToBuy) {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			IAppleExtensions extensions = IAPButton.IAPButtonStoreManager.Instance.ExtensionProvider.GetExtension<IAppleExtensions>();
			extensions.simulateAskToBuy = shouldSimulateAskToBuy;
		}
	}
}
```

When the purchase is approved or rejected, your store's normal `ProcessPurchase` or `OnPurchaseFailed` listener methods are invoked.

### Transaction Receipts
Sometimes consumable Ask to Buy purchases don't show up in the App Receipt, in which case you cannot validate them using that receipt. However, iOS provides a Transaction Receipt that contains all purchases, including Ask to Buy. Access the most recent Transaction Receipt string for a given `Product` using `IAppleExtensions`. 

**Note**: Transaction Receipts are not available for Mac builds. Requesting a Transaction Receipt on a Mac build results in an empty string.

```
#if UNITY_PURCHASING

using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class AskToBuy : MonoBehaviour, IStoreListener
{
    // Unity IAP objects
    private IStoreController m_Controller;
    private IAppleExtensions m_AppleExtensions;

    public AskToBuy ()
    {
        var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());
        builder.AddProduct ("100_gold_coins", ProductType.Consumable, new IDs {
            { "100_gold_coins_google", GooglePlay.Name },
            { "100_gold_coins_mac", MacAppStore.Name }
        });

        UnityPurchasing.Initialize (this, builder);
    }

    /// <summary>
    /// This will be called when Unity IAP has finished initialising.
    /// </summary>
    public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
    {
        m_Controller = controller;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions> ();

        // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
        // On non-Apple platforms this will have no effect; OnDeferred will never be called.
        m_AppleExtensions.RegisterPurchaseDeferredListener (OnDeferred);
    }

    /// <summary>
    /// This will be called when a purchase completes.
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.tvOS) {
            string transactionReceipt = m_AppleExtensions.GetTransactionReceiptForProduct (e.purchasedProduct);
            Console.WriteLine (transactionReceipt);
            // Send transaction receipt to server for validation
        }    
        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed (InitializationFailureReason error)
    {
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed (Product i, PurchaseFailureReason p)
    {
    }

    /// <summary>
    /// iOS Specific.
    /// This is called as part of Apple's 'Ask to buy' functionality,
    /// when a purchase is requested by a minor and referred to a parent
    /// for approval.
    ///
    /// When the purchase is approved or rejected, the normal purchase events
    /// will fire.
    /// </summary>
    /// <param name="item">Item.</param>
    private void OnDeferred (Product item)
    {
        Debug.Log ("Purchase deferred: " + item.definition.id);
    }
}

#endif // UNITY_PURCHASING
```

Unlike App Receipts, you cannot validate Transaction Receipts locally. Instead, you must send the receipt string to a remote server for validation. If you already use a remote server to validate App Receipts, send Transaction Receipts to the same Apple endpoint, to receive a JSON response.

Example JSON response:

```
{
    "receipt": {
        "original_purchase_date_pst": "2017-11-15 15:25:20 America/Los_Angeles",
        "purchase_date_ms": "1510788320209",
        "unique_identifier": "0ea7808637555b2c633eb07aa1cb0894c821a6f9",
        "original_transaction_id": "1000000352597239",
        "bvrs": "0",
        "transaction_id": "1000000352597239",
        "quantity": "1",
        "unique_vendor_identifier": "01B57C2E-9E91-42FF-9B0D-4983175D6694",
        "item_id": "1141751870",
        "original_purchase_date": "2017-11-15 23:25:20 Etc/GMT",
        "product_id": "100.gold.coins",
        "purchase_date": "2017-11-15 23:25:20 Etc/GMT",
        "is_trial_period": "false",
        "purchase_date_pst": "2017-11-15 15:25:20 America/Los_Angeles",
        "bid": "com.unity3d.unityiap.demo",
        "original_purchase_date_ms": "1510788320209"
    },
    "status": 0
}
```

## Intercepting Apple promotional purchases
Apple allows you to promote [in-game purchases](https://developer.apple.com/app-store/promoting-in-app-purchases/#:~:text=inside%20your%20app.-,Overview,approved%20and%20ready%20to%20promote.&text=When%20a%20user%20doesn't,to%20download%20the%20app%20first.) through your app’s product page. Unlike conventional in-app purchases, Apple promotional purchases initiate directly from the App Store on iOS and tvOS. The App Store then launches your app to complete the transaction, or prompts the user to download the app if it isn’t installed. 

The `IAppleConfiguration` `SetApplePromotionalPurchaseInterceptor` callback method intercepts Apple promotional purchases. Use this callback to present parental gates, send analytics events, or perform other functions before sending the purchase to Apple. The callback uses the `Product` that the user attempted to purchase. You must call `IAppleExtensions.ContinuePromotionalPurchases()` to continue with the promotional purchase. This will initiate any queued-up payments.

If you do not set the callback, promotional purchases go through immediately and call `ProcessPurchase` with the result. 

**Note**: Calling these APIs on other platforms has no effect.

```
private IAppleExtensions m_AppleExtensions;

public void Awake() {
    var module = StandardPurchasingModule.Instance();
    var builder = ConfigurationBuilder.Instance(module);
        // On iOS and tvOS we can intercept promotional purchases that come directly from
        // the App Store.
        // On other platforms this will have no effect; OnPromotionalPurchase will never be
        // called.
    builder.Configure<IAppleConfiguration>().
     SetApplePromotionalPurchaseInterceptorCallback(OnPromotionalPurchase);
    Debug.Log("Setting Apple promotional purchase interceptor callback");
}

public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
    m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
    foreach (var item in controller.products.all) {
        if (item.availableToPurchase) {                
            // Set all these products to be visible in the user's App Store
            m_AppleExtensions.SetStorePromotionVisibility(item, AppleStorePromotionVisibility.Show);
        }
    }
}

private void OnPromotionalPurchase(Product item) {
    Debug.Log("Attempted promotional purchase: " + item.definition.id);
    // Promotional purchase has been detected. 
    // Handle this event by, e.g. presenting a parental gate.
    // Here, for demonstration purposes only, we will wait five seconds before continuing 
    // the purchase.
    StartCoroutine(ContinuePromotionalPurchases());
}

private IEnumerator ContinuePromotionalPurchases() {
    Debug.Log("Continuing promotional purchases in 5 seconds");
    yield return new WaitForSeconds(5);
    Debug.Log("Continuing promotional purchases now");
    m_AppleExtensions.ContinuePromotionalPurchases (); // iOS and tvOS only
}

```