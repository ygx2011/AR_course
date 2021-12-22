using System;

namespace UnityEngine.Purchasing
{
	/// <summary>
	/// JSON based Native callback interface.
	/// </summary>
	internal interface IUnityCallback {
		void OnSetupFailed(String json);
		void OnProductsRetrieved(String json);
		void OnPurchaseSucceeded(String id, String receipt, String transactionID);
		void OnPurchaseFailed(String json);
	}
}
