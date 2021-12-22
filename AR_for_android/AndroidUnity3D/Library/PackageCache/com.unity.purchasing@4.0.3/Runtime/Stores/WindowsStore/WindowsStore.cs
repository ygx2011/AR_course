using System;
namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Class containing store information for Universal Windows Platform builds.
    /// </summary>
	public class WindowsStore
	{
		// The value of this constant must be left as 'WinRT' for legacy reasons.
		// It may be hard coded inside Applications and elsewhere, such that changing
		// it would cause breakage.
        /// <summary>
        /// The name of the store used for Universal Windows Platform builds.
        /// </summary>
		public const string Name = "WinRT";
	}
}
