namespace UnityEngine.Advertisements {
    public enum UnityAdsInitializationError {
        /// <summary>
        /// Default value / Used when no mapping available
        /// </summary>
        UNKNOWN,

        /// <summary>
        /// Error related to environment or internal services
        /// </summary>
        INTERNAL_ERROR,

        /// <summary>
        /// Error related to invalid arguments
        /// </summary>
        INVALID_ARGUMENT,

        /// <summary>
        /// Error related to url being blocked
        /// </summary>
        AD_BLOCKER_DETECTED
    }
}
