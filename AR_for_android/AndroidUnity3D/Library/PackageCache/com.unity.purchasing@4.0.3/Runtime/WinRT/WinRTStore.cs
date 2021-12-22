using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Core;

#pragma warning disable 4014
namespace UnityEngine.Purchasing.Default {
    class WinRTStore : IWindowsIAP {

        private IWindowsIAPCallback callback;
        private ICurrentApp currentApp;
        private Dictionary<string, string> transactionIdToProductId = new Dictionary<string, string>();

        private int m_loginDelay;

        public WinRTStore(ICurrentApp currentApp)
        {
            this.currentApp = currentApp;
        }


        public void Initialize(IWindowsIAPCallback callback)
        {
            this.callback = callback;
            this.m_loginDelay = 30;
        }


        public void Initialize(IWindowsIAPCallback callback, int delayTime = 30)
        {
            this.callback = callback;
            this.m_loginDelay = delayTime;
        }

        public void SetLoginDelay(int delayTime)
        {
            this.m_loginDelay = delayTime;
        }

        public int LoginDelay()
        {
            return m_loginDelay;
        }

        public void RetrieveProducts(bool persistent) {
            RunOnUIThread(() => {
                if (LoginDelay() > 0)
                {
                    PollForProducts(persistent, 0, LoginDelay(), true, false);
                }
                else
                {
                    PollForProducts(persistent, 0);
                }
            });
        }

        private async void PollForProducts(bool persistent, int delay, int retryCount = 10, bool tryLogin = false, bool loginAttempted = false, bool productsOnly = false) {
            await Task.Delay(delay);
            try {
                var result = await DoRetrieveProducts(productsOnly);
                callback.OnProductListReceived(result);
            }
            catch (Exception e)
            {

                LogError("PollForProducts() Exception (persistent = {0}, delay = {1}, retry = {2}), exception: {3}", persistent, delay, retryCount, e.Message);

                // NB: persistent here is used to distinguish when this is used by restoreTransactions() so we will
                // keep it intact and supplement for retries on initialization
                //
                if (persistent) {
                    // This seems to indicate the App is not uploaded on
                    // the dev portal, but is undocumented by Microsoft.
                    if (e.Message.Contains("801900CC")) {
                        LogError("Exception loading listing information: {0}", e.Message);
                        callback.OnProductListError("AppNotKnown");
                        // JDRjr: in the main store code this is not being checked correctly
                        // and will result in repeated init attempts. Leaving it for now, but broken...
                    }
                    else if (e.Message.Contains("80070525"))
                    {
                        LogError("PollForProducts() User not signed in error HResult = 0x{0:X} (delay = {1}, retry = {2})", e.HResult, delay, retryCount);
                        if((delay == 0)&&(productsOnly == false))
                        {
                            // First time failure give products only a try
                            PollForProducts(true, 1000, retryCount, tryLogin, loginAttempted, true);
                        }
                        else
                        {
                            // Gonna call this an error
                            LogError("Calling OnProductListError() delay = {0}, productsOnly = {1}", delay, productsOnly);
                            callback.OnProductListError("801900CC because the C# code is broken");
                        }
                    }
                    else {
                        // other (no special handling) error codes
                        // Wait up to 5 mins.
                        // JDRjr: this seems like too long...
                        delay = Math.Max(5000, delay);
                        var newDelay = Math.Min(300000, delay * 2);
                        PollForProducts(true, newDelay);
                    }
                }
                else
                {
                    // This is a restore attempt that has thrown an exception
                    // We should allow for a login attempt here as well...
                    if (tryLogin == true)
                    {
                        var uri = new Uri("ms-windows-store://signin");
                        var loginResult = await global::Windows.System.Launcher.LaunchUriAsync(uri);

                        PollForProducts(true, 1000, retryCount, false, true, false);
                    }
                    else
                    {
                        if (retryCount > 0)
                        {
                            if (loginAttempted)
                            {
                                // Will wait for retryCount seconds...
                                PollForProducts(true, 1000, --retryCount, false, true);
                            }
                            else
                            {
                                // Wait up to 5 mins.
                                delay = Math.Max(5000, delay);
                                var newDelay = Math.Min(300000, delay * 2);
                                PollForProducts(true, newDelay, --retryCount, false, false);
                            }
                        }
                        else
                        {
                            callback.OnProductListError("801900CC because the C# code is broken");
                        }
                    }
                }
            } // end of catch()
        }

