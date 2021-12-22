namespace UnityEditor.Purchasing
{
    internal class PurchasingDisabledState : BasePurchasingState
    {
        internal const string k_StateNameDisabled = "DisabledState";

        public PurchasingDisabledState(SimpleStateMachine<PurchasingServiceToggleEvent> stateMachine)
            : base(k_StateNameDisabled, stateMachine)
        {
            ModifyActionForEvent(PurchasingServiceToggleEvent.Enabled, HandleEnabling);
        }

        SimpleStateMachine<PurchasingServiceToggleEvent>.State HandleEnabling(PurchasingServiceToggleEvent raisedEvent)
        {
            return stateMachine.GetStateByName(PurchasingEnabledState.k_StateNameEnabled);
        }

        protected override AnalyticsNoticeBlock CreateAnalyticsNoticeBlock()
        {
            return AnalyticsNoticeBlock.CreateDisabledAnalyticsBlock();
        }

        internal override bool IsEnabled() => false;
    }
}
