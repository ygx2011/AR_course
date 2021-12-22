using System;
using UnityEngine.Advertisements.Platform;
using UnityEngine.Advertisements.Utilities;

namespace UnityEngine.Advertisements {
    internal class AndroidShowListener : AndroidJavaProxy {
        private IPlatform m_Platform;
        private IUnityAdsShowListener m_ManagedListener;

        public AndroidShowListener(IPlatform platform, IUnityAdsShowListener showListener) : base("com.unity3d.ads.IUnityAdsShowListener") {
            m_Platform = platform;
            m_ManagedListener = showListener;
        }

        public void onUnityAdsShowFailure(string placementId, AndroidJavaObject error, string message) {
            m_Platform?.UnityAdsDidError(message);
            m_ManagedListener?.OnUnityAdsShowFailure(placementId, EnumUtilities.GetEnumFromAndroidJavaObject(error, UnityAdsShowError.UNKNOWN), message);
        }

        public void onUnityAdsShowStart(string placementId) {
            m_Platform?.UnityAdsDidStart(placementId);
            m_ManagedListener?.OnUnityAdsShowStart(placementId);
        }

        public void onUnityAdsShowClick(string placementId) {
            m_ManagedListener?.OnUnityAdsShowClick(placementId);
        }

        public void onUnityAdsShowComplete(string placementId, AndroidJavaObject state) {
            var showCompletionState = EnumUtilities.GetEnumFromAndroidJavaObject(state, UnityAdsShowCompletionState.UNKNOWN);
            m_Platform?.UnityAdsDidFinish(placementId, EnumUtilities.GetShowResultsFromCompletionState(showCompletionState));
            m_ManagedListener?.OnUnityAdsShowComplete(placementId, showCompletionState);
        }
    }
}
