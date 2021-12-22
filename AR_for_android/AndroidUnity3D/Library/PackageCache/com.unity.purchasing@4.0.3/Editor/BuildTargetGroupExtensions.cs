using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.Purchasing;

static class BuildTargetGroupExtensions
{
    internal static ReadOnlyCollection<string> ToAppStoreDisplayNames(this BuildTargetGroup value)
    {
        var stores = value.ToAppStores();

        var storeNames = stores.Select(store => store.ToDisplayName()).ToList();

        return storeNames.AsReadOnly();
    }

    internal static ReadOnlyCollection<AppStore> ToAppStores(this BuildTargetGroup value)
    {
        AppStore[] storesArray;
        switch (value)
        {
            case BuildTargetGroup.Android:
            {
                storesArray = ToAndroidAppStores(value);
                break;
            }

            case BuildTargetGroup.iOS:
            case BuildTargetGroup.tvOS:
                storesArray = new[] {AppStore.AppleAppStore};
                break;

            case BuildTargetGroup.WSA:
                storesArray = new[] {AppStore.WinRT};
                break;

            case BuildTargetGroup.Standalone:
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    storesArray = new[] {AppStore.MacAppStore};
                    break;
                }
                goto default;

            default:
                storesArray = new[] {AppStore.fake};
                break;
        }

        return Array.AsReadOnly(storesArray);
    }

    static AppStore[] ToAndroidAppStores(this BuildTargetGroup value)
    {
        if (value != BuildTargetGroup.Android)
        {
            return new AppStore[0];
        }

        var stores = new List<AppStore>();
        for (var store = (AppStore)AppStoreMeta.AndroidStoreStart;
            store <= (AppStore)AppStoreMeta.AndroidStoreEnd;
            ++store)
        {
            stores.Add(store);
        }

        return stores.ToArray();
    }

    internal static string ToPlatformDisplayName(this BuildTargetGroup value)
    {
        switch (value)
        {
            case BuildTargetGroup.iOS:
            {
                // TRICKY: Prefer an "iOS" string on BuildTarget, to avoid the unwanted "BuildTargetGroup.iPhone"
                return BuildTarget.iOS.ToString();
            }
            case BuildTargetGroup.Standalone:
            {
                switch (EditorUserBuildSettings.activeBuildTarget)
                {
                    case BuildTarget.StandaloneOSX:
                        return "macOS";
                    case BuildTarget.StandaloneWindows:
                        return "Windows";
                    default:
                        return BuildTargetGroup.Standalone.ToString();
                }
            }
            default:
                return value.ToString();
        }
    }
}
