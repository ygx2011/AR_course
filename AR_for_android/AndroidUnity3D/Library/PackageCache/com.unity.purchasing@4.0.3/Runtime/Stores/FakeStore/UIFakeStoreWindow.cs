using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Purchasing
{
    internal class UIFakeStoreWindow : MonoBehaviour
    {
        string m_QueryText;
        string m_OkText;
        string m_CancelText;
        string m_LastSelectedOptionText;

        Action m_OnOk;
        Action m_OnCancel;
        Action<int> m_OnDropdown;

        bool m_CancelEnabled;
        bool m_DropdownEnabled;

        bool m_DoDropdown;

        UIFakeStoreDropdown m_Dropdown = new UIFakeStoreDropdown();

        Vector2 scrollPosition = new Vector2();

        const float k_MenuScreenRatio = 0.6f;

        void OnGUI()
        {
            var windowRect = CreateCenteredWindowRect();
            if (m_DoDropdown)
            {
                GUI.Window(1, windowRect, m_Dropdown.DoPopup, "Select Response");
            }
            else
            {
                GUI.Window(0, windowRect, this.DoMainGUI, "Fake Store");
            }
        }

        Rect CreateCenteredWindowRect()
        {
            float windowWidth = Screen.width * k_MenuScreenRatio;
            float windowHeight = Screen.height * k_MenuScreenRatio;
            float centeredX = (Screen.width - windowWidth) / 2f;
            float centeredY = (Screen.height - windowHeight) / 2f;

            return new Rect(centeredX, centeredY, windowWidth, windowHeight);
        }

        void DoMainGUI(int windowID)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUI.skin.horizontalScrollbar,
                GUI.skin.verticalScrollbar, GUI.skin.box);

            GUILayout.Label(m_QueryText);

            if (m_DropdownEnabled)
            {
                if (GUILayout.Button(m_LastSelectedOptionText))
                {
                    DoDropDown();
                }
            }

            if (GUILayout.Button(m_OkText))
            {
                OnOkClicked();
            }

            if (m_CancelEnabled)
            {
                if (GUILayout.Button(m_CancelText))
                {
                    OnCancelClicked();
                }
            }

            GUILayout.EndScrollView();
        }

        void DoDropDown()
        {
            m_DoDropdown = true;
        }

        void OnOkClicked()
        {
            m_OnOk?.Invoke();
        }

        void OnCancelClicked()
        {
            m_OnCancel?.Invoke();
        }

        internal void ConfigureMainDialogText(string queryText, string okText, string cancelText)
        {
            m_QueryText = queryText;
            m_OkText = okText;
            m_CancelText = cancelText;
        }

        internal void ConfigureDropdownOptions(List<string> options)
        {
            m_Dropdown.SetOptions(options);
            m_Dropdown.SetSelectionAction(this.OnDropdown);

            if (options.Count > 0)
            {
                m_LastSelectedOptionText = options.First();
            }
        }

        void OnDropdown(int index, string selectionText)
        {
            m_LastSelectedOptionText = selectionText;
            m_OnDropdown?.Invoke(index);
            m_DoDropdown = false;
        }

        internal void AssignCallbacks(Action onOk, Action onCancel, Action<int> onDropdown)
        {
            m_OnOk = onOk;

            m_CancelEnabled = (onCancel != null);
            m_OnCancel = onCancel;

            m_DropdownEnabled = (onDropdown != null);
            m_OnDropdown = onDropdown;
        }
    }
}
