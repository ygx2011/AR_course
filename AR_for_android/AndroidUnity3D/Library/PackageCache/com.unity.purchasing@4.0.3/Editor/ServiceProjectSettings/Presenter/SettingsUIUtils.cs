using System;
using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    static class SettingsUIUtils
    {
        public static VisualElement CloneUIFromTemplate(string templatePath)
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(templatePath);
            if (template == null)
            {
                return null;
            }

            return template.CloneTree().contentContainer;
        }
    }
}
