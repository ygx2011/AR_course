using System;

namespace UnityEngine.Purchasing
{
    internal class iOSStoreBindings : INativeAppleStore
    {
        public void SetUnityPurchasingCallback (UnityPurchasingCallback AsyncCallback)
        {
            throw new NotImplementedException ();
        }
        public void RestoreTransactions ()
        {
            throw new NotImplementedException ();
        }
        public void RefreshAppReceipt ()
        {
            throw new NotImplementedException ();
        }
        public void AddTransactionObserver ()
        {
            throw new NotImplementedException ();
        }
        public void SetApplicationUsername (string applicationUsername)
        {
            throw new NotImplementedException ();
        }
        public void RetrieveProducts (string json)
        {
            throw new NotImplementedException ();
        }
        public void Purchase (string productJSON, string developerPayload)
        {
            throw new NotImplementedException ();
        }
        public void FinishTransaction (string productJSON, string transactionID)
        {
            throw new NotImplementedException ();
        }

        public string appReceipt {
            get {
                throw new NotImplementedException ();
            }
        }

        public bool canMakePayments {
            get {
                throw new NotImplementedException ();
            }
        }

        public bool simulateAskToBuy {
            get {
                throw new NotImplementedException ();
            }
            set {
                throw new NotImplementedException ();
            }
        }

        public void SetStorePromotionOrder(string json)
        {
            throw new NotImplementedException();
        }

        public void SetStorePromotionVisibility(string productId, string visibility)
        {
            throw new NotImplementedException();
        }

        public string GetTransactionReceiptForProductId (string productId)
        {
            throw new NotImplementedException();
        }

        public void InterceptPromotionalPurchases ()
        {
            throw new NotImplementedException();
        }

        public void ContinuePromotionalPurchases ()
        {
            throw new NotImplementedException();
        }

        public void PresentCodeRedemptionSheet()
        {
            throw new NotImplementedException();
        }
    }
}
