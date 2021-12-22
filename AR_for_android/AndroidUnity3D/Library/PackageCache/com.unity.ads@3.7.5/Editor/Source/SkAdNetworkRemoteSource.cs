using System.IO;
using System.Net;
using UnityEngine;

namespace UnityEngine.Advertisements.Editor {
    internal class SkAdNetworkRemoteSource : ISkAdNetworkSource {
        public string Path { get; }

        public SkAdNetworkRemoteSource(string path) {
            Path = path;
        }

        public Stream Open() {
            return new WebClient().OpenRead(Path);
        }
    }
}
