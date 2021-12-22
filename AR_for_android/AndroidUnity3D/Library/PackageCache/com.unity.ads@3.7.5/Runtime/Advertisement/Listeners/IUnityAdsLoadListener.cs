using System;

namespace UnityEngine.Advertisements {
    /// <summary>
    /// An interface for executing logic when an ad either loads or fails to load.
    /// </summary>
    public interface IUnityAdsLoadListener {
        /// <summary>
        /// Executes logic for ad content successfully loading to a specified Placement.
        /// </summary>
        /// <param name="placementId">The unique identifier of the Placement attempting to load content.</param>
        void OnUnityAdsAdLoaded(string placementId);

        /// <summary>
        /// Executes logic for ad content failing to load. 
        /// </summary>
        /// <param name="placementId">The unique identifier of the Placement attempting to load content.</param>
        /// <param name="error">The <c>UnityAdsLoadError</c> that caused the failure.</param>
        /// <param name="message">The error message accompanying the <c>UnityAdsLoadError</c>.</param>
        void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message);
    }
}
