using System;
using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// IAP transaction history and debugging extension.
    /// </summary>
    public interface ITransactionHistoryExtensions : IStoreExtension
    {
        /// <summary>
        /// Get the description of the last received purchase failure.
        /// </summary>
        /// <returns> The <c>PurchaseFailureDescription</c> associated with the last failed purchase. </returns>
        PurchaseFailureDescription GetLastPurchaseFailureDescription();

        /// <summary>
        /// Gets the store-specfic error code of the last received purchase failure.
        /// </summary>
        /// <returns> The <c>StoreSpecificPurchaseErrorCode</c> associated with the last failed purchase. </returns>
        StoreSpecificPurchaseErrorCode GetLastStoreSpecificPurchaseErrorCode();
    }
}
