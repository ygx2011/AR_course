using System;

namespace UnityEngine.Purchasing
{
	internal class FakeAppleConfiguation : IAppleConfiguration
	{
		public string appReceipt {
			get {
				return "This is a fake receipt. When running on an Apple store, a base64 encoded App Receipt would be returned";
			}
		}

		public bool canMakePayments {
			get {
				return true;
			}
		}

	    public void SetApplePromotionalPurchaseInterceptorCallback(Action<Product> callback)
	    {
	    }
	}
}

