using System;
using UnityEngine.Purchasing;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Common interface for Universal Windows Platform purchasing extensions.
    /// </summary>
	public interface IMicrosoftExtensions : IStoreExtension
	{
        /// <summary>
        /// Restores previously purchased transactions.
        /// </summary>
		void RestoreTransactions();
	}
}
