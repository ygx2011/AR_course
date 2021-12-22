using System;
using UnityEngine;

namespace UnityEngine.Purchasing
{
	/// <summary>
	/// Receives callbacks from Android based stores.
	/// </summary>
	internal class JavaBridge : AndroidJavaProxy, IUnityCallback
	{
		private IUnityCallback forwardTo;
		public JavaBridge (IUnityCallback forwardTo) : base("com.unity.purchasing.common.IUnityCallback")
		{
			this.forwardTo = forwardTo;
		}

		public JavaBridge (IUnityCallback forwardTo, string javaInterface) : base(javaInterface)
		{
			this.forwardTo = forwardTo;
		}

		public void OnSetupFailed(String json) {
			forwardTo.OnSetupFailed (json);
		}

		public void OnProductsRetrieved(String json) {
			forwardTo.OnProductsRetrieved (json);
		}

		public void OnPurchaseSucceeded(String id, String receipt, String transactionID) {
			forwardTo.OnPurchaseSucceeded (id, receipt, transactionID);
		}

		public void OnPurchaseFailed(String json) {
			forwardTo.OnPurchaseFailed (json);
		}
	}
}
