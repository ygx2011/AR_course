#if SERVICES_SDK_CORE_ENABLED
using System;
using Unity.Services.Core.Editor;
using UnityEditor;
using UnityEditor.Advertisements;

namespace UnityEngine.Advertisements.Editor
{
    class AdsService : IEditorGameService
    {
        public event Action GameIdsUpdated;

        public AdsService()
        {
            ((EditorGameServiceFlagEnabler)Enabler).ServiceFlagRequestComplete += FetchMissingGameIdsIfPossible;
        }

        void FetchMissingGameIdsIfPossible()
        {
            if (!AdvertisementSettings.enabled
                || string.IsNullOrEmpty(CloudProjectSettings.projectId))
            {
                return;
            }

            var iosGameId = AdvertisementSettings.GetGameId(RuntimePlatform.IPhonePlayer);
            var androidGameId = AdvertisementSettings.GetGameId(RuntimePlatform.Android);
            if (string.IsNullOrEmpty(iosGameId)
                || string.IsNullOrEmpty(androidGameId))
            {
                new RequestGameIds().SendWithRetry(OnRequestGameIdsCompletedSuccess, OnRequestGameIdsCompletedError);
            }
        }

        void OnRequestGameIdsCompletedSuccess(RequestGameIds.Response response)
        {
            SetGameIds(response);
        }

        void OnRequestGameIdsCompletedError(Exception exception)
        {
            Debug.LogException(exception);
        }

        void SetGameIds(RequestGameIds.Response gameIds)
        {
            AdvertisementSettings.SetGameId(RuntimePlatform.IPhonePlayer, gameIds.iOSGameKey);
            AdvertisementSettings.SetGameId(RuntimePlatform.Android, gameIds.androidGameKey);

            GameIdsUpdated?.Invoke();
        }

        ~AdsService()
        {
            if (Enabler is EditorGameServiceFlagEnabler adsServiceEnabler)
            {
                adsServiceEnabler.ServiceFlagRequestComplete -= FetchMissingGameIdsIfPossible;
            }
        }

        public string Name => "Ads";

        public IEditorGameServiceIdentifier Identifier => new AdsServiceIdentifier();

        public bool RequiresCoppaCompliance => true;

        public bool HasDashboard => true;

        public string GetFormattedDashboardUrl()
        {
#if ENABLE_EDITOR_GAME_SERVICES
            return AdsDashboardUrls.GetOverviewUrl();
#else
            return string.Empty;
#endif
        }

        public IEditorGameServiceEnabler Enabler { get; } = new AdsServiceEnabler();
    }
}
#endif
