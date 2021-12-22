using System;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// An interface to native underlying store systems. Provides a base for opaquely typed
    /// communication across a language-bridge upon which additional functionality can be composed.
    /// Is used by most public IStore implementations which themselves are owned by the purchasing
    /// core.
    /// </summary>
	public interface INativeStore
	{
		/// <summary>
		/// Call the Store to retrieve the store products. The `IStoreCallback` will be call with the retrieved products.
		/// </summary>
		/// <param name="json">The catalog of products to retrieve the store information from in JSON format.</param>
        void RetrieveProducts(String json);
		/// <summary>
		/// Call the Store to purchase a product. The `IStoreCallback` will be call when the purchase is successful.
		/// </summary>
		/// <param name="productJSON">The product to buy in JSON format.</param>
		/// <param name="developerPayload">A string used by some stores to fight fraudulent transactions.</param>
        void Purchase(string productJSON, string developerPayload);
		/// <summary>
		/// Call the Store to consume a product.
		/// </summary>
		/// <param name="productJSON">Product to consume in JSON format.</param>
		/// <param name="transactionID">The transaction id of the receipt to close.</param>
        void FinishTransaction(string productJSON, string transactionID);
	}

	internal delegate void UnityPurchasingCallback(string subject, string payload, string receipt, string transactionId);
}
