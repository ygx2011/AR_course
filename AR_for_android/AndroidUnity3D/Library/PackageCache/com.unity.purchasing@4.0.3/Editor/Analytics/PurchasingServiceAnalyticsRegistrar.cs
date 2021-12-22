using UnityEditor;
using UnityEngine.Analytics;

namespace UnityEditor.Purchasing
{
    internal static class PurchasingServiceAnalyticsRegistrar
    {
        const int k_MaxEventsPerHour = 100;
        const int k_MaxItems = 10;
        const string k_VendorKey = "unity.services.core.editor";

        internal static AnalyticsResult RegisterEvent(EditorAnalyticsDataSignature eventSig)
        {
            return EditorAnalytics.RegisterEventWithLimit(eventSig.eventName, k_MaxEventsPerHour, k_MaxItems, k_VendorKey, eventSig.version);
        }
    }
}
