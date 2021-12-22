using System;

namespace UnityEditor.Purchasing
{
    internal class GenericEditorClickButtonEvent : IEditorAnalyticsEvent
    {
        readonly string m_Component;
        readonly string m_Tool;
        readonly string m_Action;
        readonly string m_Option;

        internal GenericEditorClickButtonEvent(string component, string tool, string action, string option)
        {
            m_Component = component;
            m_Tool = tool;
            m_Action = action;
            m_Option = option;
        }

        public virtual EditorAnalyticsDataSignature GetSignature()
        {
            return SignatureDefinitions.k_EditorClickButtonSignature;
        }

        [Serializable]
        public struct GenericEditorClickButtonParams
        {
            //Important: These param names come from the DevEx core. Do not change/add/remove them until this event changes version
            public string action;
            public string tool;
            public string component;
            public string option;
            public string platform;
        }

        public object CreateEventParams(string platformName)
        {
            return new GenericEditorClickButtonParams
            {
                action = m_Action,
                tool = m_Tool,
                component = m_Component,
                option = m_Option,
                platform = platformName
            };
        }
    }
}
