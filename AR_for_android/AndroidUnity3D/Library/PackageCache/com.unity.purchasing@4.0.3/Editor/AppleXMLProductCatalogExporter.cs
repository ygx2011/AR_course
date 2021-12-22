using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Xml.Linq;
using UnityEngine.Purchasing;
using ExporterValidationResults = UnityEditor.Purchasing.ProductCatalogEditor.ExporterValidationResults;

namespace UnityEditor.Purchasing
{
    /// <summary>
    /// Exports a ProductCatalog to the format expected for Apple's XML Delivery.
    /// Apple Documentation: http://help.apple.com/itc/appsspec/#/itcc10d06ec8
    /// </summary>
    internal class AppleXMLProductCatalogExporter : ProductCatalogEditor.IProductCatalogExporter
    {

        internal static string kMandatoryExportFolder;
        internal List<string> kFilesToCopy = new List<string> ();
        private const string kNewLine = "\n";

        public string DisplayName {
            get {
                return "Apple XML Delivery";
            }
        }

        public string DefaultFileName {
            get {
                return "metadata";
            }
        }

        public string FileExtension {
            get {
                return "xml";
            }
        }

        public string StoreName {
            get {
                return AppleAppStore.Name;
            }
        }

        public string MandatoryExportFolder {
            get {
                return kMandatoryExportFolder;
            }
        }

        public List<string> FilesToCopy {
            get {
                return kFilesToCopy;
            }
        }

        public bool SaveCompletePackage {
            get {
                return true;
            }
        }

        public string Export(ProductCatalog catalog)
        {
            // Get a list of all the locales that have a value for at least one
            // product, and are valid for Apple export. Order must be preserved
            // throughout this method, so this is converted to a list and then
            // wrapped in a ReadOnlyCollection to prevent mutation.
            var localesToExport = new ReadOnlyCollection<TranslationLocale>(new List<TranslationLocale>(GetLocalesToExport(catalog)));
            XDeclaration declaration = new XDeclaration("1.0", "utf-8", "yes");
            XDocument document = new XDocument();
            XNamespace ns = "http://apple.com/itunes/importer";
            XElement package = new XElement(ns + "package",
                new XAttribute("version", "software5.7"));
            document.Add(package);
            package.Add(new XElement(ns + "provider", catalog.appleTeamID));
            package.Add(new XElement(ns + "team_id", catalog.appleTeamID));
            XElement software = new XElement(ns + "software");
            package.Add(software);
            software.Add(new XElement(ns + "vendor_id", catalog.appleSKU));
            XElement softwareMetadata = new XElement(ns + "software_metadata");
            software.Add(softwareMetadata);
            XElement inAppPurchases = new XElement(ns + "in_app_purchases");
            softwareMetadata.Add(inAppPurchases);

            foreach (var item in catalog.allProducts) {
                XElement inAppPurchase = new XElement(ns + "in_app_purchase", 
                    new XElement(ns + "product_id", item.GetStoreID(AppleAppStore.Name) ?? item.id),
                    new XElement(ns + "reference_name", item.id),
                    new XElement(ns + "type", ProductTypeString(item)));
                XElement products = new XElement(ns + "products");
                inAppPurchase.Add(products);
                XElement product = new XElement(ns + "product",
                    new XElement(ns + "cleared_for_sale", true),
                    new XElement(ns + "wholesale_price_tier", item.applePriceTier));
                products.Add(product);

                XElement locales = new XElement(ns + "locales");
                inAppPurchase.Add(locales);
                // Variable number of localizations, not every product will specify a localization for every language
                // so some of the these descriptions may be missing, in which case we just skip it.
                foreach (var loc in localesToExport) {
                    LocalizedProductDescription desc = item.defaultDescription.googleLocale == loc ? item.defaultDescription : item.GetDescription(loc);
                    if (desc != null) {
                        XElement locale = new XElement(ns + "locale", 
                            new XAttribute("name", LocaleToAppleString(loc)),
                            new XElement(ns + "title", desc.Title),
                            new XElement(ns + "description", desc.Description));
                        locales.Add(locale);
                    }
                }

                XElement reviewScreenshot = new XElement(ns + "review_screenshot");
                inAppPurchase.Add(reviewScreenshot);
                reviewScreenshot.Add(new XElement(ns + "file_name", Path.GetFileName(item.screenshotPath)));
                FileInfo fileInfo = new FileInfo(item.screenshotPath);
                if (fileInfo.Exists) {
                    reviewScreenshot.Add(new XElement(ns + "size", fileInfo.Length));
                    reviewScreenshot.Add(new XElement(ns + "checksum", GetMD5Hash(fileInfo)));
                }

                inAppPurchases.Add(inAppPurchase);
            }
            // Split the declaration and the document because we want UTF-8, not UTF-16.
            return declaration.ToString() + kNewLine + document.ToString();
        }

