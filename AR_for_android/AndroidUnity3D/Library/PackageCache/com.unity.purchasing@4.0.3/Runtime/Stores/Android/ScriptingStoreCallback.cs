using System;
using System.Collections.Generic;
using Uniject;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
	/// <summary>
	/// Wraps an IStoreCallback executing methods on
	/// the scripting thread.
	/// </summary>
	internal class ScriptingStoreCallback : IStoreCallback
	{
        IStoreCallback m_ForwardTo;
        IUtil m_Util;

		public ScriptingStoreCallback(IStoreCallback forwardTo, IUtil util) {
			m_ForwardTo = forwardTo;
			m_Util = util;
		}

        public ProductCollection products => m_ForwardTo.products;

        public void OnSetupFailed(InitializationFailureReason reason)
        {
            m_Util.RunOnMainThread(() => m_ForwardTo.OnSetupFailed(reason));
        }

        public void OnProductsRetrieved(List<ProductDescription> products)
        {
            m_Util.RunOnMainThread(() => m_ForwardTo.OnProductsRetrieved(products));
        }

        public void OnPurchaseSucceeded (string id, string receipt, string transactionID)
		{
			m_Util.RunOnMainThread (() => m_ForwardTo.OnPurchaseSucceeded (id, receipt, transactionID));
		}

        public void OnAllPurchasesRetrieved(List<Product> purchasedProducts)
        {
            m_Util.RunOnMainThread(() => m_ForwardTo.OnAllPurchasesRetrieved(purchasedProducts));
        }

        public void OnPurchaseFailed(PurchaseFailureDescription desc)
        {
            m_Util.RunOnMainThread(() => m_ForwardTo.OnPurchaseFailed(desc));
        }

        public bool useTransactionLog { get; set; }
    }
}

