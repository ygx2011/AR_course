using System;

namespace UnityEditor.Purchasing
{
    internal class GenericEditorEditFieldEvent : IEditorAnalyticsEvent
    {
        readonly string m_Component;
        readonly string m_Tool;
        readonly string m_FieldName;

        internal GenericEditorEditFieldEvent(string component, string tool, string fieldName)
        {
            m_Component = component;
            m_Tool = tool;
            m_FieldName = fieldName;
        }

        public virtual EditorAnalyticsDataSignature GetSignature()
        {
            return SignatureDefinitions.k_EditorEditFieldSignature;
        }

        [Serializable]
        public struct GenericEditorEditFieldParams
        {
            //Important: These param names come from the DevEx core. Do not change/add/remove them until this event changes version
            public string tool;
            public string component;
            public string name;
            public string platform;
        }

        public object CreateEventParams(string platformName)
        {
            return new GenericEditorEditFieldParams
            {
                tool = m_Tool,
                component = m_Component,
                name = m_FieldName,
                platform = platformName
            };
        }
    }
}
