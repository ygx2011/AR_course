using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    internal class AnalyticsNoticeBlock : IPurchasingSettingsUIBlock
    {
        const string k_EnabledNoticeSectionName = "EnabledNoticeSection";
        const string k_DisabledNoticeSectionName = "DisabledNoticeSection";

        private string m_ActiveSectionName;

        VisualElement m_NoticeBlock;

        internal static AnalyticsNoticeBlock CreateEnabledAnalyticsBlock()
        {
            return new AnalyticsNoticeBlock(k_EnabledNoticeSectionName);
        }

        internal static AnalyticsNoticeBlock CreateDisabledAnalyticsBlock()
        {
            return new AnalyticsNoticeBlock(k_DisabledNoticeSectionName);
        }

        private AnalyticsNoticeBlock(string activeSection)
        {
            m_ActiveSectionName = activeSection;
        }

        public VisualElement GetUIBlockElement()
        {
            return SetupConfigBlock();
        }

        VisualElement SetupConfigBlock()
        {
            m_NoticeBlock = SettingsUIUtils.CloneUIFromTemplate(UIResourceUtils.analyticsNoticeUxmlPath);

            SetupNoticeBlock();
            SetupStyleSheets();

            return m_NoticeBlock;
        }

        void SetupNoticeBlock()
        {
            ToggleStateSectionVisibility(k_EnabledNoticeSectionName);
            ToggleStateSectionVisibility(k_DisabledNoticeSectionName);
        }

        void ToggleStateSectionVisibility(string sectionName)
        {
            var errorSection = m_NoticeBlock.Q(sectionName);
            if (errorSection != null)
            {
                errorSection.style.display = (sectionName == m_ActiveSectionName)
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            }
        }

        void SetupStyleSheets()
        {
            m_NoticeBlock.AddStyleSheetPath(UIResourceUtils.purchasingCommonUssPath);
            m_NoticeBlock.AddStyleSheetPath(EditorGUIUtility.isProSkin ? UIResourceUtils.purchasingDarkUssPath : UIResourceUtils.purchasingLightUssPath);
        }
    }
}
