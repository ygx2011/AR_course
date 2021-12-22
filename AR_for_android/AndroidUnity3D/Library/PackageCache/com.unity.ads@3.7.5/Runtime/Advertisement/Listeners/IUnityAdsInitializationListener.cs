using System;

namespace UnityEngine.Advertisements {
    public interface IUnityAdsInitializationListener {

        /// <summary>
        /// Callback which notifies UnityAds has been successfully initialized.
        /// </summary>
        void OnInitializationComplete();

        /// <summary>
        /// Callback which notifies UnityAds has failed initialization with error message and error category.
        /// </summary>
        /// <param name="error"> If UnityAdsInitializationError.INTERNAL_ERROR, initialization failed due to environment or internal services
        /// If UnityAdsInitializationError.INVALID_ARGUMENT, initialization failed due to invalid argument(e.g. game ID)
        /// If UnityAdsInitializationError.AD_BLOCKER_DETECTED, initialization failed due to url being blocked</param>
        /// <param name="message">Human-readable error message</param>
        void OnInitializationFailed(UnityAdsInitializationError error, string message);
    }
}
