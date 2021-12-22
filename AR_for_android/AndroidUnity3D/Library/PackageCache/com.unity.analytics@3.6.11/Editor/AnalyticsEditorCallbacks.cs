

using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEditor.Web;

namespace UnityEditor.Analytics
{
    [InitializeOnLoad]
    class AnalyticsEventTrackerEditorCallbacks
    {
        static AnalyticsEventTrackerEditorCallbacks()
        {
			bool useCEFServiceMenu = true;
			#if UNITY_2020_1_OR_NEWER
			useCEFServiceMenu = false;
			#endif
            AnalyticsEventTrackerEditor.SetServiceMenuDelegate(useCEFServiceMenu);
        }
    }
}
