using System;
using System.IO;
using UnityEngine;

namespace UnityEngine.Advertisements.Editor {
    internal class SkAdNetworkLocalSource : ISkAdNetworkSource {
        public string Path { get; }

        public SkAdNetworkLocalSource(string path) {
            Path = path;
        }

        public Stream Open() {
            return new FileStream(Path, FileMode.Open);
        }
    }
}
