using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor.Purchasing
{
    class SwitchStoreEditorWindow : EditorWindow
    {
#if UNITY_2019
        /// <summary>
        /// <seealso cref="https://unity3d.com/unity/whats-new/2019.4.15"/>
        /// </summary>
        /// <returns>true if API is missing</returns>
        static bool IsMissingCreateGUI()
        {
            var matches = Regex.Match(Application.unityVersion, @"^(\d+)\.(\d+)\.(\d+)");
            if (!matches.Success)
            {
                return false;
            }

            var major = int.Parse(matches.Groups[1].Value);
            var minor = int.Parse(matches.Groups[2].Value);
            var patch = int.Parse(matches.Groups[3].Value);

            return major <= 2019 && minor <= 4 && patch <= 14;
        }

        void OnEnable()
        {
            if (IsMissingCreateGUI())
            {
                CreateGUI();
            }
        }
#endif

        void CreateGUI()
        {
            var settingsBlock = PlatformsAndStoresServiceSettingsBlock.CreateStateSpecificBlock(true);
            var settingsElement = settingsBlock.GetUIBlockElement();

            rootVisualElement.Add(settingsElement);
        }
    }
}
