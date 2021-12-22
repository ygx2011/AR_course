#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// You must obfuscate your secrets using Window > Unity IAP > Receipt Validation Obfuscator
// before receipt validation will compile in this sample.
//#define RECEIPT_VALIDATION
#endif

//#define DELAY_CONFIRMATION // Returns PurchaseProcessingResult.Pending from ProcessPurchase, then calls ConfirmPendingPurchase after a delay
//#define USE_PAYOUTS // Enables use of PayoutDefinitions to specify what the player should receive when a product is purchased
//#define INTERCEPT_PROMOTIONAL_PURCHASES // Enables intercepting promotional purchases that come directly from the Apple App Store
//#define SUBSCRIPTION_MANAGER //Enables subscription product manager for AppleStore and GooglePlay store
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

#if RECEIPT_VALIDATION
using UnityEngine.Purchasing.Security;
#endif

/// <summary>
/// An example of Unity IAP functionality.
/// To use with your account, configure the product ids (AddProduct).
/// </summary>
[AddComponentMenu("Unity IAP/Demo")]
public class IAPDemo : MonoBehaviour, IStoreListener
{
    // Unity IAP objects
    private IStoreController m_Controller;

    private IAppleExtensions m_AppleExtensions;
    private IMicrosoftExtensions m_MicrosoftExtensions;
    private ITransactionHistoryExtensions m_TransactionHistoryExtensions;
    private IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;

#pragma warning disable 0414
    private bool m_IsGooglePlayStoreSelected;
#pragma warning restore 0414

    private bool m_PurchaseInProgress;

    private Dictionary<string, IAPDemoProductUI> m_ProductUIs = new Dictionary<string, IAPDemoProductUI>();

    /// <summary>
    /// The interface for instantiating user interface for a <typeparamref name="Product"/>.
    /// Instantiates a IAPDemoProductUI.
    /// </summary>
    public GameObject productUITemplate;

    /// <summary>
    /// The size of the current Product UI content area.
    /// </summary>
    public RectTransform contentRect;

    /// <summary>
    /// The handle for the <typeparamref name="IAPButton"/> configured to restore IAP transactions.
    /// </summary>
    public Button restoreButton;

    /// <summary>
    /// The reference for the <typeparamref name="Text"/> to show the current IAP SDK version.
    /// </summary>
    public Text versionText;

#if RECEIPT_VALIDATION
    private CrossPlatformValidator validator;
#endif

    /// <summary>
    /// Purchasing initialized successfully.
    ///
    /// The <c>IStoreController</c> and <c>IExtensionProvider</c> are
    /// available for accessing purchasing functionality.
    /// </summary>
    /// <param name="controller"> The <c>IStoreController</c> created during initialization. </param>
    /// <param name="extensions"> The <c>IExtensionProvider</c> created during initialization. </param>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_Controller = controller;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        m_MicrosoftExtensions = extensions.GetExtension<IMicrosoftExtensions>();
        m_TransactionHistoryExtensions = extensions.GetExtension<ITransactionHistoryExtensions>();
        m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

        InitUI(controller.products.all);

        // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
        // On non-Apple platforms this will have no effect; OnDeferred will never be called.
        m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);

#if SUBSCRIPTION_MANAGER
        Dictionary<string, string> introductory_info_dict = m_AppleExtensions.GetIntroductoryPriceDictionary();
