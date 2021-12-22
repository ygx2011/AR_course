using System;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using UnityEngine;

namespace UnityEngine.Purchasing {

    /// <summary>
    /// A period of time expressed in either days, months, or years. Conveys a subscription's duration definition.
    /// Note this reflects the types of subscription durations settable on a subscription on supported app stores.
    /// </summary>
    public class TimeSpanUnits {
        /// <summary>
        /// Discrete duration in days, if less than a month, otherwise zero.
        /// </summary>
        public double days;
        /// <summary>
        /// Discrete duration in months, if less than a year, otherwise zero.
        /// </summary>
        public int months;
        /// <summary>
        /// Discrete duration in years, otherwise zero.
        /// </summary>
        public int years;

        /// <summary>
        /// Construct a subscription duration.
        /// </summary>
        /// <param name="d">Discrete duration in days, if less than a month, otherwise zero.</param>
        /// <param name="m">Discrete duration in months, if less than a year, otherwise zero.</param>
        /// <param name="y">Discrete duration in years, otherwise zero.</param>
        public TimeSpanUnits (double d, int m, int y) {
            this.days = d;
            this.months = m;
            this.years = y;
        }
    }

    /// <summary>
    /// Use to query in-app purchasing subscription product information, and upgrade subscription products.
    /// Supports the Apple App Store, Google Play store, and Amazon AppStore.
    /// Note Amazon support offers no subscription duration information.
    /// Note expiration dates may become invalid after updating subscriptions between two types of duration.
    /// </summary>
    /// <seealso cref="IAppleExtensions.GetIntroductoryPriceDictionary"/>
    /// <seealso cref="UpdateSubscription"/>
    public class SubscriptionManager {

        private string receipt;
        private string productId;
        private string intro_json;

