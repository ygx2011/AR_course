#if UNITY_2018_1_OR_NEWER  && UNITY_IOS
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.iOS.Xcode;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build.Reporting;

namespace UnityEngine.Advertisements.Editor
{
    internal class PostProcessBuildPlist : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;
        private const string k_SkAdNetworkIdentifier = "SKAdNetworkIdentifier";
        private const string k_SkAdNetworkItems = "SKAdNetworkItems";
        private const string k_SkAdNetworksFileName = "SKAdNetworks";
        private const string k_UnitySkAdNetworkId = "4DZT52R2T5.skadnetwork";

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.iOS)
            {
                return;
            }

            UpdateInfoPlistWithSkAdNetworkIds(report.summary.outputPath);
        }

        internal static void UpdateInfoPlistWithSkAdNetworkIds(string pathToPlistFile) {
            var provider = new SkAdNetworkLocalSourceProvider();
            var ids = new HashSet<string>();
            try {
                SkAdNetworkParser.GetAllParsers().ToList().ForEach(parser => {
                    provider.GetSources(k_SkAdNetworksFileName, parser.GetExtension()).ToList().ForEach(source => {
                        ids.UnionWith(parser.ParseSource(source));
                    });
                });
            }
            catch (Exception e) {
                Debug.LogError($"Failed to parse SKAdNetwork files due to following reason: {e.Message}");
            }

            if (!ids.Contains(k_UnitySkAdNetworkId)) {
                ids.Add(k_UnitySkAdNetworkId);
            }

            try {
                WriteSkAdNetworkIdsToInfoPlist(ids, pathToPlistFile);
            }
            catch (Exception e) {
                Debug.LogError($"Failed to update info.plist file due to following reason: {e.Message}");
            }
        }

        /// <summary>
        /// Write all plistValues to an existing Info.plist file
        /// </summary>
        internal static void WriteSkAdNetworkIdsToInfoPlist(HashSet<string> skAdNetworkIds, string outputPath)
        {
            var infoPlistPath = outputPath + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(infoPlistPath));
            var root = plist.root;

            if (root == null) {
                Debug.LogWarning("[Unity SKAdNetwork Parser] Unable to parse info.plist.  Unable to add SkAdNetwork Identifiers.");
                return;
            }

            if(!root.values?.ContainsKey(k_SkAdNetworkItems) ?? false) {
                root.CreateArray(k_SkAdNetworkItems);
            }

            var adNetworkItems = root[k_SkAdNetworkItems].AsArray();

            if (adNetworkItems == null) {
                Debug.LogWarning("[Unity SKAdNetwork Parser] Unable to modify existing info.plist.  Unable to add SkAdNetwork Identifiers.");
                return;
            }

            foreach (var adNetworkId in skAdNetworkIds) {
                if (!PlistContainsAdNetworkId(adNetworkItems, adNetworkId)) {
                    adNetworkItems.AddDict().SetString(k_SkAdNetworkIdentifier, adNetworkId);
                }
            }

            File.WriteAllText(infoPlistPath, plist.WriteToString());
        }

        /// <summary>
        /// Check if the value is already contained in the plist
        /// </summary>
        internal static bool PlistContainsAdNetworkId(PlistElementArray adNetworkItems, string adNetworkId)
        {
            foreach(var adNetworkItem in adNetworkItems.values)
            {
                var item = adNetworkItem.AsDict();
                if(item.values.TryGetValue(k_SkAdNetworkIdentifier, out var value))
                {
                    if(value.AsString() == adNetworkId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
#endif //UNITY_2018_1_OR_NEWER