#endif
        // Sample code for expose product sku details for apple store
        //Dictionary<string, string> product_details = m_AppleExtensions.GetProductDetails();


        Debug.Log("Available items:");
        foreach (var item in controller.products.all)
        {
            if (item.availableToPurchase)
            {
                Debug.Log(string.Join(" - ",
                    new[]
                    {
                        item.metadata.localizedTitle,
                        item.metadata.localizedDescription,
                        item.metadata.isoCurrencyCode,
                        item.metadata.localizedPrice.ToString(),
                        item.metadata.localizedPriceString,
                        item.transactionID,
                        item.receipt
                    }));
#if INTERCEPT_PROMOTIONAL_PURCHASES
                // Set all these products to be visible in the user's App Store according to Apple's Promotional IAP feature
                // https://developer.apple.com/library/content/documentation/NetworkingInternet/Conceptual/StoreKitGuide/PromotingIn-AppPurchases/PromotingIn-AppPurchases.html
                m_AppleExtensions.SetStorePromotionVisibility(item, AppleStorePromotionVisibility.Show);
#endif

#if SUBSCRIPTION_MANAGER
                // this is the usage of SubscriptionManager class
                if (item.receipt != null) {
                    if (item.definition.type == ProductType.Subscription) {
                        if (checkIfProductIsAvailableForSubscriptionManager(item.receipt)) {
                            string intro_json = (introductory_info_dict == null || !introductory_info_dict.ContainsKey(item.definition.storeSpecificId)) ? null : introductory_info_dict[item.definition.storeSpecificId];
                            SubscriptionManager p = new SubscriptionManager(item, intro_json);
                            SubscriptionInfo info = p.getSubscriptionInfo();
                            Debug.Log("product id is: " + info.getProductId());
                            Debug.Log("purchase date is: " + info.getPurchaseDate());
                            Debug.Log("subscription next billing date is: " + info.getExpireDate());
                            Debug.Log("is subscribed? " + info.isSubscribed().ToString());
                            Debug.Log("is expired? " + info.isExpired().ToString());
                            Debug.Log("is cancelled? " + info.isCancelled());
                            Debug.Log("product is in free trial peroid? " + info.isFreeTrial());
                            Debug.Log("product is auto renewing? " + info.isAutoRenewing());
                            Debug.Log("subscription remaining valid time until next billing date is: " + info.getRemainingTime());
                            Debug.Log("is this product in introductory price period? " + info.isIntroductoryPricePeriod());
                            Debug.Log("the product introductory localized price is: " + info.getIntroductoryPrice());
                            Debug.Log("the product introductory price period is: " + info.getIntroductoryPricePeriod());
                            Debug.Log("the number of product introductory price period cycles is: " + info.getIntroductoryPricePeriodCycles());
                        } else {
                            Debug.Log("This product is not available for SubscriptionManager class, only products that are purchase by 1.19+ SDK can use this class.");
                        }
                    } else {
                        Debug.Log("the product is not a subscription product");
                    }
                } else {
                    Debug.Log("the product should have a valid receipt");
                }
#endif
            }
        }

        // Populate the product menu now that we have Products
        AddProductUIs(m_Controller.products.all);

        LogProductDefinitions();
    }