        /// <summary>
        /// Performs subscription updating, migrating a subscription into another as long as they are both members
        /// of the same subscription group on the App Store.
        /// </summary>
        /// <param name="newProduct">Destination subscription product, belonging to the same subscription group as <paramref name="oldProduct"/></param>
        /// <param name="oldProduct">Source subscription product, belonging to the same subscription group as <paramref name="newProduct"/></param>
        /// <param name="developerPayload">Carried-over metadata from prior call to <typeparamref name="SubscriptionManager.UpdateSubscription"/> </param>
        /// <param name="appleStore">Triggered upon completion of the subscription update.</param>
        /// <param name="googleStore">Triggered upon completion of the subscription update.</param>
        public static void UpdateSubscription(Product newProduct, Product oldProduct, string developerPayload, Action<Product, string> appleStore, Action<string, string> googleStore) {
            if (oldProduct.receipt == null) {
                Debug.LogError("The product has not been purchased, a subscription can only be upgrade/downgrade when has already been purchased");
                return;
            }
            var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(oldProduct.receipt);
            if (receipt_wrapper == null || !receipt_wrapper.ContainsKey("Store") || !receipt_wrapper.ContainsKey("Payload")) {
                Debug.LogWarning("The product receipt does not contain enough information");
                return;
            }
            var store = (string)receipt_wrapper ["Store"];
            var payload = (string)receipt_wrapper ["Payload"];

            if (payload != null ) {
                switch (store) {
                case "GooglePlay":
                    {
                        SubscriptionManager oldSubscriptionManager = new SubscriptionManager(oldProduct, null);
                        SubscriptionInfo oldSubscriptionInfo = null;
                        try {
                            oldSubscriptionInfo = oldSubscriptionManager.getSubscriptionInfo();
                        } catch (Exception e) {
                            Debug.unityLogger.LogError("Error: the product that will be updated does not have a valid receipt", e);
                            return;
                        }
                        string newSubscriptionId = newProduct.definition.storeSpecificId;
                        googleStore(oldSubscriptionInfo.getSubscriptionInfoJsonString(), newSubscriptionId);
                        return;
                    }
                case "AppleAppStore":
                case "MacAppStore":
                    {
                        appleStore(newProduct, developerPayload);
                        return;
                    }
                default:
                    {
                        Debug.LogWarning("This store does not support update subscriptions");
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Performs subscription updating, migrating a subscription into another as long as they are both members
        /// of the same subscription group on the App Store.
        /// </summary>
        /// <param name="oldProduct">Source subscription product, belonging to the same subscription group as <paramref name="newProduct"/></param>
        /// <param name="newProduct">Destination subscription product, belonging to the same subscription group as <paramref name="oldProduct"/></param>
        /// <param name="googlePlayUpdateCallback">Triggered upon completion of the subscription update.</param>
        public static void UpdateSubscriptionInGooglePlayStore(Product oldProduct, Product newProduct, Action<string, string> googlePlayUpdateCallback) {
            SubscriptionManager oldSubscriptionManager = new SubscriptionManager(oldProduct, null);
            SubscriptionInfo oldSubscriptionInfo = null;
            try {
                oldSubscriptionInfo = oldSubscriptionManager.getSubscriptionInfo();
            } catch (Exception e) {
                Debug.unityLogger.LogError("Error: the product that will be updated does not have a valid receipt", e);
                return;
            }
            string newSubscriptionId = newProduct.definition.storeSpecificId;
            googlePlayUpdateCallback(oldSubscriptionInfo.getSubscriptionInfoJsonString(), newSubscriptionId);
        }

        /// <summary>
        /// Performs subscription updating, migrating a subscription into another as long as they are both members
        /// of the same subscription group on the App Store.
        /// </summary>
        /// <param name="newProduct">Destination subscription product, belonging to the same subscription group as <paramref name="oldProduct"/></param>
        /// <param name="developerPayload">Carried-over metadata from prior call to <typeparamref name="SubscriptionManager.UpdateSubscription"/> </param>
        /// <param name="appleStoreUpdateCallback">Triggered upon completion of the subscription update.</param>
        public static void UpdateSubscriptionInAppleStore(Product newProduct, string developerPayload, Action<Product, string> appleStoreUpdateCallback) {
            appleStoreUpdateCallback(newProduct, developerPayload);
        }

        /// <summary>
        /// Construct an object that allows inspection of a subscription product.
        /// </summary>
        /// <param name="product">Subscription to be inspected</param>
        /// <param name="intro_json">From <typeparamref name="IAppleExtensions.GetIntroductoryPriceDictionary"/></param>
        public SubscriptionManager(Product product, string intro_json) {
            this.receipt = product.receipt;
            this.productId = product.definition.storeSpecificId;
            this.intro_json = intro_json;
        }

        /// <summary>
        /// Construct an object that allows inspection of a subscription product.
        /// </summary>
        /// <param name="receipt">A Unity IAP unified receipt from <typeparamref name="Product.receipt"/></param>
        /// <param name="id">A product identifier.</param>
        /// <param name="intro_json">From <typeparamref name="IAppleExtensions.GetIntroductoryPriceDictionary"/></param>
        public SubscriptionManager(string receipt, string id, string intro_json) {
            this.receipt = receipt;
            this.productId = id;
            this.intro_json = intro_json;
        }

        /// <summary>
        /// Convert my Product into a <typeparamref name="SubscriptionInfo"/>.
        /// My Product.receipt must have a "Payload" JSON key containing supported native app store
        /// information, which will be converted here.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullProductIdException">My Product must have a non-null product identifier</exception>
        /// <exception cref="StoreSubscriptionInfoNotSupportedException">A supported app store must be used as my product</exception>
        /// <exception cref="NullReceiptException">My product must have</exception>
        public SubscriptionInfo getSubscriptionInfo() {

            if (this.receipt != null) {
                var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);

                var validPayload = receipt_wrapper.TryGetValue("Payload", out var payloadAsObject);
                var validStore  = receipt_wrapper.TryGetValue("Store", out var storeAsObject);

                if (validPayload && validStore)
                {

                    var payload = payloadAsObject as string;
                    var store = storeAsObject as string;

                    switch (store) {
                    case GooglePlay.Name:
                        {
                            return getGooglePlayStoreSubInfo(payload);
                        }
                    case AppleAppStore.Name:
                    case MacAppStore.Name:
                        {
                            if (this.productId == null) {
                                throw new NullProductIdException();
                            }
                            return getAppleAppStoreSubInfo(payload, this.productId);
                        }
                    case AmazonApps.Name:
                        {
                            return getAmazonAppStoreSubInfo(this.productId);
                        }
                    default:
                        {
                            throw new StoreSubscriptionInfoNotSupportedException("Store not supported: " + store);
                        }
                    }
                }
            }

            throw new NullReceiptException();

        }

        private SubscriptionInfo getAmazonAppStoreSubInfo(string productId) {
            return new SubscriptionInfo(productId);
        }
        private SubscriptionInfo getAppleAppStoreSubInfo(string payload, string productId) {

            AppleReceipt receipt = null;

            var logger = UnityEngine.Debug.unityLogger;

            try {
                receipt = new AppleReceiptParser().Parse(Convert.FromBase64String(payload));
            } catch (ArgumentException e) {
                logger.Log ("Unable to parse Apple receipt", e);
            } catch (Security.IAPSecurityException e) {
                logger.Log ("Unable to parse Apple receipt", e);
            } catch (NullReferenceException e) {
                logger.Log ("Unable to parse Apple receipt", e);
            }

            List<AppleInAppPurchaseReceipt> inAppPurchaseReceipts = new List<AppleInAppPurchaseReceipt>();

            if (receipt != null && receipt.inAppPurchaseReceipts != null && receipt.inAppPurchaseReceipts.Length > 0) {
                foreach (AppleInAppPurchaseReceipt r in receipt.inAppPurchaseReceipts) {
                    if (r.productID.Equals(productId)) {
                        inAppPurchaseReceipts.Add(r);
                    }
                }
            }
            return inAppPurchaseReceipts.Count == 0 ? null : new SubscriptionInfo(findMostRecentReceipt(inAppPurchaseReceipts), this.intro_json);
        }

        private AppleInAppPurchaseReceipt findMostRecentReceipt(List<AppleInAppPurchaseReceipt> receipts) {
            receipts.Sort((b, a) => (a.purchaseDate.CompareTo(b.purchaseDate)));
            return receipts[0];
        }

        private SubscriptionInfo getGooglePlayStoreSubInfo(string payload)
        {
            var payload_wrapper = (Dictionary<string, object>) MiniJson.JsonDecode(payload);
            var validSkuDetailsKey = payload_wrapper.TryGetValue("skuDetails", out var skuDetailsObject);

            string skuDetails = null;
            if (validSkuDetailsKey) skuDetails = skuDetailsObject as string;

            var purchaseHistorySupported = false;

            var original_json_payload_wrapper =
                (Dictionary<string, object>) MiniJson.JsonDecode((string) payload_wrapper["json"]);

            var validIsAutoRenewingKey =
                original_json_payload_wrapper.TryGetValue("autoRenewing", out var autoRenewingObject);

            var isAutoRenewing = false;
            if (validIsAutoRenewingKey) isAutoRenewing = (bool) autoRenewingObject;

            // Google specifies times in milliseconds since 1970.
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var validPurchaseTimeKey =
                original_json_payload_wrapper.TryGetValue("purchaseTime", out var purchaseTimeObject);

            long purchaseTime = 0;

            if (validPurchaseTimeKey) purchaseTime = (long) purchaseTimeObject;

            var purchaseDate = epoch.AddMilliseconds(purchaseTime);

            var validDeveloperPayloadKey =
                original_json_payload_wrapper.TryGetValue("developerPayload", out var developerPayloadObject);

            var isFreeTrial = false;
            var hasIntroductoryPrice = false;
            string updateMetadata = null;

            if (validDeveloperPayloadKey)
            {
                var developerPayloadJSON = (string) developerPayloadObject;
                var developerPayload_wrapper = (Dictionary<string, object>) MiniJson.JsonDecode(developerPayloadJSON);
                var validIsFreeTrialKey =
                    developerPayload_wrapper.TryGetValue("is_free_trial", out var isFreeTrialObject);
                if (validIsFreeTrialKey) isFreeTrial = (bool) isFreeTrialObject;

                var validHasIntroductoryPriceKey =
                    developerPayload_wrapper.TryGetValue("has_introductory_price_trial",
                        out var hasIntroductoryPriceObject);

                if (validHasIntroductoryPriceKey) hasIntroductoryPrice = (bool) hasIntroductoryPriceObject;

                var validIsUpdatedKey = developerPayload_wrapper.TryGetValue("is_updated", out var isUpdatedObject);

                var isUpdated = false;

                if (validIsUpdatedKey) isUpdated = (bool) isUpdatedObject;

                if (isUpdated)
                {
                    var isValidUpdateMetaKey = developerPayload_wrapper.TryGetValue("update_subscription_metadata",
                        out var updateMetadataObject);

                    if (isValidUpdateMetaKey) updateMetadata = (string) updateMetadataObject;
                }
            }

            return new SubscriptionInfo(skuDetails, isAutoRenewing, purchaseDate, isFreeTrial, hasIntroductoryPrice,
                purchaseHistorySupported, updateMetadata);
        }


    }

    /// <summary>
    /// A container for a Product’s subscription-related information.
    /// </summary>
    /// <seealso cref="SubscriptionManager.getSubscriptionInfo"/>
    public class SubscriptionInfo {
        private Result is_subscribed;
        private Result is_expired;
        private Result is_cancelled;
        private Result is_free_trial;
        private Result is_auto_renewing;
        private Result is_introductory_price_period;
        private string productId;
        private DateTime purchaseDate;
        private DateTime subscriptionExpireDate;
        private DateTime subscriptionCancelDate;
        private TimeSpan remainedTime;
        private string introductory_price;
        private TimeSpan introductory_price_period;
        private long introductory_price_cycles;

        private TimeSpan freeTrialPeriod;
        private TimeSpan subscriptionPeriod;

        // for test
        private string free_trial_period_string;
        private string sku_details;

        /// <summary>
        /// Unpack Apple receipt subscription data.
        /// </summary>
        /// <param name="r">The Apple receipt from <typeparamref name="CrossPlatformValidator"/></param>
        /// <param name="intro_json">From <typeparamref name="IAppleExtensions.GetIntroductoryPriceDictionary"/>. Keys:
        /// <c>introductoryPriceLocale</c>, <c>introductoryPrice</c>, <c>introductoryPriceNumberOfPeriods</c>, <c>numberOfUnits</c>,
        /// <c>unit</c>, which can be fetched from Apple's remote service.</param>
        /// <exception cref="InvalidProductTypeException">Error found involving an invalid product type.</exception>
        /// <see cref="UnityEngine.Purchasing.Security.CrossPlatformValidator"/>
        public SubscriptionInfo(AppleInAppPurchaseReceipt r, string intro_json) {

            var productType = (AppleStoreProductType) Enum.Parse(typeof(AppleStoreProductType), r.productType.ToString());

            if (productType == AppleStoreProductType.Consumable || productType == AppleStoreProductType.NonConsumable) {
                throw new InvalidProductTypeException();
            }

            if (!string.IsNullOrEmpty(intro_json)) {
                var intro_wrapper = (Dictionary<string, object>) MiniJson.JsonDecode(intro_json);
                var nunit = -1;
                var unit = SubscriptionPeriodUnit.NotAvailable;
                this.introductory_price = intro_wrapper.TryGetString("introductoryPrice") + intro_wrapper.TryGetString("introductoryPriceLocale");
                if (string.IsNullOrEmpty(this.introductory_price)) {
                    this.introductory_price = "not available";
                } else {
                    try {
                        this.introductory_price_cycles = Convert.ToInt64(intro_wrapper.TryGetString("introductoryPriceNumberOfPeriods"));
                        nunit = Convert.ToInt32(intro_wrapper.TryGetString("numberOfUnits"));
                        unit = (SubscriptionPeriodUnit)Convert.ToInt32(intro_wrapper.TryGetString("unit"));
                    } catch(Exception e) {
                        Debug.unityLogger.Log ("Unable to parse introductory period cycles and duration, this product does not have configuration of introductory price period", e);
                        unit = SubscriptionPeriodUnit.NotAvailable;
                    }
                }
                DateTime now = DateTime.Now;
                switch (unit) {
                    case SubscriptionPeriodUnit.Day:
                        this.introductory_price_period = TimeSpan.FromTicks(TimeSpan.FromDays(1).Ticks * nunit);
                        break;
                    case SubscriptionPeriodUnit.Month:
                        TimeSpan month_span = now.AddMonths(1) - now;
                        this.introductory_price_period = TimeSpan.FromTicks(month_span.Ticks * nunit);
                        break;
                    case SubscriptionPeriodUnit.Week:
                        this.introductory_price_period = TimeSpan.FromTicks(TimeSpan.FromDays(7).Ticks * nunit);
                        break;
                    case SubscriptionPeriodUnit.Year:
                        TimeSpan year_span = now.AddYears(1) - now;
                        this.introductory_price_period = TimeSpan.FromTicks(year_span.Ticks * nunit);
                        break;
                    case SubscriptionPeriodUnit.NotAvailable:
                        this.introductory_price_period = TimeSpan.Zero;
                        this.introductory_price_cycles = 0;
                        break;
                }
            } else {
                this.introductory_price = "not available";
                this.introductory_price_period = TimeSpan.Zero;
                this.introductory_price_cycles = 0;
            }

            DateTime current_date = DateTime.UtcNow;
            this.purchaseDate = r.purchaseDate;
            this.productId = r.productID;

            this.subscriptionExpireDate = r.subscriptionExpirationDate;
            this.subscriptionCancelDate = r.cancellationDate;

            // if the product is non-renewing subscription, apple store will not return expiration date for this product
            if (productType == AppleStoreProductType.NonRenewingSubscription) {
                this.is_subscribed = Result.Unsupported;
                this.is_expired = Result.Unsupported;
                this.is_cancelled = Result.Unsupported;
                this.is_free_trial = Result.Unsupported;
                this.is_auto_renewing = Result.Unsupported;
                this.is_introductory_price_period = Result.Unsupported;
            } else {
                this.is_cancelled = (r.cancellationDate.Ticks > 0) && (r.cancellationDate.Ticks < current_date.Ticks) ? Result.True : Result.False;
                this.is_subscribed = r.subscriptionExpirationDate.Ticks >= current_date.Ticks ? Result.True : Result.False;
                this.is_expired = (r.subscriptionExpirationDate.Ticks > 0 && r.subscriptionExpirationDate.Ticks < current_date.Ticks) ? Result.True : Result.False;
                this.is_free_trial = (r.isFreeTrial == 1) ? Result.True : Result.False;
                this.is_auto_renewing = ((productType == AppleStoreProductType.AutoRenewingSubscription) && this.is_cancelled == Result.False
                        && this.is_expired == Result.False) ? Result.True : Result.False;
                this.is_introductory_price_period = r.isIntroductoryPricePeriod == 1 ? Result.True : Result.False;
            }

            if (this.is_subscribed == Result.True) {
                this.remainedTime = r.subscriptionExpirationDate.Subtract(current_date);
            } else {
                this.remainedTime = TimeSpan.Zero;
            }


        }

        /// <summary>
        /// Especially crucial values relating to Google subscription products.
        /// Note this is intended to be called internally.
        /// </summary>
        /// <param name="skuDetails">The raw JSON from <c>SkuDetail.getOriginalJson</c></param>
        /// <param name="isAutoRenewing">Whether this subscription is expected to auto-renew</param>
        /// <param name="purchaseDate">A date this subscription was billed</param>
        /// <param name="isFreeTrial">Indicates whether this Product is a free trial</param>
        /// <param name="hasIntroductoryPriceTrial">Indicates whether this Product may be owned with an introductory price period.</param>
        /// <param name="purchaseHistorySupported">Unsupported</param>
        /// <param name="updateMetadata">Unsupported. Mechanism previously propagated subscription upgrade information to new subscription. </param>
        /// <exception cref="InvalidProductTypeException">For non-subscription product types. </exception>
        public SubscriptionInfo(string skuDetails, bool isAutoRenewing, DateTime purchaseDate, bool isFreeTrial,
                bool hasIntroductoryPriceTrial, bool purchaseHistorySupported, string updateMetadata) {

            var skuDetails_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(skuDetails);
            var validTypeKey = skuDetails_wrapper.TryGetValue("type", out var typeObject);

            if (!validTypeKey || (string)typeObject == "inapp") {
                throw new InvalidProductTypeException();
            }

            var validProductIdKey = skuDetails_wrapper.TryGetValue("productId", out var productIdObject);
            productId = null;
            if (validProductIdKey) productId = productIdObject as string;

            this.purchaseDate = purchaseDate;
            this.is_subscribed = Result.True;
            this.is_auto_renewing = isAutoRenewing ? Result.True : Result.False;
            this.is_expired = Result.False;
            this.is_cancelled = isAutoRenewing ? Result.False : Result.True;
            this.is_free_trial = Result.False;


            string sub_period = null;
            if (skuDetails_wrapper.ContainsKey("subscriptionPeriod")) {
                sub_period = (string)skuDetails_wrapper["subscriptionPeriod"];
            }
            string free_trial_period = null;
            if (skuDetails_wrapper.ContainsKey("freeTrialPeriod")) {
                free_trial_period = (string)skuDetails_wrapper["freeTrialPeriod"];
            }
            string introductory_price = null;
            if (skuDetails_wrapper.ContainsKey("introductoryPrice")) {
                introductory_price = (string)skuDetails_wrapper["introductoryPrice"];
            }
            string introductory_price_period_string = null;
            if (skuDetails_wrapper.ContainsKey("introductoryPricePeriod")) {
                introductory_price_period_string = (string)skuDetails_wrapper["introductoryPricePeriod"];
            }
            long introductory_price_cycles = 0;
            if (skuDetails_wrapper.ContainsKey("introductoryPriceCycles")) {
                introductory_price_cycles = (long)skuDetails_wrapper["introductoryPriceCycles"];
            }

            // for test
            free_trial_period_string = free_trial_period;

            this.subscriptionPeriod = computePeriodTimeSpan(parsePeriodTimeSpanUnits(sub_period));

            this.freeTrialPeriod = TimeSpan.Zero;
            if (isFreeTrial) {
                this.freeTrialPeriod = parseTimeSpan(free_trial_period);
            }

            this.introductory_price = introductory_price;
            this.introductory_price_cycles = introductory_price_cycles;
            this.introductory_price_period = TimeSpan.Zero;
            this.is_introductory_price_period = Result.False;
            TimeSpan total_introductory_duration = TimeSpan.Zero;

            if (hasIntroductoryPriceTrial) {
                if (introductory_price_period_string != null && introductory_price_period_string.Equals(sub_period)) {
                    this.introductory_price_period = this.subscriptionPeriod;
                } else {
                    this.introductory_price_period = parseTimeSpan(introductory_price_period_string);
                }
                // compute the total introductory duration according to the introductory price period and period cycles
                total_introductory_duration = accumulateIntroductoryDuration(parsePeriodTimeSpanUnits(introductory_price_period_string), this.introductory_price_cycles);
            }

            // if this subscription is updated from other subscription, the remaining time will be applied to this subscription
            TimeSpan extra_time = TimeSpan.FromSeconds(updateMetadata == null ? 0.0 : computeExtraTime(updateMetadata, this.subscriptionPeriod.TotalSeconds));

            TimeSpan time_since_purchased = DateTime.UtcNow.Subtract(purchaseDate);


            // this subscription is still in the extra time (the time left by the previous subscription when updated to the current one)
            if (time_since_purchased <= extra_time) {
                // this subscription is in the remaining credits from the previous updated one
                this.subscriptionExpireDate = purchaseDate.Add(extra_time);
            } else if (time_since_purchased <= this.freeTrialPeriod.Add(extra_time)) {
                // this subscription is in the free trial period
                // this product will be valid until free trial ends, the beginning of next billing date
                this.is_free_trial = Result.True;
                this.subscriptionExpireDate = purchaseDate.Add(this.freeTrialPeriod.Add(extra_time));
            } else if (time_since_purchased < this.freeTrialPeriod.Add(extra_time).Add(total_introductory_duration)) {
                // this subscription is in the introductory price period
                this.is_introductory_price_period = Result.True;
                DateTime introductory_price_begin_date = this.purchaseDate.Add(this.freeTrialPeriod.Add(extra_time));
                this.subscriptionExpireDate = nextBillingDate(introductory_price_begin_date, parsePeriodTimeSpanUnits(introductory_price_period_string));
            } else {
                // no matter sub is cancelled or not, the expire date will be next billing date
                DateTime billing_begin_date = this.purchaseDate.Add(this.freeTrialPeriod.Add(extra_time).Add(total_introductory_duration));
                this.subscriptionExpireDate = nextBillingDate(billing_begin_date, parsePeriodTimeSpanUnits(sub_period));
            }

            this.remainedTime = this.subscriptionExpireDate.Subtract(DateTime.UtcNow);
            this.sku_details = skuDetails;
        }

        /// <summary>
        /// Especially crucial values relating to subscription products.
        /// Note this is intended to be called internally.
        /// </summary>
        /// <param name="productId">This subscription's product identifier</param>
        public SubscriptionInfo(string productId) {
            this.productId = productId;
            this.is_subscribed = Result.True;
            this.is_expired = Result.False;
            this.is_cancelled = Result.Unsupported;
            this.is_free_trial = Result.Unsupported;
            this.is_auto_renewing = Result.Unsupported;
            this.remainedTime = TimeSpan.MaxValue;
            this.is_introductory_price_period = Result.Unsupported;
            this.introductory_price_period = TimeSpan.MaxValue;
            this.introductory_price = null;
            this.introductory_price_cycles = 0;
        }

        /// <summary>
        /// Store specific product identifier.
        /// </summary>
        /// <returns>The product identifier from the store receipt.</returns>
        public string getProductId() { return this.productId; }

        /// <summary>
        /// A date this subscription was billed.
        /// Note the store-specific behavior.
        /// </summary>
        /// <returns>
        /// For Apple, the purchase date is the date when the subscription was either purchased or renewed.
        /// For Google, the purchase date is the date when the subscription was originally purchased.
        /// </returns>
        public DateTime getPurchaseDate() { return this.purchaseDate; }

        /// <summary>
        /// Indicates whether this auto-renewable subscription Product is currently subscribed or not.
        /// Note the store-specific behavior.
        /// Note also that the receipt may update and change this subscription expiration status if the user sends
        /// their iOS app to the background and then returns it to the foreground. It is therefore recommended to remember
        /// subscription expiration state at app-launch, and ignore the fact that a subscription may expire later during
        /// this app launch runtime session.
        /// </summary>
        /// <returns>
        /// <typeparamref name="Result.True"/> Subscription status if the store receipt's expiration date is
        /// after the device's current time.
        /// <typeparamref name="Result.False"/> otherwise.
        /// Non-renewable subscriptions in the Apple store return a <typeparamref name="Result.Unsupported"/> value.
        /// </returns>
        /// <seealso cref="isExpired"/>
        /// <seealso cref="DateTime.UtcNow"/>
        public Result isSubscribed() { return this.is_subscribed; }

        /// <summary>
        /// Indicates whether this auto-renewable subscription Product is currently unsubscribed or not.
        /// Note the store-specific behavior.
        /// Note also that the receipt may update and change this subscription expiration status if the user sends
        /// their iOS app to the background and then returns it to the foreground. It is therefore recommended to remember
        /// subscription expiration state at app-launch, and ignore the fact that a subscription may expire later during
        /// this app launch runtime session.
        /// </summary>
        /// <returns>
        /// <typeparamref name="Result.True"/> Subscription status if the store receipt's expiration date is
        /// before the device's current time.
        /// <typeparamref name="Result.False"/> otherwise.
        /// Non-renewable subscriptions in the Apple store return a <typeparamref name="Result.Unsupported"/> value.
        /// </returns>
        /// <seealso cref="isSubscribed"/>
        /// <seealso cref="DateTime.UtcNow"/>
        public Result isExpired() { return this.is_expired; }

        /// <summary>
        /// Indicates whether this Product has been cancelled.
        /// A cancelled subscription means the Product is currently subscribed, and will not renew on the next billing date.
        /// </summary>
        /// <returns>
        /// <typeparamref name="Result.True"/> Cancellation status if the store receipt's indicates this subscription is cancelled.
        /// <typeparamref name="Result.False"/> otherwise.
        /// Non-renewable subscriptions in the Apple store return a <typeparamref name="Result.Unsupported"/> value.
        /// </returns>
        public Result isCancelled() { return this.is_cancelled; }

        /// <summary>
        /// Indicates whether this Product is a free trial.
        /// Note the store-specific behavior.
        /// </summary>
        /// <returns>
        /// <typeparamref name="Result.True"/> This subscription is a free trial according to the store receipt.
        /// <typeparamref name="Result.False"/> This subscription is not a free trial according to the store receipt.
        /// Non-renewable subscriptions in the Apple store
        /// and Google subscriptions queried on devices with version lower than 6 of the Android in-app billing API return a <typeparamref name="Result.Unsupported"/> value.
        /// </returns>
        public Result isFreeTrial() { return this.is_free_trial; }

        /// <summary>
        /// Indicates whether this Product is expected to auto-renew. The product must be auto-renewable, not canceled, and not expired.
        /// </summary>
        /// <returns>
        /// <typeparamref name="Result.True"/> The store receipt's indicates this subscription is auto-renewing.
        /// <typeparamref name="Result.False"/> The store receipt's indicates this subscription is not auto-renewing.
        /// Non-renewable subscriptions in the Apple store return a <typeparamref name="Result.Unsupported"/> value.
        /// </returns>
        public Result isAutoRenewing() { return this.is_auto_renewing; }

        /// <summary>
        /// Indicates how much time remains until the next billing date.
        /// Note the store-specific behavior.
        /// Note also that the receipt may update and change this subscription expiration status if the user sends
        /// their iOS app to the background and then returns it to the foreground.
        /// </summary>
        /// <returns>
        /// A time duration from now until subscription billing occurs.
        /// Google subscriptions queried on devices with version lower than 6 of the Android in-app billing API return <typeparamref name="TimeSpan.MaxValue"/>.
        /// </returns>
        /// <seealso cref="DateTime.UtcNow"/>
        public TimeSpan getRemainingTime() { return this.remainedTime; }

        /// <summary>
        /// Indicates whether this Product is currently owned within an introductory price period.
        /// Note the store-specific behavior.
        /// </summary>
        /// <returns>
        /// <typeparamref name="Result.True"/> The store receipt's indicates this subscription is within its introductory price period.
        /// <typeparamref name="Result.False"/> The store receipt's indicates this subscription is not within its introductory price period.
        /// <typeparamref name="Result.False"/> If the product is not configured to have an introductory period.
        /// Non-renewable subscriptions in the Apple store return a <typeparamref name="Result.Unsupported"/> value.
        /// Google subscriptions queried on devices with version lower than 6 of the Android in-app billing API return a <typeparamref name="Result.Unsupported"/> value.
        /// </returns>
        public Result isIntroductoryPricePeriod() { return this.is_introductory_price_period; }

        /// <summary>
        /// Indicates how much time remains for the introductory price period.
        /// Note the store-specific behavior.
        /// </summary>
        /// <returns>
        /// Duration remaining in this product's introductory price period.
        /// Subscription products with no introductory price period return <typeparamref name="TimeSpan.Zero"/>.
        /// Products in the Apple store return <typeparamref name="TimeSpan.Zero"/> if the application does
        /// not support iOS version 11.2+, macOS 10.13.2+, or tvOS 11.2+.
        /// <typeparamref name="TimeSpan.Zero"/> returned also for products which do not have an introductory period configured.
        /// </returns>
        public TimeSpan getIntroductoryPricePeriod() { return this.introductory_price_period; }

        /// <summary>
        /// For subscriptions with an introductory price, get this price.
        /// Note the store-specific behavior.
        /// </summary>
        /// <returns>
        /// For subscriptions with a introductory price, a localized price string.
        /// For Google store the price may not include the currency symbol (e.g. $) and the currency code is available in <typeparamref name="ProductMetadata.isoCurrencyCode"/>.
        /// For all other product configurations, the string <c>"not available"</c>.
        /// </returns>
        /// <seealso cref="ProductMetadata.isoCurrencyCode"/>
        public string getIntroductoryPrice() { return string.IsNullOrEmpty(this.introductory_price) ? "not available" : this.introductory_price; }

        /// <summary>
        /// Indicates the number of introductory price billing periods that can be applied to this subscription Product.
        /// Note the store-specific behavior.
        /// </summary>
        /// <returns>
        /// Products in the Apple store return <c>0</c> if the application does not support iOS version 11.2+, macOS 10.13.2+, or tvOS 11.2+.
        /// <c>0</c> returned also for products which do not have an introductory period configured.
        /// </returns>
        /// <seealso cref="intro"/>
        public long getIntroductoryPricePeriodCycles() { return this.introductory_price_cycles; }

        /// <summary>
        /// When this auto-renewable receipt expires.
        /// </summary>
        /// <returns>
        /// An absolute date when this receipt will expire.
        /// </returns>
        public DateTime getExpireDate() { return this.subscriptionExpireDate; }

        /// <summary>
        /// When this auto-renewable receipt was canceled.
        /// Note the store-specific behavior.
        /// </summary>
        /// <returns>
        /// For Apple store, the date when this receipt was canceled.
        /// For other stores this will be <c>null</c>.
        /// </returns>
        public DateTime getCancelDate() { return this.subscriptionCancelDate; }

        /// <summary>
        /// The period duration of the free trial for this subscription, if enabled.
        /// Note the store-specific behavior.
        /// </summary>
        /// <returns>
        /// For Google Play store if the product is configured with a free trial, this will be the period duration.
        /// For Apple store this will be <c> null </c>.
        /// </returns>
        public TimeSpan getFreeTrialPeriod() { return this.freeTrialPeriod; }

        /// <summary>
        /// The duration of this subscription.
        /// Note the store-specific behavior.
        /// </summary>
        /// <returns>
        /// A duration this subscription is valid for.
        /// <typeparamref name="TimeSpan.Zero"/> returned for Apple products.
        /// </returns>
        public TimeSpan getSubscriptionPeriod() { return this.subscriptionPeriod; }

        /// <summary>
        /// The string representation of the period in ISO8601 format this subscription is free for.
        /// Note the store-specific behavior.
        /// </summary>
        /// <returns>
        /// For Google Play store on configured subscription this will be the period which the can own this product for free, unless
        /// the user is ineligible for this free trial.
        /// For Apple store this will be <c> null </c>.
        /// </returns>
        public string getFreeTrialPeriodString() { return this.free_trial_period_string; }

        /// <summary>
        /// The raw JSON SkuDetails from the underlying Google API.
        /// Note the store-specific behavior.
        /// Note this is not supported.
        /// </summary>
        /// <returns>
        /// For Google store the <c> SkuDetails#getOriginalJson </c> results.
        /// For Apple this returns <c>null</c>.
        /// </returns>
        public string getSkuDetails() { return this.sku_details; }

        /// <summary>
        /// A JSON including a collection of data involving free-trial and introductory prices.
        /// Note the store-specific behavior.
        /// Used internally for subscription updating on Google store.
        /// </summary>
        /// <returns>
        /// A JSON with keys: <c>productId</c>, <c>is_free_trial</c>, <c>is_introductory_price_period</c>, <c>remaining_time_in_seconds</c>.
        /// </returns>
        /// <seealso cref="SubscriptionManager.UpdateSubscription"/>
        public string getSubscriptionInfoJsonString() {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("productId", this.productId);
            dict.Add("is_free_trial", this.is_free_trial);
            dict.Add("is_introductory_price_period", this.is_introductory_price_period == Result.True);
            dict.Add("remaining_time_in_seconds", this.remainedTime.TotalSeconds);
            return MiniJson.JsonEncode(dict);
        }

        private DateTime nextBillingDate(DateTime billing_begin_date, TimeSpanUnits units) {

            if (units.days == 0.0 && units.months == 0 && units.years == 0) return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            DateTime next_billing_date = billing_begin_date;
            // find the next billing date that after the current date
            while (DateTime.Compare(next_billing_date, DateTime.UtcNow) <= 0) {

                next_billing_date = next_billing_date.AddDays(units.days).AddMonths(units.months).AddYears(units.years);
            }
            return next_billing_date;
        }

        private TimeSpan accumulateIntroductoryDuration(TimeSpanUnits units, long cycles) {
            TimeSpan result = TimeSpan.Zero;
            for (long i = 0; i < cycles; i++) {
                result = result.Add(computePeriodTimeSpan(units));
            }
            return result;
        }

        private TimeSpan computePeriodTimeSpan(TimeSpanUnits units) {
            DateTime now = DateTime.Now;
            return now.AddDays(units.days).AddMonths(units.months).AddYears(units.years).Subtract(now);
        }


        private double computeExtraTime(string metadata, double new_sku_period_in_seconds) {
            var wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(metadata);
            long old_sku_remaining_seconds = (long)wrapper["old_sku_remaining_seconds"];
            long old_sku_price_in_micros = (long)wrapper["old_sku_price_in_micros"];

            double old_sku_period_in_seconds = (parseTimeSpan((string)wrapper["old_sku_period_string"])).TotalSeconds;
            long new_sku_price_in_micros = (long)wrapper["new_sku_price_in_micros"];
            double result = ((((double)old_sku_remaining_seconds / (double)old_sku_period_in_seconds ) * (double)old_sku_price_in_micros) / (double)new_sku_price_in_micros) * new_sku_period_in_seconds;
            return result;
        }

        private TimeSpan parseTimeSpan(string period_string) {
            TimeSpan result = TimeSpan.Zero;
            try {
                result = XmlConvert.ToTimeSpan(period_string);
            } catch(Exception) {
                if (period_string == null || period_string.Length == 0) {
                    result = TimeSpan.Zero;
                } else {
                    // .Net "P1W" is not supported and throws a FormatException
                    // not sure if only weekly billing contains "W"
                    // need more testing
                    result = new TimeSpan(7, 0, 0, 0);
                }
            }
            return result;
        }

        private TimeSpanUnits parsePeriodTimeSpanUnits(string time_span) {
            switch (time_span) {
            case "P1W":
                // weekly subscription
                return new TimeSpanUnits(7.0, 0, 0);
            case "P1M":
                // monthly subscription
                return new TimeSpanUnits(0.0, 1, 0);
            case "P3M":
                // 3 months subscription
                return new TimeSpanUnits(0.0, 3, 0);
            case "P6M":
                // 6 months subscription
                return new TimeSpanUnits(0.0, 6, 0);
            case "P1Y":
                // yearly subscription
                return new TimeSpanUnits(0.0, 0, 1);
            default:
                // seasonal subscription or duration in days
                return new TimeSpanUnits((double)parseTimeSpan(time_span).Days, 0, 0);
            }
        }


    }


    /// <summary>
    /// For representing boolean values which may also be not available.
    /// </summary>
    public enum Result {
        /// <summary>
        /// Corresponds to boolean <c> true </c>.
        /// </summary>
        True,
        /// <summary>
        /// Corresponds to boolean <c> false </c>.
        /// </summary>
        False,
        /// <summary>
        /// Corresponds to no value, such as for situations where no result is available.
        /// </summary>
        Unsupported,
    };

    /// <summary>
    /// Used internally to parse Apple receipts. Corresponds to Apple SKProductPeriodUnit.
    /// </summary>
    /// <see cref="https://developer.apple.com/documentation/storekit/skproductperiodunit?language=objc"/>
    public enum SubscriptionPeriodUnit {
        /// <summary>
        /// An interval lasting one day.
        /// </summary>
        Day = 0,
        /// <summary>
        /// An interval lasting one month.
        /// </summary>
        Month = 1,
        /// <summary>
        /// An interval lasting one week.
        /// </summary>
        Week = 2,
        /// <summary>
        /// An interval lasting one year.
        /// </summary>
        Year = 3,
        /// <summary>
        /// Default value when no value is available.
        /// </summary>
        NotAvailable = 4,
    };

    enum AppleStoreProductType {
        NonConsumable = 0,
        Consumable = 1,
        NonRenewingSubscription = 2,
        AutoRenewingSubscription = 3,
    };

    /// <summary>
    /// Error found during receipt parsing.
    /// </summary>
    public class ReceiptParserException : System.Exception {
        /// <summary>
        /// Construct an error object for receipt parsing.
        /// </summary>
        public ReceiptParserException() { }

        /// <summary>
        /// Construct an error object for receipt parsing.
        /// </summary>
        /// <param name="message">Description of error</param>
        public ReceiptParserException(string message) : base(message) { }
    }

    /// <summary>
    /// An error was found when an invalid <typeparamref name="Product.definition.type"/> is provided.
    /// </summary>
    public class InvalidProductTypeException : ReceiptParserException {}

    /// <summary>
    /// An error was found when an unexpectedly null <typeparamref name="Product.definition.id"/> is provided.
    /// </summary>
    public class NullProductIdException : ReceiptParserException {}

    /// <summary>
    /// An error was found when an unexpectedly null <typeparamref name="Product.receipt"/> is provided.
    /// </summary>
    public class NullReceiptException : ReceiptParserException {}

    /// <summary>
    /// An error was found when an unsupported app store <typeparamref name="Product.receipt"/> is provided.
    /// </summary>
    public class StoreSubscriptionInfoNotSupportedException : ReceiptParserException {
        /// <summary>
        /// An error was found when an unsupported app store <typeparamref name="Product.receipt"/> is provided.
        /// </summary>
        /// <param name="message">Human readable explanation of this error</param>
        public StoreSubscriptionInfoNotSupportedException (string message) : base (message) {
        }
    }
}
