using UnityEngine.Networking;

namespace UnityEditor.Purchasing
{
    class CloudProjectWebRequest: IWebRequestInternal
    {
        const string k_AuthHeaderName = "AUTHORIZATION";
        static readonly string k_AuthHeaderValue = $"Bearer {CloudProjectSettings.accessToken}";

        UnityWebRequest IWebRequestInternal.BuildWebRequest(string uri)
        {
            var authSignatureRequest = UnityWebRequest.Get(uri);
            authSignatureRequest.suppressErrorsToConsole = true;
            authSignatureRequest.SetRequestHeader(k_AuthHeaderName, k_AuthHeaderValue);
            return authSignatureRequest;
        }
    }
}
