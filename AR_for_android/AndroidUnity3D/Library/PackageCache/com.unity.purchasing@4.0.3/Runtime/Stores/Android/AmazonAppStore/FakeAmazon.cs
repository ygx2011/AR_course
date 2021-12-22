using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// THIS IS A FAKE, NO CODE WILL BE EXECUTED!
    ///
    /// Access Amazon store specific functionality.
    /// </summary>
    public class FakeAmazonExtensions : IAmazonExtensions, IAmazonConfiguration
	{
        /// <summary>
        /// THIS IS A FAKE, NO CODE WILL BE EXECUTED!
        ///
        /// To use for Amazon’s local Sandbox testing app, generate a JSON description of your product catalog on the device’s SD card.
        /// </summary>
        /// <param name="products">Products to add to the testing app JSON.</param>
        public void WriteSandboxJSON(HashSet<ProductDefinition> products)
		{
		}

        /// <summary>
        /// THIS IS A FAKE, NO CODE WILL BE EXECUTED!
        ///
        /// Amazon makes it possible to notify them of a product that cannot be fulfilled.
        ///
        /// This method calls Amazon's notifyFulfillment(transactionID, FulfillmentResult.UNAVAILABLE);
        /// https://developer.amazon.com/public/apis/earn/in-app-purchasing/docs-v2/implementing-iap-2.0
        /// </summary>
        /// <param name="transactionID">Products transaction id</param>
		public void NotifyUnableToFulfillUnavailableProduct(string transactionID) {
		}

        /// <summary>
        /// THIS IS A FAKE, NO CODE WILL BE EXECUTED!
        ///
        /// Gets the current Amazon user ID (for other Amazon services).
        /// </summary>
        public string amazonUserId
		{
			get { return "fakeid"; }
		}
	}
}
