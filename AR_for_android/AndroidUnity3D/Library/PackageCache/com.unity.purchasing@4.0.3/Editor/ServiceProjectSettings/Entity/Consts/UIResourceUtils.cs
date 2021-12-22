namespace UnityEditor.Purchasing
{
    static class UIResourceUtils
    {
        internal static readonly string purchasingServicesRootUxmlPath = $"{SettingsUIConstants.packageUxmlRoot}/PurchasingProjectSettings.uxml";

        internal static readonly string labelUxmlPath = $"{SettingsUIConstants.packageUxmlRoot}/Label.uxml";

        internal static readonly string catalogUxmlPath = $"{SettingsUIConstants.packageUxmlRoot}/CatalogEditor.uxml";
        internal static readonly string platformSupportUxmlPath = $"{SettingsUIConstants.packageUxmlRoot}/PlatformSupportVisual.uxml";
        internal static readonly string googlePlayConfigUxmlPath = $"{SettingsUIConstants.packageUxmlRoot}/GooglePlayConfiguration.uxml";
        internal static readonly string appleConfigUxmlPath = $"{SettingsUIConstants.packageUxmlRoot}/AppleConfiguration.uxml";
        internal static readonly string analyticsNoticeUxmlPath = $"{SettingsUIConstants.packageUxmlRoot}/AnalyticsNotice.uxml";

        internal static readonly string platformSupportCommonUssPath = $"{SettingsUIConstants.packageUssRoot}/PlatformSupportVisualCommon.uss";
        internal static readonly string platformSupportDarkUssPath = $"{SettingsUIConstants.packageUssRoot}/PlatformSupportVisualDark.uss";
        internal static readonly string platformSupportLightUssPath = $"{SettingsUIConstants.packageUssRoot}/PlatformSupportVisualLight.uss";
        internal static readonly string purchasingCommonUssPath = $"{SettingsUIConstants.packageUssRoot}/ServicesProjectSettingsCommon.uss";
        internal static readonly string purchasingDarkUssPath = $"{SettingsUIConstants.packageUssRoot}/ServicesProjectSettingsDark.uss";
        internal static readonly string purchasingLightUssPath = $"{SettingsUIConstants.packageUssRoot}/ServicesProjectSettingsLight.uss";
    }
}
