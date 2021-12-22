namespace UnityEditor.Purchasing
{
    class PurchasingEnabledState : BasePurchasingState
    {
        internal const string k_StateNameEnabled = "EnabledState";

        GoogleConfigurationData m_GoogleConfigData;

        public PurchasingEnabledState(SimpleStateMachine<PurchasingServiceToggleEvent> stateMachine)
            : base(k_StateNameEnabled, stateMachine)
        {
            m_GoogleConfigData = new GoogleConfigurationData();

            m_UIBlocks.Add(new GooglePlayConfigurationSettingsBlock(m_GoogleConfigData));
            m_UIBlocks.Add(new AppleConfigurationSettingsBlock());
            m_UIBlocks.Add(new IapCatalogServiceSettingsBlock());

            ModifyActionForEvent(PurchasingServiceToggleEvent.Disabled, HandleDisabling);
        }

        SimpleStateMachine<PurchasingServiceToggleEvent>.State HandleDisabling(PurchasingServiceToggleEvent raisedEvent)
        {
            return stateMachine.GetStateByName(PurchasingDisabledState.k_StateNameDisabled);
        }

        protected override AnalyticsNoticeBlock CreateAnalyticsNoticeBlock()
        {
            return AnalyticsNoticeBlock.CreateEnabledAnalyticsBlock();
        }

        internal override bool IsEnabled() => true;
    }
}
