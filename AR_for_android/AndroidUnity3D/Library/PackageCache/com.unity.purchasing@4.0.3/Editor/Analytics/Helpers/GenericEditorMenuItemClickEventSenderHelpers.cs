namespace UnityEditor.Purchasing
{
    internal static class GenericEditorMenuItemClickEventSenderHelpers
    {
        internal static void SendGameObjectMenuAddIapButtonEvent()
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless, EventSources.k_SourceGameObjectMenu, EventActions.k_ActionAddIapButton);
        }

        internal static void SendIapMenuAddIapButtonEvent()
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless, EventSources.k_SourceIapMenu, EventActions.k_ActionAddIapButton);
        }

        internal static void SendGameObjectMenuAddIapListenerEvent()
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless,EventSources.k_SourceGameObjectMenu, EventActions.k_ActionAddIapListener);
        }

        internal static void SendIapMenuAddIapListenerEvent()
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless, EventSources.k_SourceIapMenu, EventActions.k_ActionAddIapListener);
        }

        internal static void SendTopMenuOpenCatalogEvent()
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless, EventSources.k_SourceIapMenu, EventActions.k_ActionOpenCatalog);
        }

        internal static void SendIapMenuOpenObfuscatorEvent()
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless, EventSources.k_SourceIapMenu, EventActions.k_ActionObfuscationOpenWindow);
        }

        static void BuildAndSendEvent(string component, string source, string action)
        {
            var newEvent = new GenericEditorClickMenuItemEvent(component, source, action);
            PurchasingServiceAnalyticsSender.SendEvent(newEvent);
        }
    }
}
