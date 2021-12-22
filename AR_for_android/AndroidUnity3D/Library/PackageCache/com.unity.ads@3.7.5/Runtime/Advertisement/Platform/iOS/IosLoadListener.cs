#if UNITY_IOS
using System;
using System.Runtime.InteropServices;
using AOT;

namespace UnityEngine.Advertisements.Platform.iOS
{
    internal class IosLoadListener : IosNativeObject
    {
        private delegate void LoadSuccessCallback(IntPtr ptr, string placementId);
        private delegate void LoadFailureCallback(IntPtr ptr, string placementId, int error, string message);
        private IUnityAdsLoadListener m_UnityAdsInternalListener;
        private IUnityAdsLoadListener m_UserListener;

        [DllImport("__Internal", EntryPoint = "UnityAdsLoadListenerCreate")]
        private static extern IntPtr LoadListenerCreate(LoadSuccessCallback loadSuccessCallback, LoadFailureCallback loadFailureCallback);
        [DllImport("__Internal", EntryPoint = "UnityAdsLoadListenerDestroy")]
        private static extern void LoadListenerDestroy(IntPtr ptr);

        public IosLoadListener(IUnityAdsLoadListener unityAdsInternalListener, IUnityAdsLoadListener userListener)
        {
            NativePtr = LoadListenerCreate(OnLoadSuccess, OnLoadFailure);
            m_UnityAdsInternalListener = unityAdsInternalListener;
            m_UserListener = userListener;
        }
        public override void Dispose()
        {
            if (NativePtr == IntPtr.Zero) return;
            LoadListenerDestroy(NativePtr);
            NativePtr = IntPtr.Zero;
            m_UnityAdsInternalListener = null;
            m_UserListener = null;
        }

        private void OnLoadSuccess(string placementId) {
            m_UnityAdsInternalListener?.OnUnityAdsAdLoaded(placementId);
            m_UserListener?.OnUnityAdsAdLoaded(placementId);
        }

        private void OnLoadFailure(string placementId, UnityAdsLoadError error, string message) {
            m_UnityAdsInternalListener?.OnUnityAdsFailedToLoad(placementId, error, message);
            m_UserListener?.OnUnityAdsFailedToLoad(placementId, error, message);
        }

        [MonoPInvokeCallback(typeof(LoadSuccessCallback))]
        private static void OnLoadSuccess(IntPtr ptr, string placementId) {
            var listener = Get<IosLoadListener>(ptr);
            if (listener == null) return;
            if (listener.CheckDisposedAndLogError($"Expected listener [{ptr}] has been disposed already.")) return;
            listener.OnLoadSuccess(placementId);
        }

        [MonoPInvokeCallback(typeof(LoadFailureCallback))]
        private static void OnLoadFailure(IntPtr ptr, string placementId, int error, string message)
        {
            var listener = Get<IosLoadListener>(ptr);
            if (listener == null) return;
            if (listener.CheckDisposedAndLogError($"Expected listener [{ptr}] has been disposed already.")) return;
            listener.OnLoadFailure(placementId, (UnityAdsLoadError)error, message);
        }
    }
}
#endif
