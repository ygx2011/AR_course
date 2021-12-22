using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace UnityEngine.Advertisements.Editor {
    internal class SkAdNetworkJsonParser : ISkAdNetworkParser {
        [Serializable]
        public class SkAdNetworkIdArray {
            public List<SkAdNetworkInfo> skadnetwork_ids;
        }

        [Serializable]
        public class SkAdNetworkInfo {
            public string skadnetwork_id;
        }

        public string GetExtension() {
            return SkAdNetworkFileExtension.JSON;
        }

        public HashSet<string> ParseSource(ISkAdNetworkSource source) {
            var foundIds = new HashSet<string>();
            try {
                string jsonData;
                using (var stream = source.Open()) {
                    if (stream == null) {
                        Debug.LogWarning($"[Unity SKAdNetwork Parser] Unable to parse SKAdNetwork file: {source.Path}");
                        return foundIds;
                    }

                    jsonData = new StreamReader(stream).ReadToEnd();
                }

                SkAdNetworkIdArray skAdNetworkCompanyInfo = null;
                try {
                    skAdNetworkCompanyInfo = JsonUtility.FromJson<SkAdNetworkIdArray>(jsonData);
                } catch (Exception) { }

                //Fallback to try and see if this is a JSONObject which contains an array element called skadnetwork_ids instead of the expected JSONArray
                if (skAdNetworkCompanyInfo?.skadnetwork_ids == null || skAdNetworkCompanyInfo.skadnetwork_ids.Count == 0) {
                    var updatedJson = "{\"skadnetwork_ids\":" + jsonData + "}";
                    skAdNetworkCompanyInfo = JsonUtility.FromJson<SkAdNetworkIdArray>(updatedJson);
                }

                if (skAdNetworkCompanyInfo?.skadnetwork_ids == null) {
                    Debug.LogWarning($"[Unity SKAdNetwork Parser] Unable to parse SKAdNetwork file: {source.Path}");
                    return foundIds;
                }

                foundIds.UnionWith(skAdNetworkCompanyInfo.skadnetwork_ids.Select(t => t.skadnetwork_id).Where(t => t != null));

            } catch (Exception) {
                Debug.LogWarning($"[Unity SKAdNetwork Parser] Unable to parse SKAdNetwork file: {source.Path}");
            }

            return foundIds;
        }
    }
}
