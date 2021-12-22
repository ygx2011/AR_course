#if UNITY_IOS
using System;
using System.Runtime.InteropServices;
using AOT;

namespace UnityEngine.Advertisements.Platform.iOS
{
    internal class IosInitializationListener : IosNativeObject
    {
        private delegate void InitSuccessCallback(IntPtr ptr);
        private delegate void InitFailureCallback(IntPtr ptr, int error, string message);
        private IUnityAdsInitializationListener m_UnityAdsInternalListener;
        private IUnityAdsInitializationListener m_UserListener;

        [DllImport("__Internal", EntryPoint = "UnityAdsInitializationListenerCreate")]
        private static extern IntPtr InitializationListenerCreate(InitSuccessCallback initSuccessCallback, InitFailureCallback initFailureCallback);
        [DllImport("__Internal", EntryPoint = "UnityAdsInitializationListenerDestroy")]
        private static extern void InitializationListenerDestroy(IntPtr ptr);

        public IosInitializationListener(IUnityAdsInitializationListener unityAdsInternalListener, IUnityAdsInitializationListener userListener)
        {
            NativePtr = InitializationListenerCreate(OnInitializationComplete, OnInitializationFailed);
            m_UnityAdsInternalListener = unityAdsInternalListener;
            m_UserListener = userListener;
        }
        public override void Dispose()
        {
            if (NativePtr == IntPtr.Zero) return;
            InitializationListenerDestroy(NativePtr);
            NativePtr = IntPtr.Zero;
            m_UnityAdsInternalListener = null;
            m_UserListener = null;
        }

        private void OnInitializationComplete() {
            m_UnityAdsInternalListener?.OnInitializationComplete();
            m_UserListener?.OnInitializationComplete();
        }

        private void OnInitializationFailed(UnityAdsInitializationError error, string message) {
            m_UnityAdsInternalListener?.OnInitializationFailed(error, message);
            m_UserListener?.OnInitializationFailed(error, message);
        }

        [MonoPInvokeCallback(typeof(InitSuccessCallback))]
        private static void OnInitializationComplete(IntPtr ptr) {
            var listener = Get<IosInitializationListener>(ptr);
            if (listener == null) return;
            if (listener.CheckDisposedAndLogError($"Expected listener [{ptr}] has been disposed already.")) return;
            listener.OnInitializationComplete();
        }

        [MonoPInvokeCallback(typeof(InitFailureCallback))]
        private static void OnInitializationFailed(IntPtr ptr, int error, string message)
        {
            var listener = Get<IosInitializationListener>(ptr);
            if (listener == null) return;
            if (listener.CheckDisposedAndLogError($"Expected listener [{ptr}] has been disposed already.")) return;
            listener.OnInitializationFailed((UnityAdsInitializationError)error, message);
        }
    }
}
#endif
