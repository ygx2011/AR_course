#if SERVICES_SDK_CORE_ENABLED
using System;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine.Networking;

namespace UnityEngine.Advertisements.Editor
{
    class RequestGameIds
    {
        [Serializable]
        public class Response
        {
            public string iOSGameKey;
            public string androidGameKey;
        }

        [Serializable]
        class Body
        {
            public string projectGUID;
            [CanBeNull]
            public string projectName;
            public string token;
        }

        const string k_ProductionDomain = "https://legacy-editor-integration.dashboard.unity3d.com";
        const string k_GameIdApiUrl = "/unity/v1/games/";

        public void SendWithRetry(Action<Response> onSuccess, Action<Exception> onError, int retryDelayInSeconds = 1, int maxRetryCount = 10)
        {
            var currentRetry = 0;
            var timer = new EditorTimer
            {
                IntervalInSeconds = retryDelayInSeconds
            };
            timer.Elapsed += OnTimerElapsed;

            SendAndMonitorRequest();

            void SendAndMonitorRequest()
            {
                Send(OnRequestCompletedSuccess, OnRequestCompletedError);
            }

            void OnRequestCompletedSuccess(Response response)
            {
                onSuccess?.Invoke(response);
            }

            void OnRequestCompletedError(Exception exception)
            {
                if (currentRetry < maxRetryCount)
                {
                    timer.Restart();
                }
                else
                {
                    onError?.Invoke(exception);
                }
            }

            void OnTimerElapsed()
            {
                ++currentRetry;
                SendAndMonitorRequest();
            }
        }

        public void Send(Action<Response> onSuccess, Action<Exception> onError)
        {
            try
            {
                var webRequest = CreateWebRequest(k_ProductionDomain);
                webRequest.SendWebRequest().completed += OnUnityWebRequestCompleted;
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
            }

            void OnUnityWebRequestCompleted(AsyncOperation webOperation)
            {
                using (var webRequest = ((UnityWebRequestAsyncOperation)webOperation).webRequest)
                {
                    var hasRequestFailed = false;
#if UNITY_2020_2_OR_NEWER
                    hasRequestFailed = webRequest.result != UnityWebRequest.Result.Success;
#else
                    hasRequestFailed = webRequest.isNetworkError
                        || webRequest.isHttpError;
#endif
                    if (hasRequestFailed)
                    {
                        var message = "Couldn't fetch Ads Service game Ids.\n" +
                            $"Error: {webRequest.error}" +
                            $"Message: {webRequest.downloadHandler.text}";
                        onError?.Invoke(new Exception(message));
                        return;
                    }

                    var response = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
                    onSuccess?.Invoke(response);
                }
            }
        }

        static UnityWebRequest CreateWebRequest(string environmentDomain)
        {
            var body = new Body
            {
                projectGUID = CloudProjectSettings.projectId,
                projectName = CloudProjectSettings.projectName,
                token = CloudProjectSettings.accessToken
            };
            var serializeBody = JsonUtility.ToJson(body);
            var webRequest = new UnityWebRequest(environmentDomain + k_GameIdApiUrl, UnityWebRequest.kHttpVerbPOST)
            {
                downloadHandler = new DownloadHandlerBuffer(),
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(serializeBody))
            };
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

            return webRequest;
        }
    }
}
#endif
