using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace UnityEngine.Advertisements.Editor {
    internal class SkAdNetworkXmlParser : ISkAdNetworkParser {
        private const string k_SkAdNetworkIdentifier = "SKAdNetworkIdentifier";

        public string GetExtension() {
            return SkAdNetworkFileExtension.XML;
        }

        public HashSet<string> ParseSource(ISkAdNetworkSource source) {
            var foundIds = new HashSet<string>();
            try {
                var xmlDocument = new XmlDocument();

                using (var stream = source.Open()) {
                    if (stream == null) {
                        Debug.LogWarning("[Unity SKAdNetwork Parser] Unable to parse SKAdNetwork file: {source.Path}");
                        return foundIds;
                    }

                    xmlDocument.Load(stream);
                }

                var items = xmlDocument.GetElementsByTagName("key");
                for (var x = 0; x < items.Count; x++) {
                    if (items[x].InnerText == k_SkAdNetworkIdentifier) {
                        var nextSibling = items[x]?.NextSibling;
                        if (nextSibling != null) {
                            foundIds.Add(nextSibling.InnerText);
                        }
                    }
                }
            }
            catch (Exception) {
                Debug.LogWarning($"[Unity SKAdNetwork Parser] Unable to parse SKAdNetwork file: {source.Path}");
            }

            return foundIds;
        }
    }
}
