namespace UnityEditor.Purchasing
{
    static class GenericEditorFieldEditEventSenderHelpers
    {
        internal static void SendCatalogEditEvent(string fieldName)
        {
            BuildAndSendEvent(EventComponents.k_ComponentCodeless, EventTools.k_ToolCatalog, fieldName);
        }

        static void BuildAndSendEvent(string component, string action, string name)
        {
            var newEvent = new GenericEditorEditFieldEvent(component, action, name);
            PurchasingServiceAnalyticsSender.SendEvent(newEvent);
        }

    }
}
