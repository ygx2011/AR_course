using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Storage;

namespace UnityEngine.Purchasing.Default
{
    class UnibillCurrentAppSimulator : ICurrentApp
    {
        public void BuildMockProducts(List<WinProductDescription> winProducts) {
            StorageFolder myfolder = ApplicationData.Current.LocalFolder;
            if (!Exists("WindowsStoreProxy.xml")) {
                myfolder.CreateFileAsync("WindowsStoreProxy.xml").AsTask().Wait();
            }
            var file = myfolder.GetFileAsync("WindowsStoreProxy.xml").AsTask().Result;

            FileIO.WriteTextAsync(file, BuildDoc(winProducts).ToString()).AsTask().Wait();

            var task = CurrentAppSimulator.ReloadSimulatorAsync(file).AsTask();
            task.Wait();
        }

        private bool Exists(string fileName) {
            try {
                var task = ApplicationData.Current.LocalFolder.GetFileAsync(fileName).AsTask();
                task.Wait();
                if (task.Exception == null) {
                    return true;
                }
            }
            catch {
                // Filenotfound
            }
            return false;
        }

        private XDocument BuildDoc(List<WinProductDescription> winProducts) {
            XNamespace xml = "xml";
            XElement CurrentApp =
                    new XElement("CurrentApp",
                        new XElement("ListingInformation",
                            new XElement("App",
                                new XElement("AppId", "2B14D306-D8F8-4066-A45B-0FB3464C67F2"),
                                new XElement("LinkUri", "http://apps.windows.microsoft.com/app/2B14D306-D8F8-4066-A45B-0FB3464C67F2"),
                                new XElement("CurrentMarket", "en-US"),
                                new XElement("AgeRating", "3"),
                                new XElement("MarketData", new XAttribute(XNamespace.Xml + "lang", "en-us"),
                                    new XElement("Name", "Unity IAP demo full license"),
                                    new XElement("Description", "Unity IAP Mock mode"),
                                    new XElement("Price", "4.99"),
                                    new XElement("CurrencySymbol", "$")
                                )
                            ),
                          winProducts.Select(x => new XElement("Product", new XAttribute("ProductId", x.platformSpecificID), new XAttribute("ProductType", x.consumable ? "Consumable" : "Durable"),
                                        new XElement("MarketData", new XAttribute(XNamespace.Xml + "lang", "en-us"),
                                        new XElement("Name", x.title),
                                        new XElement("Price", 0.01m),
                                        new XElement("CurrencySymbol", RegionInfo.CurrentRegion.CurrencySymbol)
                                    )
                                )
                            )
                        ),
                        new XElement("LicenseInformation",
                            new XElement("App",
                                new XElement("IsActive", "true"),
                                new XElement("IsTrial", "false")
                            )
                        )
                    );
            var doc = new XDocument();
            doc.Add(CurrentApp);
            return doc;
        }

        public IAsyncOperation<IReadOnlyList<UnfulfilledConsumable>> GetUnfulfilledConsumablesAsync()
        {
            return CurrentAppSimulator.GetUnfulfilledConsumablesAsync();
        }

        public IAsyncOperation<ListingInformation> LoadListingInformationAsync()
        {
            return CurrentAppSimulator.LoadListingInformationAsync();
        }


        public IAsyncOperation<FulfillmentResult> ReportConsumableFulfillmentAsync(string productId, Guid transactionId)
        {
            return CurrentAppSimulator.ReportConsumableFulfillmentAsync(productId, transactionId);
        }

        public IAsyncOperation<PurchaseResults> RequestProductPurchaseAsync(string productId)
        {
            return  CurrentAppSimulator.RequestProductPurchaseAsync(productId);
        }

        public IAsyncOperation<string> RequestProductReceiptAsync(string productId) {
            return CurrentAppSimulator.GetProductReceiptAsync(productId);
        }

        public LicenseInformation LicenseInformation {
            get
            {
                return CurrentAppSimulator.LicenseInformation;
            }
        }


        public IAsyncOperation<string> RequestAppReceiptAsync() {
            return CurrentAppSimulator.GetAppReceiptAsync();
        }
    }
}
