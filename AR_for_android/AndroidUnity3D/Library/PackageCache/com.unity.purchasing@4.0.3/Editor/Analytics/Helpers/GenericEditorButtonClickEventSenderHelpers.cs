using UnityEngine;

namespace UnityEditor.Purchasing
{
    internal class GenericEditorButtonClickEventSenderHelpers
    {
        internal static void SendCatalogAddProductEvent()
        {
            BuildAndSendEventWithoutOption(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventActions.k_ActionAddProduct);
        }

        internal static void SendCatalogRemoveProductEvent()
        {
            BuildAndSendEventWithoutOption(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventActions.k_ActionRemoveProduct);
        }

        internal static void SendCatalogAddPayoutEvent()
        {
            BuildAndSendEventWithoutOption(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventActions.k_ActionAddPayout);
        }

        internal static void SendCatalogRemovePayoutEvent()
        {
            BuildAndSendEventWithoutOption(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventActions.k_ActionRemovePayout);
        }

        internal static void SendCatalogAddTranslationEvent()
        {
            BuildAndSendEventWithoutOption(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventActions.k_ActionAddTranslation);
        }

        internal static void SendCatalogRemoveTranslationEvent()
        {
            BuildAndSendEventWithoutOption(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventActions.k_ActionRemoveTranslation);
        }

        internal static void SendCatalogSelectAppleScreenshotEvent()
        {
            BuildAndSendEventWithoutOption(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventActions.k_ActionSelectAppleScreenshot);
        }

        internal static void SendCatalogAppStoreExportEvent(string option)
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventActions.k_ActionAppStoreExport, option);
        }

        internal static void SendCatalogSyncToUdpEvent()
        {
            BuildAndSendEventWithoutOption(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventActions.k_ActionSyncToUdp);
        }

        internal static void SendObfuscatorWindowObfuscateGooglePlayEventEvent()
        {
            BuildAndSendEventWithoutOption(EventComponents.k_ComponentObfuscation, EventTools.k_ToolObfuscatorWindow, EventActions.k_ActionObfuscationGooglePlayButton);
        }

        static void BuildAndSendEventWithoutOption(string component, string tool, string action)
        {
            BuildAndSendEvent(component, tool, action, null);
        }

        static void BuildAndSendEvent(string component, string tool, string action, string option)
        {
            var newEvent = new GenericEditorClickButtonEvent(component, tool, action, option);
            PurchasingServiceAnalyticsSender.SendEvent(newEvent);
        }
    }
}
