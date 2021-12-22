using System;
using System.Collections.Generic;
using Uniject;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;
using AOT;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// App Store implementation of <see cref="IStore"/>.
    /// </summary>
    internal class AppleStoreImpl : JSONStore, IAppleExtensions, IAppleConfiguration
    {
        private Action<Product> m_DeferredCallback;
        private Action m_RefreshReceiptError;
        private Action<string> m_RefreshReceiptSuccess;
        private Action<bool> m_RestoreCallback;
        private Action<Product> m_PromotionalPurchaseCallback;
        private INativeAppleStore m_Native;

        private static IUtil util;
        private static AppleStoreImpl instance;


        private string products_json;


        public AppleStoreImpl(IUtil util) {
            AppleStoreImpl.util = util;
            instance = this;
        }

        public void SetNativeStore(INativeAppleStore apple) {
            base.SetNativeStore (apple);
            this.m_Native = apple;
            apple.SetUnityPurchasingCallback (MessageCallback);
        }

        public string appReceipt {
            get {
                return m_Native.appReceipt;
            }
        }

        public bool canMakePayments {
            get {
                return m_Native.canMakePayments;
            }
        }

        public void SetApplePromotionalPurchaseInterceptorCallback(Action<Product> callback)
        {
            m_PromotionalPurchaseCallback = callback;
        }

        public bool simulateAskToBuy {
            get {
                return m_Native.simulateAskToBuy;
            }
            set {
                m_Native.simulateAskToBuy = value;
            }
        }

        public void SetStorePromotionOrder(List<Product> products)
        {
            // Encode product list as a json doc containing an array of store-specific ids:
            // { "products": [ "ssid1", "ssid2" ] }
            var productIds = new List<string>();
            foreach (var p in products)
            {
                if (p != null && !string.IsNullOrEmpty(p.definition.storeSpecificId))
                    productIds.Add(p.definition.storeSpecificId);
            }
            var dict = new Dictionary<string, object>{ { "products", productIds } };
            m_Native.SetStorePromotionOrder(MiniJson.JsonEncode(dict));
        }

        public void SetStorePromotionVisibility(Product product, AppleStorePromotionVisibility visibility)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            m_Native.SetStorePromotionVisibility(product.definition.storeSpecificId, visibility.ToString());
        }

        public string GetTransactionReceiptForProduct (Product product) {
            return m_Native.GetTransactionReceiptForProductId (product.definition.storeSpecificId);
        }

        public void SetApplicationUsername (string applicationUsername)
        {
            m_Native.SetApplicationUsername (applicationUsername);
        }

        public override void OnProductsRetrieved (string json)
        {
            // base.OnProductsRetrieved (json); // Don't call this, because we want to enrich the products first

            // get product list
            var productDescriptions = JSONSerializer.DeserializeProductDescriptions (json);
            List<ProductDescription> finalProductDescriptions = null;

            this.products_json = json;

            // parse app receipt
            if (m_Native != null) {
                var base64AppReceipt = m_Native.appReceipt;
                if (!string.IsNullOrEmpty (base64AppReceipt)) {
                    AppleReceipt appleReceipt = getAppleReceiptFromBase64String(base64AppReceipt);
                    if (appleReceipt != null
                        && appleReceipt.inAppPurchaseReceipts != null
                        && appleReceipt.inAppPurchaseReceipts.Length > 0) {
                        // Enrich the product descriptions with parsed receipt data
                        finalProductDescriptions = new List<ProductDescription> ();
                        foreach (var productDescription in productDescriptions) {
                            // JDRjr this Find may not be sufficient for subsciptions (or even multiple non-consumables?)
                            var foundReceipts = Array.FindAll (appleReceipt.inAppPurchaseReceipts, (r) => r.productID == productDescription.storeSpecificId);
                            if (foundReceipts == null || foundReceipts.Length == 0) {
                                finalProductDescriptions.Add (productDescription);
                            } else {
                                Array.Sort(foundReceipts, (b, a) => (a.purchaseDate.CompareTo(b.purchaseDate)));
                                var mostRecentReceipt = foundReceipts[0];
                                var productType = (AppleStoreProductType) Enum.Parse(typeof(AppleStoreProductType), mostRecentReceipt.productType.ToString());
                                if (productType == AppleStoreProductType.AutoRenewingSubscription) {
                                    // if the product is auto-renewing subscription, filter the expired products
                                    if (new SubscriptionInfo(mostRecentReceipt, null).isExpired() == Result.True) {
                                        finalProductDescriptions.Add (productDescription);
                                    } else {
                                        finalProductDescriptions.Add (
                                                new ProductDescription (
                                                    productDescription.storeSpecificId,
                                                    productDescription.metadata,
                                                    base64AppReceipt,
                                                    mostRecentReceipt.transactionID));
                                    }
                                } else if (productType == AppleStoreProductType.Consumable) {
                                    finalProductDescriptions.Add (productDescription);
                                } else {
                                    finalProductDescriptions.Add (
                                            new ProductDescription (
                                                productDescription.storeSpecificId,
                                                productDescription.metadata,
                                                base64AppReceipt,
                                                mostRecentReceipt.transactionID));
                                }
                            }
                        }
                    }
                }


            }

            // Pass along the enriched product descriptions
            unity.OnProductsRetrieved (finalProductDescriptions ?? productDescriptions);

            // If there is a promotional purchase callback, tell the store to intercept those purchases.
            if (m_PromotionalPurchaseCallback != null)
            {
                m_Native.InterceptPromotionalPurchases();
            }

            // Indicate we are ready to start receiving payments.
            m_Native.AddTransactionObserver ();
        }

        public void RestoreTransactions(Action<bool> callback)
        {
            m_RestoreCallback = callback;
            m_Native.RestoreTransactions ();
        }

        public void RefreshAppReceipt(Action<string> successCallback, Action errorCallback)
        {
            m_RefreshReceiptSuccess = successCallback;
            m_RefreshReceiptError = errorCallback;
            m_Native.RefreshAppReceipt ();
        }

        public void RegisterPurchaseDeferredListener(Action<Product> callback)
        {
            m_DeferredCallback = callback;
        }

        public void ContinuePromotionalPurchases()
        {
            m_Native.ContinuePromotionalPurchases ();
        }

        public Dictionary<string, string> GetIntroductoryPriceDictionary() {
            return JSONSerializer.DeserializeSubscriptionDescriptions(this.products_json);
        }

        public Dictionary<string, string> GetProductDetails() {
            return JSONSerializer.DeserializeProductDetails(this.products_json);
        }

        public void PresentCodeRedemptionSheet()
        {
            m_Native.PresentCodeRedemptionSheet();
        }

        public void OnPurchaseDeferred(string productId)
        {
            if (null != m_DeferredCallback)
            {
                var product = unity.products.WithStoreSpecificID(productId);
                if (null != product)
                    m_DeferredCallback(product);
            }
        }

        public void OnPromotionalPurchaseAttempted(string productId)
        {
            if (null != m_PromotionalPurchaseCallback)
            {
                var product = unity.products.WithStoreSpecificID(productId);
                if (null != product)
                {
                    m_PromotionalPurchaseCallback(product);
                }
            }
        }

        public void OnTransactionsRestoredSuccess()
        {
            if (null != m_RestoreCallback)
                m_RestoreCallback(true);
        }

        public void OnTransactionsRestoredFail(string error)
        {
            if (null != m_RestoreCallback)
                m_RestoreCallback(false);
        }

        public void OnAppReceiptRetrieved(string receipt)
        {
            if (receipt != null)
            {
                if (null != m_RefreshReceiptSuccess)
                    m_RefreshReceiptSuccess(receipt);
            }
        }

        public void OnAppReceiptRefreshedFailed()
        {
            if (null != m_RefreshReceiptError)
                m_RefreshReceiptError();
        }


        [MonoPInvokeCallback(typeof(UnityPurchasingCallback))]
        private static void MessageCallback(string subject, string payload, string receipt, string transactionId) {
            util.RunOnMainThread(() => {
                instance.ProcessMessage (subject, payload, receipt, transactionId);
            });
        }

        private void ProcessMessage(string subject, string payload, string receipt, string transactionId) {
            switch (subject) {
            case "OnSetupFailed":
                OnSetupFailed (payload);
                break;
            case "OnProductsRetrieved":
                OnProductsRetrieved (payload);
                break;
            case "OnPurchaseSucceeded":
                OnPurchaseSucceeded (payload, receipt, transactionId);
                break;
            case "OnPurchaseFailed":
                OnPurchaseFailed (payload);
                break;
            case "onProductPurchaseDeferred":
                OnPurchaseDeferred (payload);
                break;
            case "onPromotionalPurchaseAttempted":
                OnPromotionalPurchaseAttempted (payload);
                break;
            case "onTransactionsRestoredSuccess":
                OnTransactionsRestoredSuccess ();
                break;
            case "onTransactionsRestoredFail":
                OnTransactionsRestoredFail (payload);
                break;
            case "onAppReceiptRefreshed":
                OnAppReceiptRetrieved (payload);
                break;
            case "onAppReceiptRefreshFailed":
                OnAppReceiptRefreshedFailed ();
                break;
            }
        }

        public override void OnPurchaseSucceeded (string id, string receipt, string transactionId) {
            if (isValidPurchaseState(getAppleReceiptFromBase64String(receipt), id)) {
                base.OnPurchaseSucceeded(id, receipt, transactionId);
            } else {
                base.FinishTransaction(null, transactionId);
            }
        }

        internal AppleReceipt getAppleReceiptFromBase64String(string receipt) {
            AppleReceipt appleReceipt = null;
            if (!string.IsNullOrEmpty(receipt)) {
                var parser = new AppleReceiptParser ();
                try {
                    appleReceipt = parser.Parse (Convert.FromBase64String (receipt));
                } catch (Exception) {
                }
            }
            return appleReceipt;
        }

        internal bool isValidPurchaseState(AppleReceipt appleReceipt, string id) {
            var isValid = true;
            if (appleReceipt != null
                    && appleReceipt.inAppPurchaseReceipts != null
                    && appleReceipt.inAppPurchaseReceipts.Length > 0) {
                var foundReceipts = Array.FindAll(appleReceipt.inAppPurchaseReceipts, (r) => r.productID == id);
                if (foundReceipts != null && foundReceipts.Length > 0) {
                    Array.Sort(foundReceipts, (b, a) => (a.purchaseDate.CompareTo(b.purchaseDate)));
                    var mostRecentReceipt = foundReceipts[0];
                    var productType = (AppleStoreProductType) Enum.Parse(typeof(AppleStoreProductType), mostRecentReceipt.productType.ToString());
                    if (productType == AppleStoreProductType.AutoRenewingSubscription) {
                        // if the product is auto-renewing subscription, check if this transaction is expired
                        if (new SubscriptionInfo(mostRecentReceipt, null).isExpired() == Result.True) {
                            isValid = false;
                        }
                    }
                }
            }
            return isValid;
        }

    }
}
