using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Reflection;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Represents a price in a format that is serialized as both a decimal and a double.
    /// </summary>
    [Serializable]
    public class Price : ISerializationCallbackReceiver
    {
        /// <summary>
        /// The price as a decimal.
        /// </summary>
        public decimal value;

        [SerializeField] private int[] data;

#pragma warning disable 414 // This field appears to be unused, but it is here for serialization
        [SerializeField] private double num;
#pragma warning restore 414

        /// <summary>
        /// Callback executed before Serialization.
        /// Converts value to raw data and to a double.
        /// </summary>
        public void OnBeforeSerialize()
        {
            data = decimal.GetBits(value);
            num = decimal.ToDouble(value);
        }

        /// <summary>
        /// Callback executed after Deserialization.
        /// Converts the raw data to a decimal and asigns it to value.
        /// </summary>
        public void OnAfterDeserialize()
        {
            if (data != null && data.Length == 4)
            {
                value = new decimal(data);
            }
        }
    }

    /// <summary>
    /// Represents a pair of store identifier and product id for the store.
    /// </summary>
    [Serializable]
    public class StoreID
    {
        /// <summary>
        /// The name of the store.
        /// </summary>
        public string store;

        /// <summary>
        /// The unique id of the store.
        /// </summary>
        public string id;

        /// <summary>
        /// Constructor. Simply assigns the parameters as member data.
        /// </summary>
        /// <param name="store_"> The name of the store. </param>
        /// <param name="id_">  The unique id of the store. </param>
        public StoreID(string store_, string id_)
        {
            store = store_;
            id = id_;
        }
    }

    /// <summary>
    /// The locales supported by Google for IAP product translation.
    /// </summary>
    public enum TranslationLocale
    {
        // Added for Google:
        /// <summary>
        /// Chinese (Traditional).
        /// </summary>
        zh_TW,
        /// <summary>
        /// Czech.
        /// </summary>
        cs_CZ,
        /// <summary>
        /// Danish.
        /// </summary>
        da_DK,
        /// <summary>
        /// Dutch.
        /// </summary>
        nl_NL,
        /// <summary>
        /// English (US).
        /// </summary>
        en_US,
        /// <summary>
        /// French.
        /// </summary>
        fr_FR,
        /// <summary>
        /// Finnish.
        /// </summary>
        fi_FI,
        /// <summary>
        /// German.
        /// </summary>
        de_DE,
        /// <summary>
        /// Hebrew.
        /// </summary>
        iw_IL,
        /// <summary>
        /// Hindi.
        /// </summary>
        hi_IN,
        /// <summary>
        /// Italian.
        /// </summary>
        it_IT,
        /// <summary>
        /// Japanese.
        /// </summary>
        ja_JP,
        /// <summary>
        /// Korean.
        /// </summary>
        ko_KR,
        /// <summary>
        /// Norwegian.
        /// </summary>
        no_NO,
        /// <summary>
        /// Polish.
        /// </summary>
        pl_PL,
        /// <summary>
        /// Portuguese.
        /// </summary>
        pt_PT,
        /// <summary>
        /// Russian.
        /// </summary>
        ru_RU,
        /// <summary>
        /// Spanish.
        /// </summary>
        es_ES,
        /// <summary>
        /// Swedish.
        /// </summary>
        sv_SE,
        // Added for Xiaomi:
        /// <summary>
        /// Chinese (Simplified).
        /// </summary>
        zh_CN,
        // Added for Apple:
        /// <summary>
        /// English (Australia).
        /// </summary>
        en_AU,
        /// <summary>
        /// English (Canada).
        /// </summary>
        en_CA,
        /// <summary>
        /// English (UK).
        /// </summary>
        en_GB,
        /// <summary>
        /// French (Canada).
        /// </summary>
        fr_CA,
        /// <summary>
        /// Greek.
        /// </summary>
        el_GR,
        /// <summary>
        /// Indonesian.
        /// </summary>
        id_ID,
        /// <summary>
        /// Malay.
        /// </summary>
        ms_MY,
        /// <summary>
        /// Portuguese (Brazil).
        /// </summary>
        pt_BR,
        /// <summary>
        /// Spanish (Mexico).
        /// </summary>
        es_MX,
        /// <summary>
        /// Thai.
        /// </summary>
        th_TH,
        /// <summary>
        /// Turkish.
        /// </summary>
        tr_TR,
        /// <summary>
        /// Vietnamese.
        /// </summary>
        vi_VN,
    }

    /// <summary>
    /// Class that facilitates localization code extensions.
    /// </summary>
    public static class LocaleExtensions
    {
        /// <summary>
        /// Must match 1:1 "TranslationLocale"
        /// </summary>
        private static readonly string[] Labels =
        {
            "Chinese (Traditional)",
            "Czech",
            "Danish",
            "Dutch",
            "English (U.S.)",
            "French",
            "Finnish",
            "German",
            "Hebrew",
            "Hindi",
            "Italian",
            "Japanese",
            "Korean",
            "Norwegian",
            "Polish",
            "Portuguese (Portugal)",
            "Russian",
            "Spanish (Spain)",
            "Swedish",
            "Chinese (Simplified)",
            "English (Australia)",
            "English (Canada)",
            "English (U.K.)",
            "French (Canada)",
            "Greek",
            "Indonesian",
            "Malay",
            "Portuguese (Brazil)",
            "Spanish (Mexico)",
            "Thai",
            "Turkish",
            "Vietnamese"
        };

        private static readonly TranslationLocale[] GoogleLocales =
        {
            TranslationLocale.zh_TW,
            TranslationLocale.cs_CZ,
            TranslationLocale.da_DK,
            TranslationLocale.nl_NL,
            TranslationLocale.en_US,
            TranslationLocale.fr_FR,
            TranslationLocale.fi_FI,
            TranslationLocale.de_DE,
            TranslationLocale.iw_IL,
            TranslationLocale.hi_IN,
            TranslationLocale.it_IT,
            TranslationLocale.ja_JP,
            TranslationLocale.ko_KR,
            TranslationLocale.no_NO,
            TranslationLocale.pl_PL,
            TranslationLocale.pt_PT,
            TranslationLocale.ru_RU,
            TranslationLocale.es_ES,
            TranslationLocale.sv_SE,
        };

        private static readonly TranslationLocale[] AppleLocales =
        {
            TranslationLocale.zh_CN, // Chinese (Simplified)
            TranslationLocale.zh_TW, // Chinese (Traditional)
            TranslationLocale.da_DK, // Danish
            TranslationLocale.nl_NL, // Dutch
            TranslationLocale.en_AU, // English (Australia)
            TranslationLocale.en_CA, // English (Canada)
            TranslationLocale.en_GB, // English (U.K.)
            TranslationLocale.en_US, // English (U.S.)
            TranslationLocale.fi_FI, // Finnish
            TranslationLocale.fr_FR, // French
            TranslationLocale.fr_CA, // French (Canada)
            TranslationLocale.de_DE, // German
            TranslationLocale.el_GR, // Greek
            TranslationLocale.id_ID, // Indonesian
            TranslationLocale.it_IT, // Italian
            TranslationLocale.ja_JP, // Japanese
            TranslationLocale.ko_KR, // Korean
            TranslationLocale.ms_MY, // Malay
            TranslationLocale.no_NO, // Norwegian
            TranslationLocale.pt_BR, // Portuguese (Brazil)
            TranslationLocale.pt_PT, // Portuguese (Portugal)
            TranslationLocale.ru_RU, // Russian
            TranslationLocale.es_MX, // Spanish (Mexico)
            TranslationLocale.es_ES, // Spanish (Spain)
            TranslationLocale.sv_SE, // Swedish
            TranslationLocale.th_TH, // Thai
            TranslationLocale.tr_TR, // Turkish
            TranslationLocale.vi_VN, // Vietnamese
        };

        private static string[] LabelsWithSupportedPlatforms;

        /// <summary>
        /// For every enum value in TranslationLocale, build a string with Labels + GoogleLocales for
        /// each platform supported.
        /// </summary>
        /// <returns></returns>
        public static string[] GetLabelsWithSupportedPlatforms()
        {
            if (LabelsWithSupportedPlatforms != null)
                return LabelsWithSupportedPlatforms;

            LabelsWithSupportedPlatforms = new string[Enum.GetValues(typeof(TranslationLocale)).Length];

            List<TranslationLocale> googleLocalesList = GoogleLocales.ToList();
            List<TranslationLocale> appleLocalesList = AppleLocales.ToList();

            int i = 0;
            foreach (TranslationLocale locale in Enum.GetValues(typeof(TranslationLocale)))
            {
                var platforms = new List<string>();
                if (googleLocalesList.Contains(locale))
                    platforms.Add("Google Play");
                if (appleLocalesList.Contains(locale))
                    platforms.Add("Apple");

                var platformSuffix = string.Join(", ", platforms.ToArray());

                LabelsWithSupportedPlatforms[i] = Labels[i] + " (" + platformSuffix + ")";

                i++;
            }

            return LabelsWithSupportedPlatforms;
        }

        /// <summary>
        /// Checks that a <c>TranslationLocale</c> is supported on Apple.
        /// </summary>
        /// <param name="locale"> The locale to check. </param>
        /// <returns> If the locale is supported or not. </returns>
        public static bool SupportedOnApple(this TranslationLocale locale)
        {
            return AppleLocales.Contains(locale);
        }

        /// <summary>
        /// Checks that a <c>TranslationLocale</c> is supported on Google.
        /// </summary>
        /// <param name="locale"> The locale to check. </param>
        /// <returns> If the locale is supported or not. </returns>
        public static bool SupportedOnGoogle(this TranslationLocale locale)
        {
            return GoogleLocales.Contains(locale);
        }
    }

    /// <summary>
    /// A description of an IAP product. Includes both a title and a longer description, plus an optional locale for
    /// specifying the language of this description. Characters wider than one byte are escaped as \\uXXXX for
    /// serialization to work around a bug in Unity's JSONUtility deserialization prior to Unity 5.6.
    /// </summary>
    [Serializable]
    public class LocalizedProductDescription
    {
        /// <summary>
        /// The <c>TranslationLocale</c> for GooglePlay.
        /// </summary>
        public TranslationLocale googleLocale = TranslationLocale.en_US;
        [SerializeField]
        private string title;
        [SerializeField]
        private string description;

        /// <summary>
        /// Copy this product description.
        /// </summary>
        /// <returns> A new instance identical to this object </returns>
        public LocalizedProductDescription Clone()
        {
            var desc = new LocalizedProductDescription ();

            desc.googleLocale = this.googleLocale;
            desc.Title = this.Title;
            desc.Description = this.Description;

            return desc;
        }

        /// <summary>
        /// The title of the product description.
        /// </summary>
        public string Title {
            get {
                return DecodeNonLatinCharacters(title);
            }
            set {
                title = EncodeNonLatinCharacters(value);
            }
        }

        /// <summary>
        /// The product description displayed as a string.
        /// </summary>
        public string Description {
            get {
                return DecodeNonLatinCharacters(description);
            }
            set {
                description = EncodeNonLatinCharacters(value);
            }
        }

        private static string EncodeNonLatinCharacters(string s)
        {
            if (s == null)
                return s;

            var sb = new StringBuilder();
            foreach (char c in s) {
                if (c > 127) {
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                } else {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private static string DecodeNonLatinCharacters(string s)
        {
            if (s == null)
                return s;

            return Regex.Replace(s, @"\\u(?<Value>[a-zA-Z0-9]{4})", m => {
                return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
            });
        }
    }

    /// <summary>
    /// Represents the definition of a payout in the product catalog.
    /// </summary>
    [Serializable]
    public class ProductCatalogPayout
    {
        /// <summary>
        /// Types of Product Payouts. Mirrors the <c>PayoutType</c> enum.
        /// </summary>
        /// Values here should mirror the values in the Core PayoutType enum, but we don't want to use that enum
        /// directly because it will create a dependency between the plugin and a particular core/editor version.
        public enum ProductCatalogPayoutType
        {
            /// <summary>
            /// "Other" payouts are those with a customizable payout subtype.
            /// </summary>
            Other,
            /// <summary>
            /// Payout is a currency, often paired with quantity to specify the amount.
            /// </summary>
            Currency,
            /// <summary>
            /// Payout is an item.
            /// </summary>
            Item,
            /// <summary>
            /// Payout is a resource, often used in in-game economies or for crafting features.
            /// Examples: Iron, Wood.
            /// </summary>
            Resource
        }

        // Serialize the type as a string for readability and future-proofing
        [SerializeField]
        string t = ProductCatalogPayoutType.Other.ToString();
        /// <summary>
        /// The type of the payout of the product.
        /// </summary>
        public ProductCatalogPayoutType type {
            get {
                var retval = ProductCatalogPayoutType.Other;
                if (Enum.IsDefined(typeof(ProductCatalogPayoutType), t))
                    retval = (ProductCatalogPayoutType)Enum.Parse (typeof (ProductCatalogPayoutType), t);
                return retval;
            }
            set {
                t = value.ToString ();
            }
        }

        /// <summary>
        /// ProductCatalogPayoutType as a string.
        /// </summary>
        public string typeString {
            get {
                return t;
            }
        }

        /// <summary>
        /// The maximum string length of the subtype for the "Other" payout type or any type requiring specification of a subtype.
        /// </summary>
        public const int MaxSubtypeLength = 64;

        [SerializeField]
        string st = string.Empty;

        /// <summary>
        /// The custom name for a subtype for the "Other" payout type.
        /// </summary>
        public string subtype {
            get {
                return st;
            }
            set {
                if (value.Length > MaxSubtypeLength)
                    throw new ArgumentException (string.Format ("subtype should be no longer than {0} characters", MaxSubtypeLength));
                st = value;
            }
        }

        [SerializeField]
        double q;

        /// <summary>
        /// The quantity of payout.
        /// </summary>
        public double quantity {
            get {
                return q;
            }
            set {
                q = value;
            }
        }

        /// <summary>
        /// The maximum byte length of the payout data when serialized.
        /// </summary>
        public const int MaxDataLength = 1024;

        [SerializeField]
        string d = string.Empty;
        /// <summary>
        /// The raw data of the payout.
        /// </summary>
        public string data {
            get {
                return d;
            }
            set {
                if (value.Length > MaxDataLength)
                    throw new ArgumentException (string.Format ("data should be no longer than {0} characters", MaxDataLength));
                d = value;
            }
        }
    }

    /// <summary>
    /// Represents a single product from the product catalog. Each item contains some common fields and some fields
    /// that are specific to a particular store.
    /// </summary>
    [Serializable]
    public class ProductCatalogItem
    {
        // Local configuration fields

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public string id;

        /// <summary>
        /// The <c>ProductType</c> of the item.
        /// </summary>
        public ProductType type;

        [SerializeField]
        private List<StoreID> storeIDs = new List<StoreID>();

        /// <summary>
        /// The default localized description of the product.
        /// </summary>
        public LocalizedProductDescription defaultDescription = new LocalizedProductDescription();

        // Apple configuration fields
        /// <summary>
        /// Screenshot path for Apple configuration.
        /// </summary>
        public string screenshotPath;

        /// <summary>
        /// The price tier for the Apple Store.
        /// </summary>
        public int applePriceTier = 0;

        // Google configuration fields
        /// <summary>
        /// The price for GooglePlay.
        /// </summary>
        public Price googlePrice = new Price();

        /// <summary>
        /// The price template for GooglePlay.
        /// </summary>
        public string pricingTemplateID;

        [SerializeField]
        private List<LocalizedProductDescription> descriptions = new List<LocalizedProductDescription>();

        // UDP configuration fields
        /// <summary>
        /// The price for UDP.
        /// </summary>
        public Price udpPrice = new Price();

        // Payouts
        [SerializeField]
        private List<ProductCatalogPayout> payouts = new List<ProductCatalogPayout>();

        /// <summary>
        /// Adds a new payout to the list.
        /// </summary>
        public void AddPayout()
        {
            payouts.Add(new ProductCatalogPayout());
        }

        /// <summary>
        /// Removes a payout to the list.
        /// </summary>
        /// <param name="payout"> The payout to be removed. </param>
        public void RemovePayout(ProductCatalogPayout payout)
        {
            payouts.Remove(payout);
        }

        /// <summary>
        /// Gets the list of payouts for this product.
        /// </summary>
        /// <returns> The list of payouts </returns>
        public IList<ProductCatalogPayout> Payouts {
            get {
                return payouts;
            }
        }

        /// <summary>
        /// Creates a copy of this object.
        /// </summary>
        /// <returns> A new instance of <c>ProductCatalogItem</c> identical to this. </returns>
        public ProductCatalogItem Clone()
        {
            ProductCatalogItem item = new ProductCatalogItem ();

            item.id = this.id;
            item.type = this.type;
            item.SetStoreIDs (this.allStoreIDs);
            item.defaultDescription = this.defaultDescription.Clone ();
            item.screenshotPath = this.screenshotPath;
            item.applePriceTier = this.applePriceTier;
            item.googlePrice.value = this.googlePrice.value;
            item.pricingTemplateID = this.pricingTemplateID;
            foreach (var desc in this.descriptions) {
                item.descriptions.Add (desc.Clone ());
            }

            return item;
        }

        /// <summary>
        /// Assigns or adds the a store for this item by name and id.
        /// </summary>
        /// <param name="aStore"> The name of the store. </param>
        /// <param name="aId">  The unique id of the store. </param>
        public void SetStoreID(string aStore, string aId)
        {
            storeIDs.RemoveAll((obj) => obj.store == aStore);
            if (!string.IsNullOrEmpty(aId))
                storeIDs.Add(new StoreID(aStore, aId));
        }

        /// <summary>
        /// Gets the store id by name.
        /// </summary>
        /// <param name="store"> The name of the store. </param>
        /// <returns> The id of the store if found, otherwise returns null. </returns>
        public string GetStoreID(string store)
        {
            StoreID sID = storeIDs.Find((obj) => obj.store == store);
            return sID == null ? null : sID.id;
        }

        /// <summary>
        /// Gets all of the <c>StoreIds</c> associated with this item.
        /// </summary>
        /// <returns> A collection of all store IDs for this item. </returns>
        public ICollection<StoreID> allStoreIDs {
            get {
                return storeIDs;
            }
        }

        /// <summary>
        /// Assigns or modifies a collection of <c>StoreID</c>s associated with this item.
        /// </summary>
        /// <param name="storeIds"> The set of <c>StoreID</c>s to assign or overwrite. </param>
        public void SetStoreIDs(ICollection<StoreID> storeIds) {
            foreach (var storeId in storeIds) {
                storeIDs.RemoveAll((obj) => obj.store == storeId.store);
                if (!string.IsNullOrEmpty(storeId.id))
                    storeIDs.Add(new StoreID(storeId.store, storeId.id));
            }
        }

        /// <summary>
        /// Gets the product description, localized to a specific locale.
        /// </summary>
        /// <param name="locale"> The locale of the description desired. </param>
        /// <returns> The localized description of this item. </returns>
        public LocalizedProductDescription GetDescription(TranslationLocale locale)
        {
            return descriptions.Find((obj) => obj.googleLocale == locale);
        }

        /// <summary>
        /// Gets the product description, localized to a specific locale, or adds a default one if it's not already set.
        /// </summary>
        /// <param name="locale"> The locale of the description desired. </param>
        /// <returns> The localized description of this item. </returns>
        public LocalizedProductDescription GetOrCreateDescription(TranslationLocale locale)
        {
            return GetDescription(locale) ?? AddDescription(locale);
        }

        /// <summary>
        /// Adds a default product description, localized to a specific locale.
        /// </summary>
        /// <param name="locale"> The locale of the description desired. </param>
        /// <returns> The localized description of this item. </returns>
        public LocalizedProductDescription AddDescription(TranslationLocale locale)
        {
            RemoveDescription(locale);
            var newDesc = new LocalizedProductDescription();
            newDesc.googleLocale = locale;
            descriptions.Add(newDesc);
            return newDesc;
        }

        /// <summary>
        /// Removes a product description, localized to a specific locale.
        /// </summary>
        /// <param name="locale"> The locale of the description desired. </param>
        public void RemoveDescription(TranslationLocale locale)
        {
            descriptions.RemoveAll((obj) => obj.googleLocale == locale);
        }

        /// <summary>
        /// Property that gets whether or not a valid locale is unassigned.
        /// </summary>
        /// <returns> Whether or not a new locale is avalable. </returns>
        public bool HasAvailableLocale
        {
            get {
                return Enum.GetValues(typeof(TranslationLocale)).Length > descriptions.Count + 1; // +1 for the default description
            }
        }

        /// <summary>
        /// Property that gets the next avalaible locale on the list.
        /// </summary>
        /// <returns> The next avalable locale. </returns>
        public TranslationLocale NextAvailableLocale
        {
            get {
                foreach (TranslationLocale l in Enum.GetValues(typeof(TranslationLocale)))
                {
                    if (GetDescription(l) == null && defaultDescription.googleLocale != l)
                    {
                        return l;
                    }
                }

                return TranslationLocale.en_US; // Not sure what to do if all locales have a description
            }
        }

        /// <summary>
        /// Property that gets the translated descriptions.
        /// </summary>
        /// <returns> A collection of all translated descriptions. </returns>
        public ICollection<LocalizedProductDescription> translatedDescriptions
        {
            get {
                return descriptions;
            }
        }
    }

    /// <summary>
    /// The product catalog represents a list of IAP products, with enough information about each product to do a batch
    /// export for Apple's Application Loader or the Google Play CSV import format. To retreive the standard catalog,
    /// use ProductCatalog.LoadDefaultCatalog().
    /// </summary>
    [Serializable]
    public class ProductCatalog
    {
        private static IProductCatalogImpl instance;

        /// <summary>
        /// The apple SKU of the app.
        /// </summary>
        public string appleSKU;

        /// <summary>
        /// The apple team ID of the app.
        /// </summary>
        public string appleTeamID;

        /// <summary>
        /// Enables automatic initialization when using Codeless IAP.
        /// </summary>
        public bool enableCodelessAutoInitialization = false;
        [SerializeField]
        private List<ProductCatalogItem> products = new List<ProductCatalogItem>();

        /// <summary>
        /// The collection of all products.
        /// </summary>
        public ICollection<ProductCatalogItem> allProducts => products;

        /// <summary>
        /// The collection of all valid products.
        /// </summary>
        public ICollection<ProductCatalogItem> allValidProducts
        {
            get {
                return products.Where(x => (!string.IsNullOrEmpty(x.id) && x.id.Trim().Length != 0 )).ToList();
            }
        }

        internal static void Initialize()
        {
            if (instance == null)
            {
                Initialize(new ProductCatalogImpl());
            }
        }

        /// <summary>
        /// Override the default catalog implementation.
        /// </summary>
        /// <param name="productCatalogImpl"></param>
        public static void Initialize(IProductCatalogImpl productCatalogImpl)
        {
            instance = productCatalogImpl;
        }

        /// <summary>
        /// Adds an item to the catalog.
        /// </summary>
        /// <param name="item"> The item to be added. </param>
        public void Add(ProductCatalogItem item)
        {
            products.Add(item);
        }

        /// <summary>
        /// Removes an item to the catalog.
        /// </summary>
        /// <param name="item"> The item to be removed. </param>
        public void Remove(ProductCatalogItem item)
        {
            products.Remove(item);
        }

        /// <summary>
        /// Check if the catalog is empty. A catalog is considered empty when it contains no products with valid IDs.
        /// </summary>
        /// <returns>A boolean representing whether or not the catalog is empty.</returns>
        public bool IsEmpty()
        {
            foreach (ProductCatalogItem item in products)
            {
                if (!String.IsNullOrEmpty(item.id))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// The path of the catalog file.
        /// </summary>
        public const string kCatalogPath = "Assets/Resources/IAPProductCatalog.json";

        /// <summary>
        /// The previous path of the catalog file used in older versions of Unity IAP.
        /// </summary>
        public const string kPrevCatalogPath = "Assets/Plugins/UnityPurchasing/Resources/IAPProductCatalog.json";


        /// <summary>
        /// Serializes the catalog to JSON.
        /// </summary>
        /// <param name="catalog"> The catalog. </param>
        /// <returns> The raw json string of the catalog data </returns>
        public static string Serialize(ProductCatalog catalog)
        {
            return JsonUtility.ToJson(catalog);
        }

        /// <summary>
        /// Deserializes the catalog from JSON.
        /// </summary>
        /// <param name="catalogJSON"> The raw json string of catalog data. </param>
        /// <returns> The deserialized Prodcut Catalog. </returns>
        public static ProductCatalog Deserialize(string catalogJSON)
        {
            return JsonUtility.FromJson<ProductCatalog>(catalogJSON);
        }

        /// <summary>
        /// Deserializes the catalog from a text asset.
        /// </summary>
        /// <param name="asset"> The text asset. </param>
        /// <returns> The deserialized Prodcut Catalog. </returns>
        public static ProductCatalog FromTextAsset(TextAsset asset)
        {
            return Deserialize(asset.text);
        }

        /// <summary>
        /// Loads the default catalog.
        /// </summary>
        /// <returns> The <c>ProductCatalog</c> instance. </returns>
        public static ProductCatalog LoadDefaultCatalog()
        {
            Initialize();

            return instance.LoadDefaultCatalog();
        }
    }

    /// <summary>
    /// For testing
    /// </summary>
    public interface IProductCatalogImpl
    {
        /// <summary>
        /// Loads the default catalog.
        /// </summary>
        /// <returns> The <c>ProductCatalog</c> instance. </returns>
        ProductCatalog LoadDefaultCatalog();
    }

    /// <summary>
    /// Implementation
    /// </summary>
    internal class ProductCatalogImpl : IProductCatalogImpl
    {
        /// <summary>
        /// Loads the default catalog.
        /// </summary>
        /// <returns> The <c>ProductCatalog</c> instance. </returns>
        public ProductCatalog LoadDefaultCatalog()
        {
            var asset = Resources.Load("IAPProductCatalog") as TextAsset;
            if (asset != null)
            {
                return ProductCatalog.FromTextAsset(asset);
            }
            else
            {
                return new ProductCatalog();
            }
        }
    }
}
