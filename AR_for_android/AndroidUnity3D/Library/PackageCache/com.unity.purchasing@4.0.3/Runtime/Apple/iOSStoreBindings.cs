using System.Runtime.InteropServices;

#if !UNITY_EDITOR
namespace UnityEngine.Purchasing
{
    internal class iOSStoreBindings : INativeAppleStore
    {
        [DllImport("__Internal")]
        private static extern void unityPurchasingRetrieveProducts(string json);

        [DllImport("__Internal")]
        private static extern void unityPurchasingPurchase(string json, string developerPayload);

        [DllImport("__Internal")]
        private static extern void unityPurchasingFinishTransaction(string productJSON, string transactionId);

        [DllImport("__Internal")]
        private static extern void unityPurchasingRestoreTransactions();

        [DllImport("__Internal")]
        private static extern void unityPurchasingRefreshAppReceipt();

        [DllImport("__Internal")]
        private static extern void unityPurchasingAddTransactionObserver();

        [DllImport ("__Internal")]
        private static extern void unityPurchasingSetApplicationUsername(string username);

        [DllImport("__Internal")]
        private static extern void setUnityPurchasingCallback (UnityPurchasingCallback AsyncCallback);

        [DllImport("__Internal")]
        private static extern string getUnityPurchasingAppReceipt ();

        [DllImport("__Internal")]
        private static extern string getUnityPurchasingTransactionReceiptForProductId (string productId);

        [DllImport("__Internal")]
        private static extern bool getUnityPurchasingCanMakePayments ();

        [DllImport ("__Internal")]
        private static extern void setSimulateAskToBuy (bool enabled);

        [DllImport ("__Internal")]
        private static extern bool getSimulateAskToBuy ();

        [DllImport("__Internal")]
        private static extern void unityPurchasingUpdateStorePromotionOrder(string json);

        [DllImport("__Internal")]
        private static extern void unityPurchasingUpdateStorePromotionVisibility(string productId, string visibility);

        [DllImport("__Internal")]
        private static extern void unityPurchasingInterceptPromotionalPurchases ();

        [DllImport("__Internal")]
        private static extern void unityPurchasingContinuePromotionalPurchases ();

        [DllImport("__Internal")]
        private static extern void unityPurchasingPresentCodeRedemptionSheet ();

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
            unityPurchasingSetApplicationUsername(applicationUsername);
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
