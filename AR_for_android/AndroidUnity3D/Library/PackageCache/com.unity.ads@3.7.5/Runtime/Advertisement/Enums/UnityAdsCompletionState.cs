namespace UnityEngine.Advertisements {
    public enum UnityAdsCompletionState {
        /// <summary>
        /// A state that indicates that the user skipped the ad.
        /// </summary>
        SKIPPED,

        /// <summary>
        /// A state that indicates that the ad was played entirely.
        /// </summary>
        COMPLETED,

        /// <summary>
        /// Default value / Used when no mapping available
        /// </summary>
        UNKNOWN
    }
}
