namespace UnityEngine.Purchasing
{
    /// <summary>
    /// The type of Android store being run.
    /// </summary>
    public enum AndroidStore
    {
        /// <summary>
        /// GooglePlay Store
        /// </summary>
        GooglePlay, //<= Map to AppStore

        /// <summary>
        /// Amazon App Store.
        /// </summary>
        AmazonAppStore, //

        /// <summary>
        /// Unity Distribution Portal, which manages other stores internally.
        /// </summary>
        /// <seealso cref="https://unity.com/products/unity-distribution-portal"/>
        UDP, //

        /// <summary>
        /// No Android Store specified. Usually the case if not using Android.
        /// </summary>
        NotSpecified
    }

    /// <summary>
    /// A meta enum to bookend the app Stores for Android. Mapped from <c>AndroidStore</c> values.
    /// Is distinct from AndroidStore to avoid non-unique Enum.Parse and Enum.ToString lookup conflicts.
    /// Note these must be synchronized with constants in the <c>AppStore</c> enum.
    /// </summary>
    public enum AndroidStoreMeta
    {
        /// <summary>
        /// The first Android App Store.
        /// </summary>
        AndroidStoreStart = AndroidStore.GooglePlay,

        /// <summary>
        /// The last Android App Store.
        /// </summary>
        AndroidStoreEnd = AndroidStore.UDP
    }
}
