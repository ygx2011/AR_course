using System;

namespace UnityEditor.Purchasing
{
    internal class GenericEditorSelectDropdownEvent : IEditorAnalyticsEvent
    {
        readonly string m_Component;
        readonly string m_Tool;
        readonly string m_Name;
        readonly string m_Value;

        internal GenericEditorSelectDropdownEvent(string component, string tool, string name, string value)
        {
            m_Component = component;
            m_Tool = tool;
            m_Name = name;
            m_Value = value;
        }

        public virtual EditorAnalyticsDataSignature GetSignature()
        {
            return SignatureDefinitions.k_EditorSelectDropdownSignature;
        }

        [Serializable]
        public struct GenericEditorSelectDropdownParams
        {
            //Important: These param names come from the DevEx core. Do not change/add/remove them until this event changes version
            public string component;
            public string tool;
            public string name;
            public string value;
            public string platform;
        }

        public object CreateEventParams(string platformName)
        {
            return new GenericEditorSelectDropdownParams
            {
                component = m_Component,
                tool = m_Tool,
                name = m_Name,
                value = m_Value,
                platform = platformName
            };
        }
    }
}
