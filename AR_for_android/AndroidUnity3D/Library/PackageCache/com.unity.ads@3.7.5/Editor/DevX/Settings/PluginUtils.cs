#if SERVICES_SDK_CORE_ENABLED
using System.Collections.Generic;
using UnityEditor;

namespace UnityEngine.Advertisements.Editor
{
    static class PluginUtils
    {
        /// <summary>
        /// Contain all GUIDs of Ads DLLs from the Asset Store.
        /// </summary>
        /// <remarks>
        /// Used to verify if the Asset Store package is installed.
        /// </remarks>
        public static readonly string[] AssetStoreDllGuids =
        {
            // Android DLL
            "cad99f482ce25421196533fe02e6a13e",

            // IOS DLL
            "d6f3e2ade30154a80a137e0079f66a08",

            // Editor DLL
            "56921141d53fd4a5888445107b1b1286"
        };

        public static bool AreAssetStorePluginsInstalled()
            => ArePluginsInstalled(AssetStoreDllGuids);

        public static bool ArePluginsInstalled(IEnumerable<string> pluginGuids)
        {
            // if a plugin is not found return false
            foreach (var pluginGuid in pluginGuids)
            {
                if (GetImporterFromGuid<PluginImporter>(pluginGuid) == null)
                {
                    return false;
                }
            }

            return true;
        }

        static TImporter GetImporterFromGuid<TImporter>(string assetGuid)
            where TImporter : AssetImporter
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            return AssetImporter.GetAtPath(assetPath) as TImporter;
        }
    }
}
#endif
