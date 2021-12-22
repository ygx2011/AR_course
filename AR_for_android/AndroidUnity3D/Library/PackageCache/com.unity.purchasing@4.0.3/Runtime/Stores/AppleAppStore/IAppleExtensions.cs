using System;
using System.Collections.Generic;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Access iOS specific functionality.
    /// </summary>
    public interface IAppleExtensions : IStoreExtension
    {
        /// <summary>
        /// Fetch the latest App Receipt from Apple.
        /// This requires an Internet connection and will prompt the user for their credentials.
        /// </summary>
        /// <param name="successCallback">This action will be called when the refresh is successful. The receipt will be passed through.</param>
        /// <param name="errorCallback">This action will be called when the refresh is in error.</param>
        void RefreshAppReceipt(Action<string> successCallback, Action errorCallback);

        /// <summary>
        /// Fetch the most recent iOS 6 style transaction receipt for the given product.
        /// This is necessary to validate Ask-to-buy purchases, which don't show up in the App Receipt.
        /// </summary>
        /// <param name="product">The product to fetch the receipt from.</param>
        /// <returns>Returns the receipt if the product has a receipt or an empty string.</returns>
        string GetTransactionReceiptForProduct (Product product);

        /// <summary>
        /// Initiate a request to Apple to restore previously made purchases.
        /// </summary>
        /// <param name="callback">Action will be called when the request to Apple comes back. The bool will be true if it was successful or false if it was not.</param>
        void RestoreTransactions(Action<bool> callback);

        /// <summary>
        /// Called when a processing a purchase from Apple that is in the "onProductPurchaseDeferred" state.
        /// </summary>
        /// <param name="callback">Action will be called with the product that is in the "onProductPurchaseDeferred" state.</param>
        void RegisterPurchaseDeferredListener(Action<Product> callback);

        /// <summary>
        /// Modify payment request with "applicationUsername" for fraud detection.
        /// </summary>
        /// <param name="applicationUsername">The application Username for fraud detection.</param>
        void SetApplicationUsername(string applicationUsername);

        /// <summary>
        /// For testing purposes only.
        ///
        /// Modify payment request for testing ask-to-buy.
        /// </summary>
        bool simulateAskToBuy { get; set; }

        /// <summary>
        /// Overrides the promoted product order on the device.
        /// </summary>
        /// <param name="products">The new order of promoted products for the device.</param>
        void SetStorePromotionOrder(List<Product> products);

        /// <summary>
        /// Override the visibility of a product on the device.
        /// </summary>
        /// <param name="product">Product to change visibility.</param>
        /// <param name="visible">The new product visibility.</param>
        void SetStorePromotionVisibility(Product product, AppleStorePromotionVisibility visible);

        /// <summary>
        /// Call the `UnityEarlyTransactionObserver.initiateQueuedPayments`
        /// </summary>
        void ContinuePromotionalPurchases();

        /// <summary>
        /// Extracting Introductory Price subscription related product details.
        /// </summary>
        /// <returns>returns the Introductory Price subscription related product details or an empty dictionary</returns>
        Dictionary<string, string> GetIntroductoryPriceDictionary();

        /// <summary>
        /// Extracting product details.
        /// </summary>
        /// <returns>returns product details or an empty dictionary</returns>
        Dictionary<string, string> GetProductDetails();

        /// <summary>
        /// Initiate Apple iOS 14 Subscription Offer Code redemption API, presentCodeRedemptionSheet
        /// </summary>
        void PresentCodeRedemptionSheet();
    }

    /// <summary>
    /// This enum is a C# representation of the Apple object `SKProductStorePromotionVisibility`.
    /// https://developer.apple.com/documentation/storekit/skproductstorepromotionvisibility?changes=latest__7
    ///
    /// Converted to a string (ToString) to pass to Apple native code, so do not change these names.
    /// </summary>
    public enum AppleStorePromotionVisibility
    {
        /// <summary>
        /// C# representation of Apple's object `SKProductStorePromotionVisibility.default`
        /// https://developer.apple.com/documentation/storekit/skproductstorepromotionvisibility/default?changes=latest__7
        /// </summary>
        Default,
        /// <summary>
        /// C# representation of Apple's object `SKProductStorePromotionVisibility.hide`
        /// https://developer.apple.com/documentation/storekit/skproductstorepromotionvisibility/hide?changes=latest__7
        /// </summary>
        Hide,
        /// <summary>
        /// C# representation of Apple's object `SKProductStorePromotionVisibility.show`
        /// https://developer.apple.com/documentation/storekit/skproductstorepromotionvisibility/show?changes=latest__7
        /// </summary>
        Show
    }
}
