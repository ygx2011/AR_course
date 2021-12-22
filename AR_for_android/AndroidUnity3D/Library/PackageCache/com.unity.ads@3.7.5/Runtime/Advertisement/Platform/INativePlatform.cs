using UnityEngine.Advertisements.Platform;

namespace UnityEngine.Advertisements
{
    internal interface INativePlatform
    {
        void SetupPlatform(IPlatform platform);
        void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad, IUnityAdsInitializationListener initializationListener);
        void Load(string placementId, IUnityAdsLoadListener loadListener);
        void Show(string placementId, IUnityAdsShowListener showListener);
        void SetMetaData(MetaData metaData);
        bool GetDebugMode();
        void SetDebugMode(bool debugMode);
        string GetVersion();
        bool IsInitialized();
        bool IsReady(string placementId);
        PlacementState GetPlacementState(string placementId);

        string GetDefaultPlacement();
    }
}
