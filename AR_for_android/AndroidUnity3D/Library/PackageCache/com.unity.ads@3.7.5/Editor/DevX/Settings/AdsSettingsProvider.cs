#if SERVICES_SDK_CORE_ENABLED
using System.Collections.Generic;
using Unity.Services.Core.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace UnityEngine.Advertisements.Editor
{
    class AdsSettingsProvider : EditorGameServiceSettingsProvider
    {
        VisualElement m_Root;
        GettingStartedUi m_GettingStartedUi;
        TestModeUi m_TestModeUi;
        GameIdsUi m_GameIdsUi;
        VisualElement m_SupportedPlatformsUi;

        bool m_HasLoggedAssetStorePackageInstalled;

        public AdsSettingsProvider(SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(GetSettingsPath(), scopes, keywords) {}

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
#if ENABLE_EDITOR_GAME_SERVICES
            return new AdsSettingsProvider(SettingsScope.Project);
#else
            return null;
#endif
        }

        internal static string GetSettingsPath()
        {
            return GenerateProjectSettingsPath(new AdsServiceIdentifier().GetKey());
        }

        AdsService m_Service = (AdsService)EditorGameServiceRegistry.Instance.GetEditorGameService<AdsServiceIdentifier>();
        protected override IEditorGameService EditorGameService => m_Service;

        protected override string Title => UiConstants.LocalizedStrings.Ads;

        protected override string Description => UiConstants.LocalizedStrings.Description;

        protected override VisualElement GenerateServiceDetailUI()
        {
            m_Root = new VisualElement();
            m_GettingStartedUi = new GettingStartedUi();
            m_TestModeUi = new TestModeUi();
            m_GameIdsUi = new GameIdsUi();
            m_SupportedPlatformsUi = PlatformSupportUiHelper.GeneratePlatformSupport(UiConstants.SupportedPlatforms);

            SetUpStyles();

            RefreshDetailUI();

            return m_Root;
        }

        void SetUpStyles()
        {
            m_Root.AddToClassList(UiConstants.ClassNames.Ads);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(UiConstants.StyleSheetPaths.Common);
            if (!(styleSheet is null))
            {
                m_Root.styleSheets.Add(styleSheet);
            }
        }

        void RefreshDetailUI()
        {
            if (m_Root is null)
            {
                return;
            }

            m_Root.Clear();

            m_Root.Add(m_GettingStartedUi);

            if (m_Service.Enabler.IsEnabled())
            {
                m_Root.Add(m_TestModeUi);
                m_Root.Add(m_GameIdsUi);

                LogOnceAssetStorePackageInstalled();
            }

            m_Root.Add(m_SupportedPlatformsUi);

            TranslateAllLabelsIn(m_Root);
        }

        void LogOnceAssetStorePackageInstalled()
        {
            if (m_HasLoggedAssetStorePackageInstalled
                || !PluginUtils.AreAssetStorePluginsInstalled())
            {
                return;
            }

            Debug.LogWarning(UiConstants.LocalizedStrings.AssetStorePackageInstalledMessage);

            m_HasLoggedAssetStorePackageInstalled = true;
        }

        static void TranslateAllLabelsIn(VisualElement root)
        {
            root.Query<TextElement>()
                .ForEach(TranslateLabel);

            string TranslateLabel(TextElement label)
            {
                label.text = L10n.Tr(label.text);

                return label.text;
            }
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);

            m_Service.GameIdsUpdated += OnGameIdsUpdated;

            var serviceEnabler = (AdsServiceEnabler)m_Service.Enabler;
            serviceEnabler.ServiceEnabled += RefreshDetailUI;
            serviceEnabler.ServiceDisabled += RefreshDetailUI;
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();

            m_Service.GameIdsUpdated -= OnGameIdsUpdated;

            var serviceEnabler = (AdsServiceEnabler)m_Service.Enabler;
            serviceEnabler.ServiceEnabled -= RefreshDetailUI;
            serviceEnabler.ServiceDisabled -= RefreshDetailUI;
        }

        void OnGameIdsUpdated()
        {
            if (m_GameIdsUi is null)
            {
                return;
            }

            m_GameIdsUi.RefreshGameIds();

            Repaint();
        }
    }
}
#endif
