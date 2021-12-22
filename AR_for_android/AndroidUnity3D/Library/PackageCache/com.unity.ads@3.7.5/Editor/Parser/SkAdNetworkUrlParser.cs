using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace UnityEngine.Advertisements.Editor {
    internal class SkAdNetworkUrlParser : ISkAdNetworkParser {
        public string GetExtension() {
            return SkAdNetworkFileExtension.NONE;
        }

        public HashSet<string> ParseSource(ISkAdNetworkSource source) {
            var foundIds = new HashSet<string>();

            try {
                string[] lines;
                using (var reader = new StreamReader(source.Open())) {
                    lines = reader.ReadToEnd().Split(Environment.NewLine.ToCharArray());
                }

                lines.Where(url => !string.IsNullOrEmpty(url))
                     .Where(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                     .ToList().ForEach(url => {
                    ISkAdNetworkParser parser = null;
                    switch (GetExtensionFromPath(url)) {
                        case SkAdNetworkFileExtension.XML:
                            parser = SkAdNetworkParser.GetParser(SkAdNetworkFileExtension.XML);
                            break;
                        case SkAdNetworkFileExtension.JSON:
                            parser = SkAdNetworkParser.GetParser(SkAdNetworkFileExtension.JSON);
                            break;
                    }

                    if (parser == null) {
                        Debug.LogWarning($"[Unity SKAdNetwork Parser] Unsupported file extension, No parser available to parse SKAdNetwork file: {source.Path} ");
                        return;
                    }

                    foundIds.UnionWith(parser.ParseSource(new SkAdNetworkRemoteSource(url)));
                });
            }
            catch (Exception) {
                Debug.LogWarning($"[Unity SKAdNetwork Parser] Unable to parse SKAdNetwork file: {source.Path}");
            }

            return foundIds;
        }

        /// <summary>
        /// Gets the extension for a filepath string
        /// </summary>
        private static string GetExtensionFromPath(string filepath) {
            var extension = Path.GetExtension(filepath);
            return string.IsNullOrEmpty(extension) ? "" : extension.Substring(1).ToLower();
        }
    }
}
