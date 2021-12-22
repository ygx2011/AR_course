#if UNITY_ANDROID
using System;
using UnityEngine.Advertisements.Purchasing;

namespace UnityEngine.Advertisements.Platform.Android
{
    internal class AndroidPlatform : AndroidJavaProxy, INativePlatform, IPurchasingEventSender
    {
        private IPlatform m_Platform;
        private AndroidJavaObject m_CurrentActivity;
        private AndroidJavaClass m_UnityAds;
        private IPurchase m_UnityAdsPurchase;
        private AndroidJavaClass m_Placement;
        public AndroidPlatform() : base("com.unity3d.ads.IUnityAdsListener") {}

        public void SetupPlatform(IPlatform platform)
        {
            m_Platform = platform;
            m_CurrentActivity = GetCurrentAndroidActivity();
            m_UnityAds = new AndroidJavaClass("com.unity3d.ads.UnityAds");
            m_Placement = new AndroidJavaClass("com.unity3d.services.ads.placement.Placement");
        }

        public void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad, IUnityAdsInitializationListener initializationListener)
        {
            m_UnityAdsPurchase = new Purchase();
            m_UnityAdsPurchase?.Initialize(this);
            m_UnityAds?.CallStatic("initialize", m_CurrentActivity, gameId, testMode, enablePerPlacementLoad, new AndroidInitializationListener(m_Platform, initializationListener));
        }

        public void Load(string placementId, IUnityAdsLoadListener loadListener)
        {
            m_UnityAds?.CallStatic("load", placementId, new AndroidLoadListener(m_Platform, loadListener));
        }

        public void Show(string placementId, IUnityAdsShowListener showListener)
        {
            m_UnityAds?.CallStatic("show", m_CurrentActivity, placementId, new AndroidShowListener(m_Platform, showListener));
        }

        public void SetMetaData(MetaData metaData)
        {
            var metaDataObject = new AndroidJavaObject("com.unity3d.ads.metadata.MetaData", m_CurrentActivity);
            metaDataObject.Call("setCategory", metaData.category);
            foreach (var entry in metaData.Values())
            {
                metaDataObject.Call<bool>("set", entry.Key, entry.Value);
            }
            metaDataObject.Call("commit");
        }

        public bool GetDebugMode()
        {
            return m_UnityAds?.CallStatic<bool>("getDebugMode") ?? false;
        }

        public void SetDebugMode(bool debugMode)
        {
            m_UnityAds?.CallStatic("setDebugMode", debugMode);
        }

        public string GetVersion()
        {
            return m_UnityAds?.CallStatic<string>("getVersion") ?? "UnknownVersion";
        }

        public bool IsInitialized()
        {
            return m_UnityAds?.CallStatic<bool>("isInitialized") ?? false;
        }

        public bool IsReady(string placementId)
        {
            return placementId == null ? m_UnityAds?.CallStatic<bool>("isReady") ?? false : m_UnityAds?.CallStatic<bool>("isReady", placementId) ?? false;
        }

        internal void RemoveListener()
        {
            m_UnityAds?.CallStatic("removeListener", this);
        }

        public PlacementState GetPlacementState(string placementId)
        {
            var rawPlacementState = placementId == null ? m_UnityAds.CallStatic<AndroidJavaObject>("getPlacementState") : m_UnityAds.CallStatic<AndroidJavaObject>("getPlacementState", placementId);
            return (PlacementState)rawPlacementState.Call<int>("ordinal");
        }

        public string GetDefaultPlacement()
        {
            return m_Placement?.CallStatic<string>("getDefaultPlacement");
        }

        public static AndroidJavaObject GetCurrentAndroidActivity()
        {
            var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            return unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        void IPurchasingEventSender.SendPurchasingEvent(string payload)
        {
            m_UnityAdsPurchase?.SendEvent(payload);
        }
    }
}
#endif
