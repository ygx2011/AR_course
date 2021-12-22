using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UnityEngine.Purchasing.Default
{
    /// <summary>
    /// Interface for Universal Windows Platform purchasing callbacks.
    /// </summary>
    public interface IWindowsIAPCallback
    {
        /// <summary>
        /// Callback received when receiving the list of products.
        /// </summary>
        /// <param name="winProducts"> The products retrieved. </param>
        void OnProductListReceived(WinProductDescription[] winProducts);

        /// <summary>
        /// Callback received when receiving an error when attempting to get the list of products.
        /// </summary>
        /// <param name="message"> The error message explaining the failure. </param>
        void OnProductListError(string message);

        /// <summary>
        /// Callback received after making a successful purchase.
        /// </summary>
        /// <param name="productId"> The ID of the product purchased. </param>
        /// <param name="receipt"> The receipt of the purchase. </param>
        /// <param name="transactionId">  The ID of the transaction event. </param>
        void OnPurchaseSucceeded(string productId, string receipt, string transactionId);

        /// <summary>
        /// Callback received after making a failed purchase.
        /// </summary>
        /// <param name="productId"> The ID of the product purchased. </param>
        /// <param name="error"> The error explaining the failure.  </param>
        void OnPurchaseFailed(string productId, string error);

        /// <summary>
        /// Call used to log messsages during the callbacks in this interface.
        /// </summary>
        /// <param name="message"> The message to be logged. </param>
        void log(string message);

        /// <summary>
        /// Call used to log various errors during the callbacks in this interface.
        /// </summary>
        /// <param name="error"> The error message to be logged. </param>
        void logError(string error);
    }

    /// <summary>
    /// Interface for Universal Windows Platform purchasing calls.
    /// </summary>
    public interface IWindowsIAP
    {
        /// <summary>
        /// Builds a set of local products to be used as a proxy for what's on the Windows Store.
        /// </summary>
        /// <param name="products"> The products used. </param>
        void BuildDummyProducts(List<WinProductDescription> products);

        /// <summary>
        /// Initializes the Windows Store.
        /// </summary>
        /// <param name="callback"> The implementation of <c>IWindowsIAPCallback</c> used to handle events. </param>
        void Initialize(IWindowsIAPCallback callback);

        /// <summary>
        /// Retrieve products from the Windows Store.
        /// </summary>
        /// <param name="retryIfOffline"> Whether or not to retry the retrieval if it fails due to lack of an Internet connection. </param>
        void RetrieveProducts(bool retryIfOffline);

        /// <summary>
        /// Purchases a product.
        /// </summary>
        /// <param name="productId"> The ID product to be purchased. </param>
        void Purchase(string productId);

        /// <summary>
        /// Finalizes a transaction.
        /// </summary>
        /// <param name="transactionId"> The ID of transaction to be finalzed. </param>
        void FinaliseTransaction(string transactionId);
    }
}
