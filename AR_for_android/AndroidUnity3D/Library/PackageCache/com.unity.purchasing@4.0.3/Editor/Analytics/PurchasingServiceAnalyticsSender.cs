using System;

namespace UnityEditor.Purchasing
{
    [InitializeOnLoad]
    internal static class PurchasingServiceAnalyticsSender
    {
        static PurchasingServiceAnalyticsSender()
        {
            RegisterEvents();
        }

        static void RegisterEvents()
        {
            PurchasingServiceAnalyticsRegistrar.RegisterEvent(SignatureDefinitions.k_GenericEditorSignature);
            PurchasingServiceAnalyticsRegistrar.RegisterEvent(SignatureDefinitions.k_EditorClickButtonSignature);
            PurchasingServiceAnalyticsRegistrar.RegisterEvent(SignatureDefinitions.k_EditorClickCheckboxSignature);
            PurchasingServiceAnalyticsRegistrar.RegisterEvent(SignatureDefinitions.k_EditorClickMenuItemSignature);
            PurchasingServiceAnalyticsRegistrar.RegisterEvent(SignatureDefinitions.k_EditorEditFieldSignature);
            PurchasingServiceAnalyticsRegistrar.RegisterEvent(SignatureDefinitions.k_EditorSelectDropdownSignature);
        }

        internal static void SendEvent(IEditorAnalyticsEvent eventToSend)
        {
            SendEventInternal(eventToSend.GetSignature(), eventToSend.CreateEventParams(GetPlatform()));
        }

        static string GetPlatform()
        {
            return Enum.GetName(typeof(BuildTarget), EditorUserBuildSettings.activeBuildTarget);
        }

        static void SendEventInternal(EditorAnalyticsDataSignature eventSignature, object eventStruct)
        {
            EditorAnalytics.SendEventWithLimit(eventSignature.eventName, eventStruct, eventSignature.version);
        }
    }
}
