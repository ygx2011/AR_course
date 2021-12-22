using System;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
	/// <summary>
    /// Access Apple store specific configurations.
	/// </summary>
    public interface IAppleConfiguration : IStoreConfiguration
	{
        /// <summary>
        /// Read the App Receipt from local storage.
        /// Returns null for iOS less than or equal to 6, may also be null on a reinstalling and require refreshing.
        /// </summary>
        string appReceipt { get; }

		/// <summary>
		/// Determine if the user can make payments; [SKPaymentQueue canMakePayments].
		/// </summary>
		bool canMakePayments { get; }

	    /// <summary>
	    /// Stores a callback that will be called when
	    /// the user attempts a promotional purchase
	    /// (directly from the Apple App Store) on
	    /// iOS or tvOS.
	    ///
	    /// If the callback is set, you must call
	    /// IAppleExtensions.ContinuePromotionalPurchases()
	    /// inside it in order to continue the intercepted
	    /// purchase(s).
	    /// </summary>
	    /// <param name="callback"></param>
	    void SetApplePromotionalPurchaseInterceptorCallback(Action<Product> callback);
	}
}

