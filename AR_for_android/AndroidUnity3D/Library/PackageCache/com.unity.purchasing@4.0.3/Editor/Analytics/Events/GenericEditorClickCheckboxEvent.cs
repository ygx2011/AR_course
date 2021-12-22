using System;

namespace UnityEditor.Purchasing
{
    internal class GenericEditorClickCheckboxEvent : IEditorAnalyticsEvent
    {
        readonly string m_Component;
        readonly string m_Tool;
        readonly string m_Name;
        readonly bool m_Value;

        internal GenericEditorClickCheckboxEvent(string component, string tool, string name, bool value)
        {
            m_Component = component;
            m_Tool = tool;
            m_Name = name;
            m_Value = value;
        }

        public virtual EditorAnalyticsDataSignature GetSignature()
        {
            return SignatureDefinitions.k_EditorClickCheckboxSignature;
        }

        [Serializable]
        public struct GenericEditorClickCheckboxParams
        {
            //Important: These param names come from the DevEx core. Do not change/add/remove them until this event changes version
            public string name;
            public string tool;
            public string component;
            public bool value;
            public string platform;
        }

        public object CreateEventParams(string platformName)
        {
            return new GenericEditorClickCheckboxParams
            {
                name = m_Name,
                tool = m_Tool,
                component = m_Component,
                value = m_Value,
                platform = platformName
            };
        }
    }
}