        private async Task<WinProductDescription[]> DoRetrieveProducts(bool productsOnly) {
            ListingInformation result = await currentApp.LoadListingInformationAsync();

            if(productsOnly == false)
            {
                // We need a comprehensive list of transaction IDs for owned items.
                // Microsoft make this difficult by failing to provide transaction IDs
                // on product licenses that are owned.
                // Therefore two data sets are joined; unfulfilled consumables (which have product IDs)
                // and transactions from the App receipt (Durables).
                var unfulfilledConsumables = await currentApp.GetUnfulfilledConsumablesAsync();
                var transactionMap = unfulfilledConsumables.ToDictionary(x => x.ProductId, x => x.TransactionId.ToString());

                // Add transaction IDs from our app receipt.
                string appReceipt = null;
                try {
                    appReceipt = await currentApp.RequestAppReceiptAsync();
                } catch (Exception e) {
                    LogError("Unable to retrieve app receipt:{0}", e.Message);
                }

                var receiptTransactions = XMLUtils.ParseProducts(appReceipt);
                foreach (var receiptTran in receiptTransactions) {
                    transactionMap[receiptTran.productId] = receiptTran.transactionId;
                }

                // Create fake transaction Ids for any owned items that we can't find transaction IDs for.
                foreach (var license in currentApp.LicenseInformation.ProductLicenses) {
                    if (!transactionMap.ContainsKey(license.Key)) {
                        transactionMap[license.Key] = license.Key.GetHashCode().ToString();
                    }
                }


                // Construct our products including receipts and transaction ID where owned
                var productDescriptions = from listing in result.ProductListings.Values
                               let priceDecimal = TryParsePrice(listing.FormattedPrice)
                               let transactionId = transactionMap.ContainsKey(listing.ProductId) ? transactionMap[listing.ProductId] : null
                               let receipt = transactionId == null ? null : appReceipt
                               select new WinProductDescription(listing.ProductId,
                                   listing.FormattedPrice, listing.Name, string.Empty, RegionInfo.CurrentRegion.ISOCurrencySymbol,
                                   priceDecimal, receipt, transactionId);

                // Transaction IDs tracked for finalising transactions
                transactionIdToProductId = transactionMap.ToDictionary(x => x.Value, x => x.Key);
                return productDescriptions.ToArray();
            }
            else
            {
                var productDescriptions = from listing in result.ProductListings.Values
                                          let priceDecimal = TryParsePrice(listing.FormattedPrice)
                                          select new WinProductDescription(listing.ProductId,
                                              listing.FormattedPrice, listing.Name, string.Empty, RegionInfo.CurrentRegion.ISOCurrencySymbol,
                                              priceDecimal, null, null);
                return productDescriptions.ToArray();
            }
        }

        private decimal TryParsePrice(string formattedPrice) {
            decimal price = 0;
            decimal.TryParse(formattedPrice, NumberStyles.Currency, CultureInfo.CurrentCulture, out price);
            return price;
        }

        public void Purchase(string productId) {
            RunOnUIThread(async () => {
                try {
                    var result = await currentApp.RequestProductPurchaseAsync(productId);
                    switch (result.Status) {
                        case ProductPurchaseStatus.Succeeded:
                            onPurchaseSucceeded(productId, result.ReceiptXml, result.TransactionId);
                            break;
                        case ProductPurchaseStatus.NotFulfilled:
                        case ProductPurchaseStatus.AlreadyPurchased:
                        case ProductPurchaseStatus.NotPurchased:
                            callback.OnPurchaseFailed(productId, result.Status.ToString());
                            break;
                    }
                }
                catch (Exception e) {
                    callback.OnPurchaseFailed(productId, e.Message);
                }
            });
        }

        private async Task FulfillConsumable(string productId, string transactionId) {
            try {
                var result = await currentApp.ReportConsumableFulfillmentAsync(productId, Guid.Parse(transactionId));

                if (FulfillmentResult.Succeeded == result) {
                    lock (transactionIdToProductId) {
                        transactionIdToProductId.Remove(transactionId);
                    }
                }
                // It doesn't matter if the consumption succeeds or not.
                // If it doesn't, it will eventually be retried automatically.
            }
            catch (Exception e) {
                LogError("Exception consuming {0} : {1} (non-fatal)", productId, e.Message);
            }
        }

        private void LogError(string message, params object[] formatArgs) {
            callback.logError(string.Format("UnityIAPWin8:" + message, formatArgs));
        }

        private void onPurchaseSucceeded(string productId, string receipt, Guid transactionId) {
            var tranId = transactionId.ToString();
            // Make a note of which product this transaction pertains to.
            lock (transactionIdToProductId) {
                transactionIdToProductId[tranId] = productId;
            }
            callback.OnPurchaseSucceeded(productId, receipt, tranId);
        }

        public void FinaliseTransaction(string transactionId)
        {
            RunOnUIThread(() =>
            {
                // We occasionally supply null transaction IDs,
                // to the biller for owned non consumables.
                // The biller will try to finalise these, so we
                // ignore them.
                if (!string.IsNullOrEmpty(transactionId))
                {
                    if (transactionIdToProductId.ContainsKey(transactionId))
                    {
                        FulfillConsumable(transactionIdToProductId[transactionId], transactionId);
                    }
                    else
                    {
                        callback.logError("Nothing to fulfill for transaction " + transactionId);
                    }
                }
            });
        }

        private static void RunOnUIThread(Action a)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                a();
            });
        }

        /// <summary>
        /// Builds a dummy list of Products.
        /// </summary>
        /// <param name="products"> The list of product descriptions. </param>
        public void BuildDummyProducts(List<WinProductDescription> products)
        {
            currentApp.BuildMockProducts(products);
        }
    }
}
#pragma warning restore 4014
