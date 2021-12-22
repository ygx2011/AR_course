namespace UnityEngine.Purchasing
{
    /// <summary>
    /// The type of Native App store being used.
    /// </summary>
    public enum AppStore
    {
        /// <summary>
        /// No store specified.
        /// </summary>
        NotSpecified,

        /// <summary>
        /// GooglePlay Store.
        /// </summary>
        GooglePlay, //<= Map to AndroidStore. First Android store. In AppStoreMeta.

        /// <summary>
        /// Amazon App Store.
        /// </summary>
        AmazonAppStore, //

        /// <summary>
        /// Unity Distribution Portal, which supports a set of stores internally.
        /// </summary>
        UDP, // Last Android store. Also in AppStoreMeta.

        /// <summary>
        /// MacOS App Store.
        /// </summary>
        MacAppStore,

        /// <summary>
        /// iOS or tvOS App Stores.
        /// </summary>
        AppleAppStore,

        /// <summary>
        /// Universal Windows Platform's store.
        /// </summary>
        WinRT,

        /// <summary>
        /// A fake store used for testing and Play-In-Editor.
        /// </summary>
        fake
    }

    // Note these must be synchronized with constants in the AndroidStore enum.
    /// <summary>
    /// A meta enum to bookend the app Stores for Android. Mapped from <c>AppStore</c>'s values.
    /// Is distinct from <c>AppStore</c> to avoid non-unique Enum.Parse and Enum.ToString lookup conflicts.
    /// </summary>
    public enum AppStoreMeta
    {
        /// <summary>
        /// The first Android App Store.
        /// </summary>
        AndroidStoreStart = AppStore.GooglePlay,

        /// <summary>
        /// The last Android App Store.
        /// </summary>
        AndroidStoreEnd = AppStore.UDP
    }
}
