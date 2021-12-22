using System;
using UnityEngine.Advertisements.Platform;
using UnityEngine.Advertisements.Utilities;

namespace UnityEngine.Advertisements {
    internal class AndroidInitializationListener : AndroidJavaProxy {
        private IPlatform m_Platform;
        private IUnityAdsInitializationListener m_ManagedListener;

        public AndroidInitializationListener(IPlatform platform, IUnityAdsInitializationListener initializationListener) : base("com.unity3d.ads.IUnityAdsInitializationListener") {
            m_Platform = platform;
            m_ManagedListener = initializationListener;
        }

        public void onInitializationComplete() {
            m_ManagedListener?.OnInitializationComplete();
        }

        public void onInitializationFailed(AndroidJavaObject error, string message) {
            m_Platform?.UnityAdsDidError(message);
            m_ManagedListener?.OnInitializationFailed(EnumUtilities.GetEnumFromAndroidJavaObject(error, UnityAdsInitializationError.UNKNOWN), message);
        }
    }
}
