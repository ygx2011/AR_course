using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Advertisements.Editor {
    internal interface ISkAdNetworkParser {
        string GetExtension();
        HashSet<string> ParseSource(ISkAdNetworkSource source);
    }
}
