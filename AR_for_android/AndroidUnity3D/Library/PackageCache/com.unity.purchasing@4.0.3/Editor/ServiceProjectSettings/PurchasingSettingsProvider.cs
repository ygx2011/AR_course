#if SERVICES_SDK_CORE_ENABLED
using System.Collections.Generic;
using Unity.Services.Core.Editor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    internal class PurchasingSettingsProvider : EditorGameServiceSettingsProvider
    {
        const string k_ProjectSettingsName = "In-App Purchasing";

        const string k_Title = "In-App Purchases";
        const string k_Description = "Simplify cross platform In-App Purchasing";

        PurchasingGameService m_Service;
        bool m_CallbacksInitialized;

        SimpleStateMachine<PurchasingServiceToggleEvent> m_StateMachine;
        PurchasingDisabledState m_DisabledState;
        PurchasingEnabledState m_EnabledState;

        internal VisualElement rootVisualElement { get; private set; }

        [SettingsProvider]
        internal static SettingsProvider CreateServicesProvider()
        {
#if ENABLE_EDITOR_GAME_SERVICES
            return new PurchasingSettingsProvider(SettingsScope.Project, PurchasingSettingsKeywords.GetKeywords());
#else
            return null;
#endif
        }

        internal PurchasingSettingsProvider(SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(GetSettingsPath(), scopes, keywords)
        {
            m_Service = (PurchasingGameService)EditorGameServiceRegistry.Instance.GetEditorGameService<PurchasingServiceIdentifier>();

            ConfigureStateMachine();
        }

        void ConfigureStateMachine()
        {
            m_StateMachine = new SimpleStateMachine<PurchasingServiceToggleEvent>();

            m_StateMachine.AddEvent(PurchasingServiceToggleEvent.Disabled);
            m_StateMachine.AddEvent(PurchasingServiceToggleEvent.Enabled);

            m_DisabledState = new PurchasingDisabledState(m_StateMachine);
            m_EnabledState = new PurchasingEnabledState(m_StateMachine);

            m_StateMachine.AddState(m_DisabledState);
            m_StateMachine.AddState(m_EnabledState);
        }

        internal static string GetSettingsPath()
        {
            return GenerateProjectSettingsPath(k_ProjectSettingsName);
        }

        protected override IEditorGameService EditorGameService => m_Service;

        protected override string Title => k_Title;

        protected override string Description => k_Description;

        protected override VisualElement GenerateServiceDetailUI()
        {
            rootVisualElement = new VisualElement();

            InitializeStateMachine();
            RefreshDetailUI();

            return rootVisualElement;
        }

        void RefreshDetailUI()
        {
            rootVisualElement.Clear();

            var clonedContainer = SettingsUIUtils.CloneUIFromTemplate(UIResourceUtils.purchasingServicesRootUxmlPath);
            rootVisualElement.Add(clonedContainer);

            var uiState = (BasePurchasingState) m_StateMachine.currentState;

            foreach (var uiStateElement in uiState.GetStateUI())
            {
                rootVisualElement.Add(uiStateElement);
            }
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);

            InitializeStateMachine();
            InitializeServiceCallbacks();
        }

        void InitializeStateMachine()
        {
            if (m_StateMachine.currentState == null)
            {
                if (m_Service.Enabler.IsEnabled())
                {
                    m_StateMachine.Initialize(m_EnabledState);
                }
                else
                {
                    m_StateMachine.Initialize(m_DisabledState);
                }
            }
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();

            FinalizeStateMachine();
            FinalizeServiceCallbacks();
        }

        void FinalizeStateMachine()
        {
            m_StateMachine.ClearCurrentState();
        }

        void InitializeServiceCallbacks()
        {
            if (!m_CallbacksInitialized)
            {
                m_Service.AddEnableAction(EnableOperationCompleted);
                m_Service.AddDisableAction(DisableOperationCompleted);

                m_CallbacksInitialized = true;
            }
        }

        void FinalizeServiceCallbacks()
        {
            if (m_CallbacksInitialized)
            {
                m_Service.RemoveEnableAction(EnableOperationCompleted);
                m_Service.RemoveDisableAction(DisableOperationCompleted);

                m_CallbacksInitialized = false;
            }
        }

        void EnableOperationCompleted()
        {
            m_StateMachine.ProcessEvent(PurchasingServiceToggleEvent.Enabled);

            RefreshDetailUI();
        }

        void DisableOperationCompleted()
        {
            m_StateMachine.ProcessEvent(PurchasingServiceToggleEvent.Disabled);

            RefreshDetailUI();
        }
    }
}
#endif
