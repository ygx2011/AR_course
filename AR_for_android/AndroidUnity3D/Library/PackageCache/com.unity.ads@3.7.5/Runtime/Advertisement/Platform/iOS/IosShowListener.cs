#if UNITY_IOS
using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine.Advertisements.Platform.iOS;

namespace UnityEngine.Advertisements {
    internal class IosShowListener : IosNativeObject {
        private delegate void ShowFailureCallback(IntPtr ptr, string placementId, int error, string message);
        private delegate void ShowStartCallback(IntPtr ptr, string placementId);
        private delegate void ShowClickCallback(IntPtr ptr, string placementId);
        private delegate void ShowCompleteCallback(IntPtr ptr, string placementId, int completionState);
        private IUnityAdsShowListener m_UnityAdsInternalListener;
        private IUnityAdsShowListener m_UserListener;

        [DllImport("__Internal", EntryPoint = "UnityAdsShowListenerCreate")]
        private static extern IntPtr ShowListenerCreate(ShowFailureCallback showFailureCallback, ShowStartCallback showStartCallback, ShowClickCallback showClickCallback, ShowCompleteCallback showCompleteCallback);
        [DllImport("__Internal", EntryPoint = "UnityAdsShowListenerDestroy")]
        private static extern void ShowListenerDestroy(IntPtr ptr);

        public IosShowListener(IUnityAdsShowListener unityAdsInternalListener, IUnityAdsShowListener userListener)
        {
            NativePtr = ShowListenerCreate(OnShowFailure, OnShowStart, OnShowClick, OnShowComplete);
            m_UnityAdsInternalListener = unityAdsInternalListener;
            m_UserListener = userListener;
        }
        public override void Dispose()
        {
            if (NativePtr == IntPtr.Zero) return;
            ShowListenerDestroy(NativePtr);
            NativePtr = IntPtr.Zero;
            m_UnityAdsInternalListener = null;
            m_UserListener = null;
        }

        private void OnShowFailure(string placementId, UnityAdsShowError error, string message) {
            m_UnityAdsInternalListener?.OnUnityAdsShowFailure(placementId, error, message);
            m_UserListener?.OnUnityAdsShowFailure(placementId, error, message);
        }

        private void OnShowStart(string placementId) {
            m_UnityAdsInternalListener?.OnUnityAdsShowStart(placementId);
            m_UserListener?.OnUnityAdsShowStart(placementId);
        }

        private void OnShowClick(string placementId) {
            m_UnityAdsInternalListener?.OnUnityAdsShowClick(placementId);
            m_UserListener?.OnUnityAdsShowClick(placementId);
        }

        private void OnShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
            m_UnityAdsInternalListener?.OnUnityAdsShowComplete(placementId, showCompletionState);
            m_UserListener?.OnUnityAdsShowComplete(placementId, showCompletionState);
        }

        [MonoPInvokeCallback(typeof(ShowFailureCallback))]
        private static void OnShowFailure(IntPtr ptr, string placementId, int error, string message) {
            var listener = Get<IosShowListener>(ptr);
            if (listener == null) return;
            if (listener.CheckDisposedAndLogError($"Expected listener [{ptr}] has been disposed already.")) return;
            listener.OnShowFailure(placementId, (UnityAdsShowError)error, message);
        }

        [MonoPInvokeCallback(typeof(ShowStartCallback))]
        private static void OnShowStart(IntPtr ptr, string placementId) {
            var listener = Get<IosShowListener>(ptr);
            if (listener == null) return;
            if (listener.CheckDisposedAndLogError($"Expected listener [{ptr}] has been disposed already.")) return;
            listener.OnShowStart(placementId);
        }

        [MonoPInvokeCallback(typeof(ShowClickCallback))]
        private static void OnShowClick(IntPtr ptr, string placementId) {
            var listener = Get<IosShowListener>(ptr);
            if (listener == null) return;
            if (listener.CheckDisposedAndLogError($"Expected listener [{ptr}] has been disposed already.")) return;
            listener.OnShowClick(placementId);
        }

        [MonoPInvokeCallback(typeof(ShowCompleteCallback))]
        private static void OnShowComplete(IntPtr ptr, string placementId, int completionState) {
            var listener = Get<IosShowListener>(ptr);
            if (listener == null) return;
            if (listener.CheckDisposedAndLogError($"Expected listener [{ptr}] has been disposed already.")) return;
            listener.OnShowComplete(placementId, (UnityAdsShowCompletionState)completionState);
        }
    }
}
#endif
