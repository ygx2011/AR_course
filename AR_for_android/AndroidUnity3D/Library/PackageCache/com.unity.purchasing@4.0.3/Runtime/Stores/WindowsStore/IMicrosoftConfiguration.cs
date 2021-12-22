using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Common interface for Universal Windows Platform configuration.
    /// </summary>
	public interface IMicrosoftConfiguration : IStoreConfiguration
	{
        /// <summary>
        /// Whether or not to use the Mock Billing system in UWP builds.
        /// If mock billing is used, the app can be tested before registering the app on the Windows Store.
        /// App releases should not be shipped with this flag set to true.
        /// </summary>
		bool useMockBillingSystem { get; set; }
	}
}
