using UnityEngine.Advertisements.Platform;
using UnityEngine.Advertisements.Utilities;

namespace UnityEngine.Advertisements {
    internal class AndroidLoadListener : AndroidJavaProxy {
        private IPlatform m_Platform;
        private IUnityAdsLoadListener m_ManagedListener;

        public AndroidLoadListener(IPlatform platform, IUnityAdsLoadListener loadListener) : base("com.unity3d.ads.IUnityAdsLoadListener") {
            m_Platform = platform;
            m_ManagedListener = loadListener;
        }

        public void onUnityAdsAdLoaded(string placementId) {
            m_Platform?.UnityAdsReady(placementId);
            m_ManagedListener?.OnUnityAdsAdLoaded(placementId);
        }

        public void onUnityAdsFailedToLoad(string placementId, AndroidJavaObject error, string message) {
            m_Platform?.UnityAdsDidError($"Failed to load placement: {placementId}");
            m_ManagedListener?.OnUnityAdsFailedToLoad(placementId, EnumUtilities.GetEnumFromAndroidJavaObject(error, UnityAdsLoadError.UNKNOWN), message);
        }
    }
}
