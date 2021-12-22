using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    class IMGUIContainerPopupAdapter
    {
        public int index;
        public string[] options;
        public Action<string> popupValueChangedAction { get; set; }

        public IMGUIContainer container
        {
            get => m_Container;
            set
            {
                m_Container = value;
                if (m_Container != null) m_Container.onGUIHandler = OnGUI;
            }
        }

        Rect m_Position;
        IMGUIContainer m_Container;

        void OnGUI()
        {
            Layout();

            var choice = DrawPopup();

            HandleChoice(choice);
        }

        void Layout()
        {
            if (m_Position.width == 0 || m_Position.height == 0)
            {
                m_Position = new Rect(0, 0, m_Container.resolvedStyle.width, m_Container
                    .resolvedStyle.height);
            }
        }

        int DrawPopup()
        {
            return EditorGUI.Popup(
                m_Position,
                index,
                options);
        }

        void HandleChoice(int choice)
        {
            if (choice != index)
            {
                index = choice;

                popupValueChangedAction(options[index]);
            }
        }
    }
}
