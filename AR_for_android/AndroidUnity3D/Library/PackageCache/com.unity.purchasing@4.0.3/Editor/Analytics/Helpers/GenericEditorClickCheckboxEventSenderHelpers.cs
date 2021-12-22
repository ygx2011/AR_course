namespace UnityEditor.Purchasing
{
    static class GenericEditorClickCheckboxEventSenderHelpers
    {
        internal static void SendCatalogAutoInitToggleEvent(bool value)
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, EventUINames.k_UINameAutoInit, value);
        }

        static void BuildAndSendEvent(string component, string tool, string name, bool value)
        {
            var newEvent = new GenericEditorClickCheckboxEvent(component, tool, name, value);
            PurchasingServiceAnalyticsSender.SendEvent(newEvent);
        }
    }
}
