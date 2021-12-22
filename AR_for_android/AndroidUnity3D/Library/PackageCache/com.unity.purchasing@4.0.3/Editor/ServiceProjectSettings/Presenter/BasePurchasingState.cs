using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    internal abstract class BasePurchasingState : SimpleStateMachine<PurchasingServiceToggleEvent>.State
    {
        protected List<IPurchasingSettingsUIBlock> m_UIBlocks;

        protected BasePurchasingState(string stateName, SimpleStateMachine<PurchasingServiceToggleEvent> stateMachine)
            : base(stateName, stateMachine)
        {
            m_UIBlocks = new List<IPurchasingSettingsUIBlock>();
            m_UIBlocks.Add(CreateAnalyticsNoticeBlock());
            m_UIBlocks.Add(PlatformsAndStoresServiceSettingsBlock.CreateStateSpecificBlock(IsEnabled()));
        }

        internal List<VisualElement> GetStateUI()
        {
            return m_UIBlocks.Select(block => block.GetUIBlockElement()).ToList();
        }

        protected abstract AnalyticsNoticeBlock CreateAnalyticsNoticeBlock();

        internal abstract bool IsEnabled();
    }
}
