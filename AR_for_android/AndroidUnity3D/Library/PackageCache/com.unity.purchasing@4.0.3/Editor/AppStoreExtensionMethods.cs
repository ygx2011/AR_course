using System.Collections.Generic;
using System.Linq;
using UnityEngine.Purchasing;

namespace UnityEditor.Purchasing
{
    internal static class AppStoreExtensionMethods
    {
        static readonly Dictionary<AppStore, string> AppStoreDisplayNames = new Dictionary<AppStore, string>()
        {
            {AppStore.AmazonAppStore, "Amazon Appstore"},
            {AppStore.AppleAppStore, "Apple App Store"},
            {AppStore.GooglePlay, "Google Play"},
            {AppStore.UDP, "Unity Distribution Portal"},
            {AppStore.MacAppStore, "Mac App Store"},
            {AppStore.WinRT, "Microsoft Store"},
            {AppStore.fake, "Fake App Store"}
        };

        public static string ToDisplayName(this AppStore value)
        {
            return AppStoreDisplayNames.ContainsKey(value) ? AppStoreDisplayNames[value] : "";
        }

        public static AppStore ToAppStoreFromDisplayName(this string value)
        {
            if (AppStoreDisplayNames.ContainsValue(value))
            {
                var dict = AppStoreDisplayNames;
                return dict.FirstOrDefault(x => x.Value == value).Key;
            }

            return AppStore.NotSpecified;
        }

        public static bool IsAndroid(this AppStore value)
        {
            return (int) value >= (int) AppStoreMeta.AndroidStoreStart &&
                   (int) value <= (int) AppStoreMeta.AndroidStoreEnd;
        }
    }
}
