using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    internal class IapCatalogServiceSettingsBlock : IPurchasingSettingsUIBlock
    {

        const string k_OpenCatalogBtn = "CatalogEditorButton";

        VisualElement m_CatalogBlock;


        public VisualElement GetUIBlockElement()
        {
            return SetupObfuscatorBlock();
        }

        VisualElement SetupObfuscatorBlock()
        {
            m_CatalogBlock = SettingsUIUtils.CloneUIFromTemplate(UIResourceUtils.catalogUxmlPath);

            SetupStyleSheets();
            PopulateConfigBlock();

            return m_CatalogBlock;
        }

        void SetupStyleSheets()
        {
            m_CatalogBlock.AddStyleSheetPath(UIResourceUtils.purchasingCommonUssPath);
            m_CatalogBlock.AddStyleSheetPath(EditorGUIUtility.isProSkin ? UIResourceUtils.purchasingDarkUssPath : UIResourceUtils.purchasingLightUssPath);
        }

        void PopulateConfigBlock()
        {
            SetupButtonActions();
        }

        void SetupButtonActions()
        {
            m_CatalogBlock.Q<Button>(k_OpenCatalogBtn).clicked += OpenCatalog;
        }

        void OpenCatalog()
        {
            ProductCatalogEditor.ShowWindow();
        }
    }
}
