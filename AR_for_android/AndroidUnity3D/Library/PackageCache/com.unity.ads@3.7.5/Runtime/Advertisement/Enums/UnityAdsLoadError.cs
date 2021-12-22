namespace UnityEngine.Advertisements {
    /// <summary>
    /// The enumerated causes of an ad failing to load. 
    /// </summary>
    public enum UnityAdsLoadError {
        /// <summary>
        /// The SDK is not properly initialized.
        /// </summary>
        INITIALIZE_FAILED,
        /// <summary>
        /// An internal Unity Ads service error occurred.
        /// </summary>
        INTERNAL_ERROR,
        /// <summary>
        /// Load failed due to invalid parameters. 
        /// </summary>        
        INVALID_ARGUMENT,
        /// <summary>
        /// No ad content was available to load. 
        /// </summary>        
        NO_FILL,
        /// <summary>
        /// The ad did not load within a specified timeframe.
        /// </summary>        
        TIMEOUT,
        UNKNOWN
    }
}
