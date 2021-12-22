namespace UnityEngine.Advertisements {
    /// <summary>
    /// The enumerated causes of an ad failing to show.
    /// </summary>
    public enum UnityAdsShowError {
        /// <summary>
        /// The SDK is not properly initialized.
        /// </summary>
        NOT_INITIALIZED,

        /// <summary>
        /// The Placement was not ready to show an ad.
        /// </summary>
        NOT_READY,

        /// <summary>
        /// An error occured related to the media player.
        /// </summary>
        VIDEO_PLAYER_ERROR,

        /// <summary>
        /// Show failed due to invalid parameters.
        /// </summary>
        INVALID_ARGUMENT,

        /// <summary>
        /// An error occured due to the device's internet connection.
        /// </summary>
        NO_CONNECTION,

        /// <summary>
        /// An ad is already showing in the specified Placement.
        /// </summary>
        ALREADY_SHOWING,

        /// <summary>
        /// An internal Unity Ads service error occurred.
        /// </summary>
        INTERNAL_ERROR,

        /// <summary>
        /// An unknown error occurred.
        /// </summary>
        UNKNOWN
    }
}
