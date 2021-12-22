namespace UnityEditor.Purchasing
{
    static class GenericEditorDropdownSelectEventSenderHelpers
    {
        internal static void SendIapMenuSelectTargetStoreEvent(string value)
        {
            BuildAndSendEvent(EventComponents.k_ComponentBuild, EventTools.k_ToolIapMenu, EventUINames.k_UINameSelectTargetStore, value);
        }

        internal static void SendCatalogSetProductTypeEvent(string value)
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventUINames.k_UINameProductType, value);
        }

        internal static void SendCatalogSetPayoutTypeEvent(string value)
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventUINames.k_UINamePayoutType, value);
        }

        internal static void SendCatalogSetTranslationLocaleEvent(string value)
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventUINames.k_UINameTranslationLocale, value);
        }

        internal static void SendCatalogSetApplePriceTierEvent(string value)
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventUINames.k_UINameApplePriceTier, value);
        }

        static void BuildAndSendEvent(string component, string tool, string name, string value)
        {
            var newEvent = new GenericEditorSelectDropdownEvent(component, tool, name, value);
            PurchasingServiceAnalyticsSender.SendEvent(newEvent);
        }
    }
}
