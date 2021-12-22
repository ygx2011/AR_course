using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Store;
using Windows.Foundation;

namespace UnityEngine.Purchasing.Default
{
    /// <summary>
    /// Wrapper class for the Windows Store API CurrentApp class.
    /// </summary>
    public class CurrentApp : ICurrentApp
    {
        /// <summary>
        /// Returns a list of purchased consumable in-app products that have not been reported to the Microsoft Store as fulfilled..
        /// </summary>
        /// <returns> The list of purchased consumable products that have not been reported as fulfilled. </returns>
        public IAsyncOperation<IReadOnlyList<UnfulfilledConsumable>> GetUnfulfilledConsumablesAsync()
        {
            return global::Windows.ApplicationModel.Store.CurrentApp.GetUnfulfilledConsumablesAsync();
        }

        /// <summary>
        /// Loads the app's listing information asynchronously.
        /// Additionally, the listing information for available in-app products is also provided.
        /// </summary>
        /// <returns> The ListingInformation that contains info (ex: name, price) specific to the market the user currently resides in. </returns>
        public IAsyncOperation<ListingInformation> LoadListingInformationAsync()
        {
            return global::Windows.ApplicationModel.Store.CurrentApp.LoadListingInformationAsync();
        }

        /// <summary>
        /// Notifies the Microsoft Store that the purchase of a consumable add-on (also called an in-app product or IAP)
        /// is fulfilled and that the user has the right to access the content.
        /// </summary>
        /// <param name="productId"> The product ID of the consumable add-on to report as fulfilled. </param>
        /// <param name="transactionId"> The transaction ID for the purchase of the consumable add-on. </param>
        /// <returns> An async operation for a <c>FulfillmentResult</c> value that indicates the status for the consumable add-on.</returns>
        public IAsyncOperation<FulfillmentResult> ReportConsumableFulfillmentAsync(string productId, Guid transactionId)
        {
            return global::Windows.ApplicationModel.Store.CurrentApp.ReportConsumableFulfillmentAsync(productId, transactionId);
        }

        /// <summary>
        /// Requests the purchase of an add-on (also called an in-app product or IAP).
        /// Additionally, calling this method displays the UI that is used to complete the transaction via the Microsoft Store.
        /// </summary>
        /// <param name="productId"> The product ID of the add-on to purchase. </param>
        /// <returns> An async operation for a <c>PurchaseResults</c> containing the results of the product purchase request.</returns>
        public IAsyncOperation<PurchaseResults> RequestProductPurchaseAsync(string productId)
        {
            return global::Windows.ApplicationModel.Store.CurrentApp.RequestProductPurchaseAsync(productId);
        }

        /// <summary>
        /// Requests the purchase of an add-on (also called an in-app product or IAP).
        /// Additionally, calling this method displays the UI that is used to complete the transaction via the Microsoft Store.
        /// </summary>
        /// <param name="productId"> The product ID of the add-on to purchase. </param>
        /// <returns> An async operation for a string providing in-app transaction details for the provided productId. </returns>
        public IAsyncOperation<string> RequestProductReceiptAsync(string productId)
        {
            return global::Windows.ApplicationModel.Store.CurrentApp.GetProductReceiptAsync(productId);
        }

        /// <summary>
        /// Read access to the current app's license metadata.
        /// </summary>
        public LicenseInformation LicenseInformation
        {
            get
            {
                return global::Windows.ApplicationModel.Store.CurrentApp.LicenseInformation;
            }
        }

        /// <summary>
        /// Requests all receipts for the purchase of the app and any in-app products.
        /// </summary>
        /// <returns> An async operation for an XML-formatted string containing all receipt information for the purchases of the app and of its products.. </returns>
        public IAsyncOperation<string> RequestAppReceiptAsync()
        {
            return global::Windows.ApplicationModel.Store.CurrentApp.GetAppReceiptAsync();
        }

        /// <summary>
        /// Dummy function implementing the building of Mock Products.
        /// </summary>
        /// <param name="products"> The list of product descriptions. </param>
        public void BuildMockProducts(List<WinProductDescription> products)
        {
            // Only implemented by the mock store.
        }
    }
}
