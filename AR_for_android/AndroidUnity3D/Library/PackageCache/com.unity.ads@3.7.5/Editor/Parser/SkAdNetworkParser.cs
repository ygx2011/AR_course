using System;
using System.Collections.Generic;

namespace UnityEngine.Advertisements.Editor {
    internal static class SkAdNetworkParser {
        private static Dictionary<string, ISkAdNetworkParser> s_Parsers;

        static SkAdNetworkParser() {
            s_Parsers = new Dictionary<string, ISkAdNetworkParser> {
                { SkAdNetworkFileExtension.XML, new SkAdNetworkXmlParser() },
                { SkAdNetworkFileExtension.JSON, new SkAdNetworkJsonParser() },
                { SkAdNetworkFileExtension.NONE, new SkAdNetworkUrlParser() }
            };
        }

        public static ISkAdNetworkParser GetParser(string parserType)
        {
            try {
                s_Parsers.TryGetValue(parserType, out var parser);
                return parser;
            }
            catch (Exception) { }
            return null;
        }

        public static IEnumerable<ISkAdNetworkParser> GetAllParsers() {
            return s_Parsers.Values;
        }
    }
}
