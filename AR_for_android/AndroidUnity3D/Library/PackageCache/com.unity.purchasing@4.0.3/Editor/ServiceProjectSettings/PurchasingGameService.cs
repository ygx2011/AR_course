#if SERVICES_SDK_CORE_ENABLED
using System;
using Unity.Services.Core.Editor;
using UnityEngine;

namespace UnityEditor.Purchasing
{
    internal class PurchasingGameService : IEditorGameService
    {
        const string k_ServiceName = "Purchasing";

        readonly PurchasingServiceEnabler m_Enabler;
        readonly PurchasingServiceIdentifier m_Identifier;

        public PurchasingGameService()
        {
            m_Enabler = new PurchasingServiceEnabler();
            m_Identifier = new PurchasingServiceIdentifier();
        }

        public void AddEnableAction(Action toAdd)
        {
            m_Enabler.OnServiceEnabled += toAdd;
        }
        public void RemoveEnableAction(Action toRemove)
        {
            m_Enabler.OnServiceEnabled -= toRemove;
        }
        public void AddDisableAction(Action toAdd)
        {
            m_Enabler.OnServiceDisabled += toAdd;
        }
        public void RemoveDisableAction(Action toRemove)
        {
            m_Enabler.OnServiceDisabled -= toRemove;
        }


        public string Name => k_ServiceName;

        public IEditorGameServiceIdentifier Identifier => m_Identifier;

        public bool RequiresCoppaCompliance => true;

        public bool HasDashboard => true;

        public string GetFormattedDashboardUrl()
        {
            return $"https://analytics.cloud.unity3d.com/projects/{CloudProjectSettings.projectId}/purchasing/";
        }

        public IEditorGameServiceEnabler Enabler => m_Enabler;
    }
}
#endif
