using System;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor.Purchasing
{
    static class AuthSignatureWebRequests
    {
        const string k_AuthSignatureKeyJsonLabel = "auth_signature";

        internal static void RequestAuthSignature(IWebRequestInternal webRequest, Action<string> projectAuthSignature)
        {
            var request = webRequest.BuildWebRequest(BuildCoreProjectUri());
            var operation = request.SendWebRequest();
            operation.completed += _ => OnGetAuthSignature(request, projectAuthSignature);
        }

        static string BuildCoreProjectUri()
        {
            return string.Format(PurchasingUrls.coreProjectsUrl, NetworkingUtils.GetProjectGuid());
        }

        static void OnGetAuthSignature(UnityWebRequest request, Action<string> projectAuthSignature)
        {
            if (request.downloadHandler.isDone && request.IsResultTransferSuccess())
            {
                try
                {
                    projectAuthSignature?.Invoke(NetworkingUtils.GetValueFromJsonDictionary(request.downloadHandler.text, k_AuthSignatureKeyJsonLabel));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
    }
}
