using System.IO;
using UnityEngine;

namespace UnityEngine.Advertisements.Editor {
    internal interface ISkAdNetworkSource {
        string Path { get; }
        Stream Open();
    }
}
