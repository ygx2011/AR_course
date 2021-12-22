using System;
using System.Collections.Generic;

namespace UnityEngine.Purchasing
{
    internal class UIFakeStoreDropdown
    {
        List<string> m_Options;
        Action<int, string> m_OnDropdown;

        Vector2 scrollPosition = new Vector2();

        public void DoPopup(int windowID)
        {
            if (m_Options != null)
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUI.skin.horizontalScrollbar,
                    GUI.skin.verticalScrollbar, GUI.skin.box);

                int optionsCount = m_Options.Count;
                for (int index = 0; index < optionsCount; index++)
                {
                    if (GUILayout.Button(m_Options[index]))
                    {
                        OnOptionSelected(index);
                    }
                }

                GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("Error! No Dropdown options set!");
            }
        }

        void OnOptionSelected(int optionIndex)
        {
            m_OnDropdown?.Invoke(optionIndex, m_Options[optionIndex]);
        }

        internal void SetOptions(List<string> options)
        {
            m_Options = new List<string>(options);
        }

        internal void SetSelectionAction(Action<int, string> onDropdown)
        {
            m_OnDropdown = onDropdown;
        }
    }
}
