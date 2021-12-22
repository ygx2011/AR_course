using System;

namespace UnityEditor.Purchasing
{
    internal class GenericEditorGameServiceEvent : IEditorAnalyticsEvent
    {
        readonly string m_Component;
        readonly string m_Action;

        internal GenericEditorGameServiceEvent(string component, string action)
        {
            m_Component = component;
            m_Action = action;
        }

        public EditorAnalyticsDataSignature GetSignature()
        {
            return SignatureDefinitions.k_GenericEditorSignature;
        }

        [Serializable]
        public struct GenericEditorGameServiceEventParams
        {
            //Important: These param names come from the DevEx core. Do not change/add/remove them until this event changes version
            public string action;
            public string component;
            public string package;
        }

        public object CreateEventParams(string packageKey)
        {
            return new GenericEditorGameServiceEventParams
            {
                action = m_Action,
                component = m_Component,
                package = packageKey
            };
        }
    }
}
