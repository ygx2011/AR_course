using System.Collections.Generic;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
	/// <summary>
	/// Access Amazon store specific configurations.
	/// </summary>
    public interface IAmazonConfiguration : IStoreConfiguration
	{
		/// <summary>
		/// To use for Amazon’s local Sandbox testing app, generate a JSON description of your product catalog on the device’s SD card.
        /// </summary>
		/// <param name="products">Products to add to the testing app JSON.</param>
        void WriteSandboxJSON(HashSet<ProductDefinition> products);
	}
}
