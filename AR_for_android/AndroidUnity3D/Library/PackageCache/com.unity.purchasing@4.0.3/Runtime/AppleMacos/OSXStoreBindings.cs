using System.Runtime.InteropServices;

#if !UNITY_EDITOR
namespace UnityEngine.Purchasing
{
    internal class OSXStoreBindings : INativeAppleStore
    {
        [DllImport("unitypurchasing")]
        private static extern bool unityPurchasingRetrieveProducts(string json);

        [DllImport("unitypurchasing")]
        private static extern bool unityPurchasingPurchase(string json, string developerPayload);

        [DllImport("unitypurchasing")]
        private static extern bool unityPurchasingFinishTransaction(string productJSON, string transactionId);

        [DllImport("unitypurchasing")]
        private static extern void unityPurchasingRestoreTransactions();

        [DllImport("unitypurchasing")]
        private static extern void unityPurchasingRefreshAppReceipt();

        [DllImport("unitypurchasing")]
        private static extern void unityPurchasingAddTransactionObserver();

        [DllImport ("unitypurchasing")]
        private static extern void unityPurchasingSetApplicationUsername (string username);

        [DllImport("unitypurchasing")]
        private static extern void setUnityPurchasingCallback (UnityPurchasingCallback AsyncCallback);

        [DllImport("unitypurchasing")]
        private static extern string getUnityPurchasingAppReceipt ();

        [DllImport("unitypurchasing")]
        private static extern string getUnityPurchasingTransactionReceiptForProductId (string productId);

        [DllImport("unitypurchasing")]
        private static extern bool getUnityPurchasingCanMakePayments ();

        [DllImport ("unitypurchasing")]
        private static extern void setSimulateAskToBuy (bool enabled);

        [DllImport ("unitypurchasing")]
        private static extern bool getSimulateAskToBuy ();

        [DllImport("unitypurchasing")]
        private static extern void unityPurchasingUpdateStorePromotionOrder(string json);

        [DllImport("unitypurchasing")]
        private static extern void unityPurchasingUpdateStorePromotionVisibility(string productId, string visibility);

        [DllImport("unitypurchasing")]
        private static extern void unityPurchasingInterceptPromotionalPurchases ();

        [DllImport("unitypurchasing")]
        private static extern void unityPurchasingContinuePromotionalPurchases ();

        [DllImport("unitypurchasing")]
        private static extern void unityPurchasingPresentCodeRedemptionSheet();

        public void SetUnityPurchasingCallback (UnityPurchasingCallback AsyncCallback)
        {
            setUnityPurchasingCallback (AsyncCallback);
        }

        public string appReceipt {
            get {
                return getUnityPurchasingAppReceipt ();
            }
        }

        public bool canMakePayments {
            get {
                return getUnityPurchasingCanMakePayments ();
            }
        }

        public bool simulateAskToBuy {
            get {
                return getSimulateAskToBuy ();
            }
            set {
                setSimulateAskToBuy (value);
            }
        }

        public void RetrieveProducts (string json)
        {
            unityPurchasingRetrieveProducts (json);
        }

        public void Purchase (string productJSON, string developerPayload)
        {
            unityPurchasingPurchase (productJSON, developerPayload);
        }

        public void FinishTransaction (string productJSON, string transactionId)
        {
            unityPurchasingFinishTransaction (productJSON, transactionId);
        }

        public void RestoreTransactions ()
        {
            unityPurchasingRestoreTransactions();
        }

        public void RefreshAppReceipt ()
        {
            unityPurchasingRefreshAppReceipt();
        }

        public void AddTransactionObserver ()
        {
            unityPurchasingAddTransactionObserver ();
        }

        public void SetApplicationUsername (string applicationUsername)
        {
            unityPurchasingSetApplicationUsername (applicationUsername);
        }

        public void SetStorePromotionOrder(string json)
        {
            unityPurchasingUpdateStorePromotionOrder(json);
        }

        public void SetStorePromotionVisibility(string productId, string visibility)
        {
            unityPurchasingUpdateStorePromotionVisibility(productId, visibility);
        }

        public string GetTransactionReceiptForProductId (string productId)
        {
            return getUnityPurchasingTransactionReceiptForProductId (productId);
        }

        public void InterceptPromotionalPurchases ()
        {
            unityPurchasingInterceptPromotionalPurchases ();
        }

        public void ContinuePromotionalPurchases ()
        {
            unityPurchasingContinuePromotionalPurchases ();
        }

        public void PresentCodeRedemptionSheet()
        {
            unityPurchasingPresentCodeRedemptionSheet();
        }
    }
}
#endif
