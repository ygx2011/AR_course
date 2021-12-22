using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Purchasing;
using Uniject;
using UnityEngine;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Default;

namespace UnityEngine.Purchasing
{
	/// <summary>
	/// Handles Windows 8.1.
	/// </summary>
	internal class WinRTStore : AbstractStore, IWindowsIAPCallback, IMicrosoftExtensions
    {
		private IWindowsIAP win8;
		private IStoreCallback callback;
		private IUtil util;
		private ILogger logger;

		private bool m_CanReceivePurchases = false;

		public WinRTStore(IWindowsIAP win8, IUtil util, ILogger logger)
        {
			this.win8 = win8;
			this.util = util;
			this.logger = logger;
		}

		/// <summary>
		/// Allow the windows IAP service to be swapped
		/// out since the Application developer can switch
		/// between sandbox/live after this store is
		/// constructed.
		/// </summary>
		public void SetWindowsIAP(IWindowsIAP iap)
        {
			this.win8 = iap;
		}

		public override void Initialize(IStoreCallback biller)
        {
			this.callback = biller;
		}

		public override void RetrieveProducts (ReadOnlyCollection<ProductDefinition> productDefs)
		{
			var dummyProducts = from def in productDefs
						   where def.type != ProductType.Subscription
						   select new WinProductDescription(
							   def.storeSpecificId, "$0.01",
							   "Fake title - " + def.storeSpecificId,
							   "Fake description - " + def.storeSpecificId,
							   "USD", 0.01m, null, null, def.type == ProductType.Consumable);
			win8.BuildDummyProducts(dummyProducts.ToList());
			init(0);
		}

		public override void FinishTransaction (ProductDefinition product, string transactionId)
		{
			this.win8.FinaliseTransaction(transactionId);
		}

		private void init(int delay)
		{
			win8.Initialize(this);
			win8.RetrieveProducts(true);
		}

		public override void Purchase(ProductDefinition product, string developerPayload)
        {
			win8.Purchase(product.storeSpecificId);
		}

		// An Action<bool> invoked on pause/resume.
		public void restoreTransactions(bool pausing)
        {
			if (!pausing) {
				if(m_CanReceivePurchases)
				{
					win8.RetrieveProducts(false);
				}
			}
		}

		public void RestoreTransactions()
        {
			win8.RetrieveProducts(false);
			// setting this here assumes that the Retrieve actually worked, but in the
			// case where it didn't we still want to persist that the user has tried to restore
			// to see if the automatic attempts (on app FG) resolve things
			m_CanReceivePurchases = true;
		}

		public void logError(string error)
        {
			// Uncomment to get diagnostics printed on screen.
			logger.LogError("Unity Purchasing", error);
		}

		public void OnProductListReceived(WinProductDescription[] winProducts)
        {
			util.RunOnMainThread(() =>
			{
				// Convert windows products to Unity Purchasing products.
				var products = from product in winProducts
					let metadata = new ProductMetadata(
						product.price, product.title, product.description,
						product.ISOCurrencyCode, product.priceDecimal)
					select new ProductDescription(
						product.platformSpecificID,
						metadata,
						product.receipt,
						product.transactionID);
				// need to determine if that list includes any purchases or just products
				// and then use that to set m_CanReceivePurchases
				callback.OnProductsRetrieved(products.ToList());
			});
		}

		public void log(string message) {
			util.RunOnMainThread(() => {
				logger.Log(message);
			});
		}
		
		public void OnPurchaseFailed(string productId, string error)
        {
			util.RunOnMainThread(() => {
				logger.LogFormat(LogType.Error, "Purchase failed: {0}, {1}", productId, error);
				if("AlreadyPurchased" == error)
				{
					try
					{
						callback.OnPurchaseFailed(new PurchaseFailureDescription(productId,
							(PurchaseFailureReason) Enum.Parse(typeof(PurchaseFailureReason), "DuplicateTransaction"), error));
					}
					catch
					{
						callback.OnPurchaseFailed(new PurchaseFailureDescription(productId,
							(PurchaseFailureReason) Enum.Parse(typeof(PurchaseFailureReason), "Unknown"), error));
					}
				}
				else if ("NotPurchased" == error)
				{
					callback.OnPurchaseFailed(new PurchaseFailureDescription(productId,
						(PurchaseFailureReason) Enum.Parse(typeof(PurchaseFailureReason), "UserCancelled"), error));
				}
				else
				{
					callback.OnPurchaseFailed(new PurchaseFailureDescription(productId,
						(PurchaseFailureReason) Enum.Parse(typeof(PurchaseFailureReason), "Unknown"), error));
				}
			});
		}

		private static int count;
		public void OnPurchaseSucceeded(string productId, string receipt, string tranId)
        {
			util.RunOnMainThread(() => {
				m_CanReceivePurchases = true;
				callback.OnPurchaseSucceeded(productId, receipt, tranId);
			});
		}

		// When using an incorrect product id:
		// "Exception from HRESULT: 0x805A0194"
		public void OnProductListError(string message)
        {
			util.RunOnMainThread(() => {
				if (message.Contains("801900CC"))
                {
					callback.OnSetupFailed(InitializationFailureReason.AppNotKnown);
				}
				else
                {
					logError("Unable to retrieve product listings. UnityIAP will automatically retry...");
					logError(message);
					init(3000);
				}
			});
		}
	}
}
