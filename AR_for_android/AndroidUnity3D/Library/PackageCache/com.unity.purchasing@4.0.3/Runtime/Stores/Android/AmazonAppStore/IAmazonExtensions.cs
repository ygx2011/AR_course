using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Access Amazon store specific functionality.
    /// </summary>
    public interface IAmazonExtensions : IStoreExtension
	{
        /// <summary>
        /// Gets the current Amazon user ID (for other Amazon services).
        /// </summary>
		string amazonUserId { get; }

		/// <summary>
		/// Amazon makes it possible to notify them of a product that cannot be fulfilled.
		///
		/// This method calls Amazon's notifyFulfillment(transactionID, FulfillmentResult.UNAVAILABLE);
		/// https://developer.amazon.com/public/apis/earn/in-app-purchasing/docs-v2/implementing-iap-2.0
		/// </summary>
		/// <param name="transactionID">Products transaction id</param>
        void NotifyUnableToFulfillUnavailableProduct(string transactionID);
	}
}