#if SUBSCRIPTION_MANAGER
    private bool checkIfProductIsAvailableForSubscriptionManager(string receipt) {
        var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
        if (!receipt_wrapper.ContainsKey("Store") || !receipt_wrapper.ContainsKey("Payload")) {
            Debug.Log("The product receipt does not contain enough information");
            return false;
        }
        var store = (string)receipt_wrapper ["Store"];
        var payload = (string)receipt_wrapper ["Payload"];

        if (payload != null ) {
            switch (store) {
            case GooglePlay.Name:
                {
                    var payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
                    if (!payload_wrapper.ContainsKey("json")) {
                        Debug.Log("The product receipt does not contain enough information, the 'json' field is missing");
                        return false;
                    }
                    var original_json_payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode((string)payload_wrapper["json"]);
                    if (original_json_payload_wrapper == null || !original_json_payload_wrapper.ContainsKey("developerPayload")) {
                        Debug.Log("The product receipt does not contain enough information, the 'developerPayload' field is missing");
                        return false;
                    }
                    var developerPayloadJSON = (string)original_json_payload_wrapper["developerPayload"];
                    var developerPayload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(developerPayloadJSON);
                    if (developerPayload_wrapper == null || !developerPayload_wrapper.ContainsKey("is_free_trial") || !developerPayload_wrapper.ContainsKey("has_introductory_price_trial")) {
                        Debug.Log("The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
                        return false;
                    }
                    return true;
                }
            case AppleAppStore.Name:
            case AmazonApps.Name:
            case MacAppStore.Name:
                {
                    return true;
                }
            default:
                {
                    return false;
                }
            }
        }
        return false;
    }
#endif


    /// <summary>
    /// A purchase succeeded.
    /// </summary>
    /// <param name="e"> The <c>PurchaseEventArgs</c> for the purchase event. </param>
    /// <returns> The result of the successful purchase </returns>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
        Debug.Log("Receipt: " + e.purchasedProduct.receipt);

        m_PurchaseInProgress = false;

#if RECEIPT_VALIDATION // Local validation is available for GooglePlay, and Apple stores
        if (m_IsGooglePlayStoreSelected ||
            Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.tvOS) {
            try {
                var result = validator.Validate(e.purchasedProduct.receipt);
                Debug.Log("Receipt is valid. Contents:");
                foreach (IPurchaseReceipt productReceipt in result) {
                    Debug.Log(productReceipt.productID);
                    Debug.Log(productReceipt.purchaseDate);
                    Debug.Log(productReceipt.transactionID);

                    GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
                    if (null != google) {
                        Debug.Log(google.purchaseState);
                        Debug.Log(google.purchaseToken);
                    }

                    AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
                    if (null != apple) {
                        Debug.Log(apple.originalTransactionIdentifier);
                        Debug.Log(apple.subscriptionExpirationDate);
                        Debug.Log(apple.cancellationDate);
                        Debug.Log(apple.quantity);
                    }

                    // For improved security, consider comparing the signed
                    // IPurchaseReceipt.productId, IPurchaseReceipt.transactionID, and other data
                    // embedded in the signed receipt objects to the data which the game is using
                    // to make this purchase.
                }
            }
            catch (IAPSecurityException ex)
            {
                Debug.Log("Invalid receipt, not unlocking content. " + ex);
                return PurchaseProcessingResult.Complete;
            }
            catch (NotImplementedException exception)
            {
                Debug.Log("Cross Platform Validator Not Implemented: " + exception);
            }
        }
        #endif

        // Unlock content from purchases here.
#if USE_PAYOUTS
        if (e.purchasedProduct.definition.payouts != null) {
            Debug.Log("Purchase complete, paying out based on defined payouts");
            foreach (var payout in e.purchasedProduct.definition.payouts) {
                Debug.Log(string.Format("Granting {0} {1} {2} {3}", payout.quantity, payout.typeString, payout.subtype, payout.data));
            }
        }
#endif
        // Indicate if we have handled this purchase.
        //   PurchaseProcessingResult.Complete: ProcessPurchase will not be called
        //     with this product again, until next purchase.
        //   PurchaseProcessingResult.Pending: ProcessPurchase will be called
        //     again with this product at next app launch. Later, call
        //     m_Controller.ConfirmPendingPurchase(Product) to complete handling
        //     this purchase. Use to transactionally save purchases to a cloud
        //     game service.
#if DELAY_CONFIRMATION
        StartCoroutine(ConfirmPendingPurchaseAfterDelay(e.purchasedProduct));
        return PurchaseProcessingResult.Pending;
#else
        UpdateProductUI(e.purchasedProduct);
        return PurchaseProcessingResult.Complete;
#endif
    }

#if DELAY_CONFIRMATION
    private HashSet<string> m_PendingProducts = new HashSet<string>();

    private IEnumerator ConfirmPendingPurchaseAfterDelay(Product p)
    {
        m_PendingProducts.Add(p.definition.id);
        Debug.Log("Delaying confirmation of " + p.definition.id + " for 5 seconds.");

		var end = Time.time + 5f;

		while (Time.time < end) {
			yield return null;
			var remaining = Mathf.CeilToInt (end - Time.time);
			UpdateProductPendingUI (p, remaining);
		}

        Debug.Log("Confirming purchase of " + p.definition.id);
        m_Controller.ConfirmPendingPurchase(p);
        m_PendingProducts.Remove(p.definition.id);
		UpdateProductUI (p);
    }
