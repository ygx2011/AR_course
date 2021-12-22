using System;
using System.Collections;
using System.Text;

#if UNITY_2018_3_OR_NEWER
using UnityEngine.Networking;
#endif

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Coroutine based IAsyncUtil.
    /// </summary>
    [AddComponentMenu("")]
    internal class AsyncWebUtil : MonoBehaviour, IAsyncWebUtil
    {
#if UNITY_2018_3_OR_NEWER
        public void Get(string url, Action<string> responseHandler, Action<string> errorHandler, int maxTimeoutInSeconds = 5)
        {
            var request = UnityWebRequest.Get(url);
            request.timeout = maxTimeoutInSeconds;

            Action<AsyncOperation> onGet = CreateGenericWebOperation(request, responseHandler, errorHandler);

            var operation = request.SendWebRequest();
            operation.completed += onGet;
        }

        public void Post(string url, string body, Action<string> responseHandler, Action<string> errorHandler, int maxTimeoutInSeconds = 5)
        {
            var request = UnityWebRequest.Post(url, body);
            request.timeout = maxTimeoutInSeconds;

            Action<AsyncOperation> onPost = CreateGenericWebOperation(request, responseHandler, errorHandler);

            var operation = request.SendWebRequest();
            operation.completed += onPost;
        }

        Action<AsyncOperation> CreateGenericWebOperation(UnityWebRequest request, Action<string> responseHandler, Action<string> errorHandler)
        {
            return op =>
            {
                if (op.isDone)
                {
                    if (!string.IsNullOrEmpty(request.error))
                    {
                        errorHandler(request.error);
                    }
                    else
                    {
                        responseHandler(request.downloadHandler.text);
                    }
                }

                request.Dispose();
            };
        }
#else
        public void Get(string url, Action<string> responseHandler, Action<string> errorHandler, int maxTimeoutInSeconds = 5)
        {
            var request = new WWW(url);
            StartCoroutine(Process(request, responseHandler, errorHandler, maxTimeoutInSeconds));
        }

        public void Post(string url, string body, Action<string> responseHandler, Action<string> errorHandler, int maxTimeoutInSeconds = 5)
        {
            Encoding enc = Encoding.UTF8;
            var request = new WWW(url, enc.GetBytes(body));
            StartCoroutine(Process(request, responseHandler, errorHandler, maxTimeoutInSeconds));
        }

        /// <summary>
        /// Handles network response and triggers responseHandler or errorHandler.
        /// errorHandler will be triggered if the network request takes longer than the maxTimeout.
        /// </summary>
        /// <returns>The process.</returns>
        /// <param name="request">Request.</param>
        /// <param name="responseHandler">Response handler.</param>
        /// <param name="errorHandler">Error handler.</param>
        /// <param name="maxTimeoutInSeconds"></param>
        private IEnumerator Process(WWW request, Action<string> responseHandler, Action<string> errorHandler, int maxTimeoutInSeconds)
        {
            float timer = 0;
            bool hasTimedOut = false;

            while (!request.isDone) {
                if (timer > maxTimeoutInSeconds) {
                    hasTimedOut = true;
                    break;
                }
                timer += Time.deltaTime;
                yield return null;
            }
            if (hasTimedOut || !string.IsNullOrEmpty(request.error)) {
                errorHandler(request.error);
            } else {
                responseHandler(request.text);
            }
            request.Dispose();
        }
#endif

        public void Schedule(Action a, int delayInSeconds)
        {
            StartCoroutine(DoInvoke(a, delayInSeconds));
        }

        private IEnumerator DoInvoke(Action a, int delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
            a();
        }
    }
}
