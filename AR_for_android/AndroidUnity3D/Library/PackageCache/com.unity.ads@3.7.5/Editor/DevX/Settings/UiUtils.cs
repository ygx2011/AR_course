#if SERVICES_SDK_CORE_ENABLED
using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace UnityEngine.Advertisements.Editor
{
    static class UiUtils
    {
        public static VisualElement GetUiFromTemplate(string templatePath)
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(templatePath);
            if (template == null)
            {
                return null;
            }

            return template.CloneTree().contentContainer;
        }

        public static void AddOnClickedForElement(this VisualElement self, Action onClicked, string elementName)
        {
            var link = self.Q(elementName);
            if (link is null)
            {
                return;
            }

            var clickable = new Clickable(onClicked);
            link.AddManipulator(clickable);
        }
    }
}
#endif
