namespace UnityEditor.Purchasing
{
    internal static class SignatureDefinitions
    {
        internal static readonly EditorAnalyticsDataSignature k_GenericEditorSignature = new EditorAnalyticsDataSignature()
        {
            eventName = "editorgameserviceeditor",
            version = 1
        };

        internal static readonly EditorAnalyticsDataSignature k_EditorClickButtonSignature = new EditorAnalyticsDataSignature()
        {
            eventName = "editor_click_button",
            version = 1
        };

        internal static readonly EditorAnalyticsDataSignature k_EditorClickMenuItemSignature = new EditorAnalyticsDataSignature()
        {
            eventName = "editor_click_menu_item",
            version = 1
        };

        internal static readonly EditorAnalyticsDataSignature k_EditorEditFieldSignature = new EditorAnalyticsDataSignature()
        {
            eventName = "editor_edit_field",
            version = 1
        };

        internal static readonly EditorAnalyticsDataSignature k_EditorSelectDropdownSignature = new EditorAnalyticsDataSignature()
        {
            eventName = "editor_select_dropdown",
            version = 1
        };

        internal static readonly EditorAnalyticsDataSignature k_EditorClickCheckboxSignature = new EditorAnalyticsDataSignature()
        {
            eventName = "editor_click_checkbox",
            version = 1
        };
    }
}
