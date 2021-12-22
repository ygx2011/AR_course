using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Store;
using Windows.Foundation;

namespace UnityEngine.Purchasing.Default
{
    interface ICurrentApp
    {
        // Used for setting up test data by our mock implementation.
        void BuildMockProducts(List<WinProductDescription> products);

        IAsyncOperation<IReadOnlyList<UnfulfilledConsumable>> GetUnfulfilledConsumablesAsync();

        IAsyncOperation<ListingInformation> LoadListingInformationAsync();

        IAsyncOperation<FulfillmentResult> ReportConsumableFulfillmentAsync(string productId, Guid transactionId);

        IAsyncOperation<PurchaseResults> RequestProductPurchaseAsync(string productId);

        IAsyncOperation<string> RequestAppReceiptAsync();

        LicenseInformation LicenseInformation { get; }
    }
}
