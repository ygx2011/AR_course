using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    class PlatformsAndStoresDisabledServiceSettingsBlock : PlatformsAndStoresServiceSettingsBlock
    {
        protected override void PopulateStateSensitiveSections(VisualElement rootElement, VisualElement currentBuildTargetSection, VisualElement otherStoresSection)
        {
            currentBuildTargetSection.parent.Remove(currentBuildTargetSection);
            currentStoreSection.parent.Remove(currentStoreSection);
            otherStoresSection.parent.Remove(otherStoresSection);
        }

        protected override IEnumerable<string> GetStoresForState()
        {
            return GetAllStores();
        }
    }
}