#endif



    /// <summary>
    /// A purchase failed with specified reason.
    /// </summary>
    /// <param name="item">The product that was attempted to be purchased. </param>
    /// <param name="r">The failure reason.</param>
    public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    {
        Debug.Log("Purchase failed: " + item.definition.id);
        Debug.Log(r);

        // Detailed debugging information
        Debug.Log("Store specific error code: " + m_TransactionHistoryExtensions.GetLastStoreSpecificPurchaseErrorCode());
        if (m_TransactionHistoryExtensions.GetLastPurchaseFailureDescription() != null)
        {
            Debug.Log("Purchase failure description message: " +
                      m_TransactionHistoryExtensions.GetLastPurchaseFailureDescription().message);
        }

        m_PurchaseInProgress = false;
    }

    /// <summary>
    /// Purchasing failed to initialise for a non recoverable reason.
    /// </summary>
    /// <param name="error"> The failure reason. </param>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Billing failed to initialize!");
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                // Ask the user if billing is disabled in device settings.
                Debug.Log("Billing disabled!");
                break;
            case InitializationFailureReason.NoProductsAvailable:
                // Developer configuration error; check product metadata.
                Debug.Log("No products available for purchase!");
                break;
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        var module = StandardPurchasingModule.Instance();

        // The FakeStore supports: no-ui (always succeeding), basic ui (purchase pass/fail), and
        // developer ui (initialization, purchase, failure code setting). These correspond to
        // the FakeStoreUIMode Enum values passed into StandardPurchasingModule.useFakeStoreUIMode.
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

        var builder = ConfigurationBuilder.Instance(module);

        // Set this to true to enable the Microsoft IAP simulator for local testing.
        builder.Configure<IMicrosoftConfiguration>().useMockBillingSystem = false;

        m_IsGooglePlayStoreSelected =
            Application.platform == RuntimePlatform.Android && module.appStore == AppStore.GooglePlay;

        // Define our products.
        // Either use the Unity IAP Catalog, or manually use the ConfigurationBuilder.AddProduct API.
        // Use IDs from both the Unity IAP Catalog and hardcoded IDs via the ConfigurationBuilder.AddProduct API.

        // Use the products defined in the IAP Catalog GUI.
        // E.g. Menu: "Window" > "Unity IAP" > "IAP Catalog", then add products, then click "App Store Export".
        var catalog = ProductCatalog.LoadDefaultCatalog();

        foreach (var product in catalog.allValidProducts)
        {
            if (product.allStoreIDs.Count > 0)
            {
                var ids = new IDs();
                foreach (var storeID in product.allStoreIDs)
                {
                    ids.Add(storeID.id, storeID.store);
                }
                builder.AddProduct(product.id, product.type, ids);
            }
            else
            {
                builder.AddProduct(product.id, product.type);
            }
        }

        // In this case our products have the same identifier across all the App stores,
        // except on the Mac App store where product IDs cannot be reused across both Mac and
        // iOS stores.
        // So on the Mac App store our products have different identifiers,
        // and we tell Unity IAP this by using the IDs class.

        builder.AddProduct("100.gold.coins", ProductType.Consumable, new IDs
            {
                {"com.unity3d.unityiap.unityiapdemo.100goldcoins.7", MacAppStore.Name},
                {"100.gold.coins", AmazonApps.Name},
                {"100.gold.coins", AppleAppStore.Name}
            }
#if USE_PAYOUTS
                , new List<PayoutDefinition> {
                new PayoutDefinition(PayoutType.Item, "", 1, "item_id:76543"),
                new PayoutDefinition(PayoutType.Currency, "gold", 50)
                }
#endif //USE_PAYOUTS
                );

        builder.AddProduct("500.gold.coins", ProductType.Consumable, new IDs
            {
                {"com.unity3d.unityiap.unityiapdemo.500goldcoins.7", MacAppStore.Name},
                {"500.gold.coins", AmazonApps.Name},
            }
#if USE_PAYOUTS
        , new PayoutDefinition(PayoutType.Currency, "gold", 500)
#endif //USE_PAYOUTS
        );

        builder.AddProduct("300.gold.coins", ProductType.Consumable, new IDs
            {
            }
#if USE_PAYOUTS
        , new List<PayoutDefinition> {
            new PayoutDefinition(PayoutType.Item, "", 1, "item_id:76543"),
            new PayoutDefinition(PayoutType.Currency, "gold", 50)
        }
#endif //USE_PAYOUTS
        );

        builder.AddProduct("sub1", ProductType.Subscription, new IDs
        {
        });

        builder.AddProduct("sub2", ProductType.Subscription, new IDs
        {
        });


        // Write Amazon's JSON description of our products to storage when using Amazon's local sandbox.
        // This should be removed from a production build.
        //builder.Configure<IAmazonConfiguration>().WriteSandboxJSON(builder.products);

