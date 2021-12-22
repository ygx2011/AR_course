using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor.Purchasing
{
    class GoogleConfigurationWebRequests
    {
        const string k_GoogleKeySubPath = "/api/v2/projects/";
        const string k_GoogleKeyGetSuffix = "/get_google_pub_key";
        const string k_GoogleKeyPostSuffix = "/set_google_pub_key";
        const string k_GoogleKeyJsonLabel = "google_pub_key";

        const string k_AuthHeaderName = "Authorization";
        const string k_AuthHeaderValueFormat = "Basic {0}";
        const string k_ContentHeaderName = "Content-Type";
        const string k_ContentHeaderValue = "application/json;charset=UTF-8";

        IWebRequestInternal m_WebRequest = new CloudProjectWebRequest();

        UnityWebRequest m_GetGoogleKeyRequest;
        GoogleConfigurationData m_PurchasingRemoteDataRef;

        Action<string> m_GetGooglePlayKeyCallback;
        Action<GooglePlayRevenueTrackingKeyState> m_SetGooglePlayKeyCallback;

        internal GoogleConfigurationWebRequests(GoogleConfigurationData remoteData, Action<string> onGetGooglePlayKey, Action<GooglePlayRevenueTrackingKeyState> onSetGooglePlayKey)
        {
            m_PurchasingRemoteDataRef = remoteData;

            m_GetGooglePlayKeyCallback = onGetGooglePlayKey;
            m_SetGooglePlayKeyCallback = onSetGooglePlayKey;
        }

        ~GoogleConfigurationWebRequests()
        {
            CancelGetGoogleKeyRequest();
        }

        void CancelGetGoogleKeyRequest()
        {
            m_GetGoogleKeyRequest?.Abort();
            m_GetGoogleKeyRequest?.Dispose();
            m_GetGoogleKeyRequest = null;
        }

        internal void RequestRetrieveKeyOperation()
        {
            AuthSignatureWebRequests.RequestAuthSignature(m_WebRequest, GetGooglePlayKey);
        }

        void GetGooglePlayKey(string projectAuthSignature)
        {
            if (m_GetGoogleKeyRequest != null)
            {
                BuildGetGooglePlayKeyWebRequest(projectAuthSignature);

                var operation = m_GetGoogleKeyRequest.SendWebRequest();
                operation.completed += OnGetGooglePlayKey;
            }
        }

        void BuildGetGooglePlayKeyWebRequest(string projectAuthSignature)
        {
            m_GetGoogleKeyRequest = UnityWebRequest.Get(GetGoogleKeyResource() + k_GoogleKeyGetSuffix);
            m_GetGoogleKeyRequest.suppressErrorsToConsole = true;

            AddAuthTokenToRequestHeader(m_GetGoogleKeyRequest, projectAuthSignature);
        }

        static void AddAuthTokenToRequestHeader(UnityWebRequest request, string projectAuthSignature)
        {
            var encodedAuthToken = NetworkingUtils.Base64Encode(NetworkingUtils.GetProjectGuid() + ":" + projectAuthSignature);
            request.SetRequestHeader(k_AuthHeaderName, string.Format(k_AuthHeaderValueFormat, encodedAuthToken));
        }

        void OnGetGooglePlayKey(AsyncOperation getKeyOperation)
        {
            var webOp = (UnityWebRequestAsyncOperation)getKeyOperation;

            if (webOp?.isDone == true && m_GetGoogleKeyRequest != null)
            {
                FetchGooglePlayKeyFromRequest();

                m_GetGoogleKeyRequest.Dispose();
                m_GetGoogleKeyRequest = null;
            }
        }

        void FetchGooglePlayKeyFromRequest()
        {
            string googlePlayKey = "";
            if (IsGoogleKeyRequestResultSuccess())
            {
                try
                {
                    googlePlayKey = NetworkingUtils.GetValueFromJsonDictionary(m_GetGoogleKeyRequest.downloadHandler.text, k_GoogleKeyJsonLabel);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);

                    m_PurchasingRemoteDataRef.googlePlayKey = "";
                }
            }
            else
            {
                m_PurchasingRemoteDataRef.googlePlayKey = "";
            }

            m_PurchasingRemoteDataRef.googlePlayKey = googlePlayKey;

            m_GetGooglePlayKeyCallback(m_PurchasingRemoteDataRef.googlePlayKey);

        }

        bool IsGoogleKeyRequestResultSuccess()
        {
            return m_GetGoogleKeyRequest.IsResultTransferSuccess();
        }

        internal void RequestUpdateOperation()
        {
            AuthSignatureWebRequests.RequestAuthSignature(m_WebRequest, PushGooglePlayKey);
        }

        void PushGooglePlayKey(string projectAuthSignature)
        {
            var request = BuildPushGooglePlayKeyRequest(projectAuthSignature);

            var operation = request.SendWebRequest();
            operation.completed += OnSubmitGooglePlayKey;
        }

        UnityWebRequest BuildPushGooglePlayKeyRequest(string projectAuthSignature)
        {
            var payload = "{\"" + k_GoogleKeyJsonLabel + "\": \"" + m_PurchasingRemoteDataRef.googlePlayKey + "\"}";
            var jsonUploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(payload));
            var request = new UnityWebRequest(GetGoogleKeyResource() + k_GoogleKeyPostSuffix,
                    UnityWebRequest.kHttpVerbPOST)
            {
                uploadHandler = jsonUploadHandler,
                suppressErrorsToConsole = true
            };

            AddAuthTokenToRequestHeader(request, projectAuthSignature);
            request.SetRequestHeader(k_ContentHeaderName, k_ContentHeaderValue);

            return request;
        }

        void OnSubmitGooglePlayKey(AsyncOperation pushKeyOperation)
        {
            var pushKeyWebOperation = (UnityWebRequestAsyncOperation)pushKeyOperation;

            if (pushKeyWebOperation?.isDone == true)
            {
                var completedRequest = pushKeyWebOperation.webRequest;
                if (completedRequest != null)
                {
                    HandleCompletedSubmitResponse(completedRequest);
                }
            }
        }

        void HandleCompletedSubmitResponse(UnityWebRequest completedRequest)
        {
            GooglePlayRevenueTrackingKeyState keyState;

            if (completedRequest.IsResultTransferSuccess())
            {
                keyState = GooglePlayRevenueTrackingKeyState.Verified;
            }
            else if (completedRequest.IsResultProtocolError())
            {
                keyState = InterpretKeyStateFromProtocolError(completedRequest.responseCode);
            }
            else
            {
                keyState = GooglePlayRevenueTrackingKeyState.InvalidFormat;
            }

            m_SetGooglePlayKeyCallback(keyState);
        }

        static GooglePlayRevenueTrackingKeyState InterpretKeyStateFromProtocolError(long responseCode)
        {
            switch (responseCode)
            {
                case 401:
                case 403:
                    return GooglePlayRevenueTrackingKeyState.UnauthorizedUser;
                case 405:
                case 500:
                    return GooglePlayRevenueTrackingKeyState.ServerError;
                default:
                    return GooglePlayRevenueTrackingKeyState.InvalidFormat;
            }
        }

        static string GetGoogleKeyResource()
        {
            return PurchasingUrls.analyticsApiUrl + k_GoogleKeySubPath + NetworkingUtils.GetProjectGuid();
        }
    }
}
