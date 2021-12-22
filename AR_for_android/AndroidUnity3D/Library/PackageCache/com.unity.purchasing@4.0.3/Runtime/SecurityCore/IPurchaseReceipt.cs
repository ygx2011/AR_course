using System;

namespace UnityEngine.Purchasing.Security
{
	/// <summary>
	/// Represents a parsed purchase receipt from a store.
	/// </summary>
	public interface IPurchaseReceipt
	{
        /// <summary>
        /// The ID of the transaction.
        /// </summary>
		string transactionID { get; }

        /// <summary>
        /// The ID of the product purchased.
        /// </summary>
		string productID { get; }

        /// <summary>
        /// The date fof the purchase.
        /// </summary>
		DateTime purchaseDate { get; }
	}
}
