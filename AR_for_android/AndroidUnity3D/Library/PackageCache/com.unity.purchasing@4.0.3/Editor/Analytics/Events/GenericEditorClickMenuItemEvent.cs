using System;

namespace UnityEditor.Purchasing
{
    internal class GenericEditorClickMenuItemEvent : IEditorAnalyticsEvent
    {
        readonly string m_Component;
        readonly string m_Source;
        readonly string m_Action;

        internal GenericEditorClickMenuItemEvent(string component, string source, string action)
        {
            m_Component = component;
            m_Source = source;
            m_Action = action;
        }

        public virtual EditorAnalyticsDataSignature GetSignature()
        {
            return SignatureDefinitions.k_EditorClickMenuItemSignature;
        }

        [Serializable]
        public struct GenericEditorClickMenuItemParams
        {
            //Important: These param names come from the DevEx core. Do not change/add/remove them until this event changes version
            public string action;
            public string source;
            public string component;
            public string platform;
        }

        public object CreateEventParams(string platformName)
        {
            return new GenericEditorClickMenuItemParams
            {
                action = m_Action,
                source = m_Source,
                component = m_Component,
                platform = platformName
            };
        }
    }
}
