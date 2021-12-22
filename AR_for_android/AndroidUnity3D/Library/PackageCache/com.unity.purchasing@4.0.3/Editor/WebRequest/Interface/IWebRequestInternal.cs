using System;
using UnityEngine.Networking;

namespace UnityEditor.Purchasing
{
    interface IWebRequestInternal
    {
        UnityWebRequest BuildWebRequest(string uri);
    }
}
