using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    class AppleConfigurationSettingsBlock : IPurchasingSettingsUIBlock
    {
        VisualElement m_AppleConfigurationBlock;

        AppleObfuscatorSection m_ObfuscatorSection;
        VisualElement m_ConfigurationBlock;

        string m_AppleErrorMessage;
        string m_GoogleErrorMessage;

        internal AppleConfigurationSettingsBlock()
        {
            m_ObfuscatorSection = new AppleObfuscatorSection();
        }

        public VisualElement GetUIBlockElement()
        {
            return SetupConfigBlock();
        }

        VisualElement SetupConfigBlock()
        {
            m_ConfigurationBlock = SettingsUIUtils.CloneUIFromTemplate(UIResourceUtils.appleConfigUxmlPath);

            m_ObfuscatorSection.SetupObfuscatorBlock(m_ConfigurationBlock);
            SetupStyleSheets();

            return m_ConfigurationBlock;
        }

        void SetupStyleSheets()
        {
            m_ConfigurationBlock.AddStyleSheetPath(UIResourceUtils.purchasingCommonUssPath);
            m_ConfigurationBlock.AddStyleSheetPath(EditorGUIUtility.isProSkin ? UIResourceUtils.purchasingDarkUssPath : UIResourceUtils.purchasingLightUssPath);
        }
    }
}
