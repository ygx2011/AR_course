#if SERVICES_SDK_CORE_ENABLED && ENABLE_EDITOR_GAME_SERVICES
using UnityEditor;

namespace UnityEngine.Advertisements.Editor
{
    static class AdsServiceTopMenu
    {
        const int k_ConfigureMenuPriority = 100;
        const string k_ServiceMenuRoot = "Services/Ads/";

        [MenuItem(k_ServiceMenuRoot + "Configure", priority = k_ConfigureMenuPriority)]
        static void ShowProjectSettings()
        {
            EditorGameServiceAnalyticsSender.SendTopMenuConfigureEvent();
            var path = AdsSettingsProvider.GetSettingsPath();
            SettingsService.OpenProjectSettings(path);
        }
    }
}
#endif
