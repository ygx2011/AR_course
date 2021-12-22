using System;

namespace UnityEngine.Advertisements {
    /// <summary>
    /// An interface for executing logic when an ad either shows or fails to show.
    /// </summary>
    public interface IUnityAdsShowListener {
        /// <summary>
        /// Executes logic for an ad failing to show.
        /// </summary>
        /// <param name="placementId">The unique identifier of the Placement attempting to show content.</param>
        /// <param name="error">The <c>UnityAdsShowError</c> that caused the failure.</param>
        /// <param name="message">The error message accompanying the <c>UnityAdsShowError</c>.</param>
        void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message);

        /// <summary>
        /// Executes logic for an ad successfully showing.
        /// </summary>
        /// <param name="placementId">The unique identifier of the Placement attempting to show content.</param>
        void OnUnityAdsShowStart(string placementId);

        /// <summary>
        /// Executes logic for a user clicking an ad while it is showing.
        /// </summary>
        /// <param name="placementId">The unique identifier of the Placement attempting to show content.</param>
        void OnUnityAdsShowClick(string placementId);

        /// <summary>
        /// Executes logic for the ad finishing in its entirety. 
        /// </summary>
        /// <param name="placementId">The unique identifier of the Placement attempting to show content.</param>
        /// <param name="showCompletionState">The <c>UnityAdsShowCompletionState</c> result of the show call.</param>
        void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState);
    }
}

