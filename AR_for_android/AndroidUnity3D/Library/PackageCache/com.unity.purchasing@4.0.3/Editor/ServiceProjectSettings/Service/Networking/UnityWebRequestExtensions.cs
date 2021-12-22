using UnityEngine.Networking;

namespace UnityEditor.Purchasing
{
    static class UnityWebRequestExtensions
    {
        public static bool IsResultTransferSuccess(this UnityWebRequest request)
        {
#if UNITY_2020_1_OR_NEWER
            return request.isDone && request.result == UnityWebRequest.Result.Success;
#else
            return request.isDone && !request.isNetworkError && !request.isHttpError;
#endif
        }

        public static bool IsResultProtocolError(this UnityWebRequest request)
        {
#if UNITY_2020_1_OR_NEWER
            return request.isDone && request.result == UnityWebRequest.Result.ProtocolError;
#else
            return request.isDone && request.isHttpError;
#endif
        }
    }
}
