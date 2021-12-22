#if SERVICES_SDK_CORE_ENABLED
using UnityEditor;

namespace UnityEngine.Advertisements.Editor
{
    /// <summary>
    /// Helper to store all constants used in Ads service UI.
    /// </summary>
    /// <remarks>
    /// This helper has been created to avoid adding too much noise in UI classes.
    /// </remarks>
    static class UiConstants
    {
        public static class ClassNames
        {
            public const string Ads = "ads";
        }

        public static class StyleSheetPaths
        {
            public const string Common = "Packages/com.unity.ads/Editor/DevX/StyleSheets/AdsService.uss";
        }

        public static class UiTemplatePaths
        {
            public const string GameIds = "Packages/com.unity.ads/Editor/DevX/UXML/AdsServiceGameIds.uxml";

            public const string GettingStarted = "Packages/com.unity.ads/Editor/DevX/UXML/AdsServiceGettingStarted.uxml";

            public const string TestMode = "Packages/com.unity.ads/Editor/DevX/UXML/AdsServiceTestMode.uxml";
        }

        public static class UiElementNames
        {
            public const string TestModeToggle = "ToggleTestMode";

            public const string LearnMoreLink = "LearnMore";

            public const string AppleGameId = "AppleGameId";

            public const string AndroidGameId = "AndroidGameId";
        }

        public static class LocalizedStrings
        {
            public static readonly string Ads = L10n.Tr("Ads");

            public static readonly string Description = L10n.Tr("Monetize your games");

            public static readonly string AssetStorePackageInstalledMessage = L10n.Tr(
                "The Asset Store Package is installed.\n" +
                "Usage of Package Manager is recommended.");

            public static readonly string Unavailable = L10n.Tr("N/A");

            public static readonly string Yes = L10n.Tr("Yes");

            public static readonly string No = L10n.Tr("No");
        }

        public static class Formats
        {
            public const string TemplateNotFound = "No UI template found for Ads Service {0}.";
        }

        public static class Urls
        {
            public const string LearnMore = "https://unityads.unity3d.com/help/index";
        }

        public static readonly string[] SupportedPlatforms =
        {
            "Android",
            "iOS"
        };
    }
}
#endif
