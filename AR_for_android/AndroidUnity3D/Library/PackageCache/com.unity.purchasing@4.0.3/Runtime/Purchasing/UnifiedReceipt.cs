using System;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// I can unpack JSON Product.receipt values.
    ///
    /// See also https://docs.unity3d.com/Manual/UnityIAPPurchaseReceipts.html
    /// <example>
    /// <code>
    /// var unifiedReceipt = JsonUtility.FromJson&lt;UnifiedReceipt&gt;(purchEvtArg.purchasedProduct.receipt)
    /// Debug.LogFormat("{0} {1} {2}", unifiedReceipt.Payload, unifiedReceipt.Store, unifiedReceipt.TransactionID);
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class UnifiedReceipt
    {
        /// <summary>
        /// The platform-specific receipt.
        /// </summary>
        public string Payload;

        /// <summary>
        /// The name of the store making the receipt.
        /// </summary>
        public string Store;

        /// <summary>
        /// The unique identifier of the transaction.
        /// </summary>
        public string TransactionID;
    }
}