        public ExporterValidationResults Validate(ProductCatalog catalog)
        {
            var results = new ExporterValidationResults();

            // Warn if exporting an empty catalog
            if (catalog.allProducts.Count == 0) {
                results.warnings.Add("Catalog is empty");
            }

            // Check for duplicate IDs
            var usedIds = new HashSet<string>();
            foreach (var product in catalog.allProducts) {
                if (usedIds.Contains(product.id)) {
                    results.errors.Add("More than one product uses the ID \"" + product.id + "\"");
                }
                usedIds.Add(product.id);
            }

            // Check for duplicate store IDs
            var usedStoreIds = new HashSet<string>();
            foreach (var product in catalog.allProducts) {
                var storeID = product.GetStoreID(AppleAppStore.Name);
                if (!string.IsNullOrEmpty(storeID)) {
                    if (usedStoreIds.Contains(storeID)) {
                        results.errors.Add("More than one product uses the Apple store ID \"" + storeID + "\"");
                    }
                    usedIds.Add(product.id);
                }
            }

            // Check for duplicate runtime IDs -- this conflict could occur if a product has a base ID that is the
            // same as another product's store-specific ID
            var runtimeIDs = new HashSet<string>();
            foreach (var product in catalog.allProducts) {
                var runtimeID = product.GetStoreID(AppleAppStore.Name) ?? product.id;
                if (runtimeIDs.Contains(runtimeID)) {
                    results.errors.Add("More than one product is identified by the ID \"" + runtimeID + "\"");
                }
                runtimeIDs.Add(runtimeID);
            }

            // Check SKU
            if (string.IsNullOrEmpty(catalog.appleSKU)) {
                results.fieldErrors["appleSKU"] = "Apple SKU is required. Find this in iTunesConnect.";
            } else {
                kMandatoryExportFolder = catalog.appleSKU + ".itmsp";
            }

            // Check Team ID
            if (string.IsNullOrEmpty(catalog.appleTeamID)) {
                results.fieldErrors["appleTeamID"] = "Apple Team ID is required. Find this on https://developer.apple.com.";
            }

            return results;
        }

        public ExporterValidationResults Validate(ProductCatalogItem item)
        {
            var results = new ExporterValidationResults();

            // Check for missing IDs
            if (string.IsNullOrEmpty(item.id)) {
                results.fieldErrors["id"] = "ID is required";
            }

            // Check for missing title
            if (string.IsNullOrEmpty(item.defaultDescription.Title)) {
                results.fieldErrors["defaultDescription.Title"] = "Title is required";
            }

            // Check for missing description
            if (string.IsNullOrEmpty(item.defaultDescription.Description)) {
                results.fieldErrors["defaultDescription.Description"] = "Description is required";
            }

            // Check for screenshot
            if (string.IsNullOrEmpty(item.screenshotPath)) {
                results.fieldErrors["screenshotPath"] = "Screenshot is required";
            } else {
                kFilesToCopy.Add(item.screenshotPath);
            }

            return results;
        }

        private HashSet<TranslationLocale> GetLocalesToExport(ProductCatalog catalog)
        {
            var locs = new HashSet<TranslationLocale>();

            foreach (var item in catalog.allProducts) {
                if (item.defaultDescription.googleLocale.SupportedOnApple())
                    locs.Add(item.defaultDescription.googleLocale);

                foreach (var desc in item.translatedDescriptions) {
                    if (desc.googleLocale.SupportedOnApple())
                        locs.Add(desc.googleLocale);
                }
            }

            return locs;
        }

        private static string ProductTypeString(ProductCatalogItem item)
        {
            switch (item.type) {
            case ProductType.Consumable:
                return "consumable";
            case ProductType.NonConsumable:
                return "non-consumable";
            case ProductType.Subscription:
                return "subscription";
            }

            return string.Empty;
        }

        private static string LocaleToAppleString(TranslationLocale loc)
        {
            switch (loc) {
                // Apple uses Hans and Hant, rather than Cn and TW
                case TranslationLocale.zh_CN:
                    return "zh-Hans";
                case TranslationLocale.zh_TW:
                    return "zh-Hant";
                // Application Loader (and iTunes Connect) prefer these
                // languages to have a two-letter code
                // Discovered here:
                // http://kitefaster.com/2016/03/09/itunes-connect-api-app-store-and-corresponding-xcode-language-codes/
                case TranslationLocale.ko_KR:
                case TranslationLocale.da_DK:
                case TranslationLocale.th_TH:
                case TranslationLocale.ms_MY:
                case TranslationLocale.fi_FI:
                case TranslationLocale.sv_SE:
                case TranslationLocale.it_IT:
                case TranslationLocale.vi_VN:
                case TranslationLocale.tr_TR:
                case TranslationLocale.ja_JP:
                case TranslationLocale.el_GR:
                case TranslationLocale.id_ID:
                case TranslationLocale.no_NO:
                    return loc.ToString().Split('_')[0];
                default:
                    return loc.ToString().Replace('_', '-');
            }
        }

        public ProductCatalog NormalizeToType(ProductCatalog catalog)
        {
            return catalog;
        }

        public static string GetMD5Hash(FileInfo fileInfo)
        {
            MD5 md5 = MD5.Create();
            FileStream fileStream = fileInfo.OpenRead();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5.ComputeHash(fileStream);

            // Create a new StringBuilder to collect the bytes
            // and create a string.
            StringBuilder stringBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return stringBuilder.ToString();
        }
    }
}