#if INTERCEPT_PROMOTIONAL_PURCHASES
        // On iOS and tvOS we can intercept promotional purchases that come directly from the App Store.
        // On other platforms this will have no effect; OnPromotionalPurchase will never be called.
        builder.Configure<IAppleConfiguration>().SetApplePromotionalPurchaseInterceptorCallback(OnPromotionalPurchase);
        Debug.Log("Setting Apple promotional purchase interceptor callback");
#endif

#if RECEIPT_VALIDATION
        string appIdentifier;
        #if UNITY_5_6_OR_NEWER
        appIdentifier = Application.identifier;
        #else
        appIdentifier = Application.bundleIdentifier;
        #endif
        try
        {
            validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), appIdentifier);
        }
        catch (NotImplementedException exception)
        {
            Debug.Log("Cross Platform Validator Not Implemented: " + exception);
        }
#endif

        // Now we're ready to initialize Unity IAP.
        UnityPurchasing.Initialize(this, builder);
    }

    /// <summary>
    /// This will be called after a call to IAppleExtensions.RestoreTransactions().
    /// </summary>
    private void OnTransactionsRestored(bool success)
    {
        Debug.Log("Transactions restored." + success);
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
    private void OnDeferred(Product item)
    {
        Debug.Log("Purchase deferred: " + item.definition.id);
    }

#if INTERCEPT_PROMOTIONAL_PURCHASES
    private void OnPromotionalPurchase(Product item) {
        Debug.Log("Attempted promotional purchase: " + item.definition.id);

        // Promotional purchase has been detected. Handle this event by, e.g. presenting a parental gate.
        // Here, for demonstration purposes only, we will wait five seconds before continuing the purchase.
        StartCoroutine(ContinuePromotionalPurchases());
    }

    private IEnumerator ContinuePromotionalPurchases()
    {
        Debug.Log("Continuing promotional purchases in 5 seconds");
        yield return new WaitForSeconds(5);
        Debug.Log("Continuing promotional purchases now");
        m_AppleExtensions.ContinuePromotionalPurchases (); // iOS and tvOS only; does nothing on Mac
    }
#endif

    private void InitUI(IEnumerable<Product> items)
    {
        // Show Restore, Register, and Validate buttons on supported platforms
        restoreButton.gameObject.SetActive(true);

        ClearProductUIs();

        restoreButton.onClick.AddListener(RestoreButtonClick);

        versionText.text = "Unity version: " + Application.unityVersion + "\n" +
                           "IAP version: " + StandardPurchasingModule.Instance().Version;
    }

    /// <summary>
    /// Triggered when the user presses the <c>Buy</c> button on a product user interface component.
    /// </summary>
    /// <param name="productID">The product identifier to buy</param>
    public void PurchaseButtonClick(string productID)
    {
        if (m_PurchaseInProgress == true)
        {
            Debug.Log("Please wait, purchase in progress");
            return;
        }

        if (m_Controller == null)
        {
            Debug.LogError("Purchasing is not initialized");
            return;
        }

        if (m_Controller.products.WithID(productID) == null)
        {
            Debug.LogError("No product has id " + productID);
            return;
        }

        // Don't need to draw our UI whilst a purchase is in progress.
        // This is not a requirement for IAP Applications but makes the demo
        // scene tidier whilst the fake purchase dialog is showing.
        m_PurchaseInProgress = true;

        //Sample code how to add accountId in developerPayload to pass it to getBuyIntentExtraParams
        //Dictionary<string, string> payload_dictionary = new Dictionary<string, string>();
        //payload_dictionary["accountId"] = "Faked account id";
        //payload_dictionary["developerPayload"] = "Faked developer payload";
        //m_Controller.InitiatePurchase(m_Controller.products.WithID(productID), MiniJson.JsonEncode(payload_dictionary));
        m_Controller.InitiatePurchase(m_Controller.products.WithID(productID), "developerPayload");

    }

    /// <summary>
    /// Triggered when the user presses the restore button.
    /// </summary>
    public void RestoreButtonClick()
    {
        if (Application.platform == RuntimePlatform.WSAPlayerX86 ||
                 Application.platform == RuntimePlatform.WSAPlayerX64 ||
                 Application.platform == RuntimePlatform.WSAPlayerARM)
        {
            m_MicrosoftExtensions.RestoreTransactions();
        }
        else if (m_IsGooglePlayStoreSelected)
        {
            m_GooglePlayStoreExtensions.RestoreTransactions(OnTransactionsRestored);
        }
        else
        {
            m_AppleExtensions.RestoreTransactions(OnTransactionsRestored);
        }
    }

    private void ClearProductUIs()
    {
        foreach (var productUIKVP in m_ProductUIs)
        {
            GameObject.Destroy(productUIKVP.Value.gameObject);
        }
        m_ProductUIs.Clear();
    }

    private void AddProductUIs(Product[] products)
    {
        ClearProductUIs();

        var templateRectTransform = productUITemplate.GetComponent<RectTransform>();
        var height = templateRectTransform.rect.height;
        var currPos = templateRectTransform.localPosition;

        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, products.Length * height);

        foreach (var p in products)
        {
            var newProductUI = GameObject.Instantiate(productUITemplate.gameObject) as GameObject;
            newProductUI.transform.SetParent(productUITemplate.transform.parent, false);
            var rect = newProductUI.GetComponent<RectTransform>();
            rect.localPosition = currPos;
            currPos += Vector3.down * height;
            newProductUI.SetActive(true);
            var productUIComponent = newProductUI.GetComponent<IAPDemoProductUI>();
            productUIComponent.SetProduct(p, PurchaseButtonClick);

            m_ProductUIs[p.definition.id] = productUIComponent;
        }
    }

    private void UpdateProductUI(Product p)
    {
        if (m_ProductUIs.ContainsKey(p.definition.id))
        {
            m_ProductUIs[p.definition.id].SetProduct(p, PurchaseButtonClick);
        }
    }

    private void UpdateProductPendingUI(Product p, int secondsRemaining)
    {
        if (m_ProductUIs.ContainsKey(p.definition.id))
        {
            m_ProductUIs[p.definition.id].SetPendingTime(secondsRemaining);
        }
    }

    private bool NeedRestoreButton()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer ||
               Application.platform == RuntimePlatform.OSXPlayer ||
               Application.platform == RuntimePlatform.tvOS ||
               Application.platform == RuntimePlatform.WSAPlayerX86 ||
               Application.platform == RuntimePlatform.WSAPlayerX64 ||
               Application.platform == RuntimePlatform.WSAPlayerARM;
    }

    private void LogProductDefinitions()
    {
        var products = m_Controller.products.all;
        foreach (var product in products)
        {
#if UNITY_5_6_OR_NEWER
            Debug.Log(string.Format("id: {0}\nstore-specific id: {1}\ntype: {2}\nenabled: {3}\n", product.definition.id, product.definition.storeSpecificId, product.definition.type.ToString(), product.definition.enabled ? "enabled" : "disabled"));
#else
            Debug.Log(string.Format("id: {0}\nstore-specific id: {1}\ntype: {2}\n", product.definition.id,
                product.definition.storeSpecificId, product.definition.type.ToString()));
#endif
        }
    }
}
