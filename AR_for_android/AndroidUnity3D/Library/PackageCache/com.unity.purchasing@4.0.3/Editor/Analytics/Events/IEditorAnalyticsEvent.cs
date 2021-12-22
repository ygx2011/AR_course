namespace UnityEditor.Purchasing
{
    internal interface IEditorAnalyticsEvent
    {
        EditorAnalyticsDataSignature GetSignature();
        object CreateEventParams(string platformName);
    }
}
