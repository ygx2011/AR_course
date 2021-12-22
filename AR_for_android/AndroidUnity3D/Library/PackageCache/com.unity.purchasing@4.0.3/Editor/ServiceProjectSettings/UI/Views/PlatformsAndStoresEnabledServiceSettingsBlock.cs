using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    class PlatformsAndStoresEnabledServiceSettingsBlock : PlatformsAndStoresServiceSettingsBlock
    {
        IMGUIContainerPopupAdapter currentStoreTargetContainer { get; set; }

        internal PlatformsAndStoresEnabledServiceSettingsBlock()
        {
            RegisterOnTargetChange();
        }

        void RegisterOnTargetChange()
        {
            if (EditorUserBuildSettings.activeBuildTargetGroup == BuildTargetGroup.Android)
            {
                RegisterOnAndroidTargetChange();
            }
        }

        void RegisterOnAndroidTargetChange()
        {
            UnregisterOnAndroidTargetChange();
            UnityPurchasingEditor.OnAndroidTargetChange += OnAndroidTargetChange;
        }

        void UnregisterOnAndroidTargetChange()
        {
            UnityPurchasingEditor.OnAndroidTargetChange -= OnAndroidTargetChange;
        }

        void OnAndroidTargetChange(AppStore appStore)
        {
            if (!appStore.IsAndroid())
            {
                return;
            }

            var field = GetTagContainer(currentStoreSection) as IMGUIContainer;
            if (field != null && currentStoreTargetContainer != null && currentStoreTargetContainer.container == field)
            {
                currentStoreTargetContainer.index = BuildTargetGroup.Android.ToAppStores().IndexOf(appStore);
            }
        }

        protected override void PopulateStateSensitiveSections(VisualElement rootElement, VisualElement currentBuildTargetSection, VisualElement otherStoresSection)
        {
            PopulateCurrentBuildTarget(currentBuildTargetSection);
            PopulateCurrentStoreTarget(currentStoreSection);
            PopulateOtherStores(otherStoresSection);
        }

        static void PopulateCurrentBuildTarget(VisualElement baseElement)
        {
            PopulatePlatform(baseElement, GetCurrentBuildTarget());
        }

        void PopulateCurrentStoreTarget(VisualElement baseElement)
        {
            var field = GetTagContainer(baseElement) as IMGUIContainer;
            if (field == null)
            {
                return;
            }

            var stores = EditorUserBuildSettings.activeBuildTargetGroup.ToAppStoreDisplayNames();

            currentStoreTargetContainer = new IMGUIContainerPopupAdapter()
            {
                popupValueChangedAction = OnCurrentStoreTargetChanged,
                options = stores.ToArray(),
                index = stores.IndexOf(GetCurrentStoreTarget()),
                container = field
            };
        }

        void OnCurrentStoreTargetChanged(string e)
        {
            var store = e.ToAppStoreFromDisplayName();

            if (store.IsAndroid())
            {
                OnCurrentStoreTargetChanged(store);
            }
        }

        void OnCurrentStoreTargetChanged(AppStore store)
        {
            var actualStore = UnityPurchasingEditor.TryTargetAndroidStore(store);

            if (actualStore != store)
            {
                OnAndroidTargetChange(actualStore);
            }
        }

        static void PopulateOtherStores(VisualElement baseElement)
        {
            PopulateStores(baseElement, GetOtherStores());
        }

        protected override IEnumerable<string> GetStoresForState()
        {
            return GetSupportedStores();
        }

        static IEnumerable<string> GetSupportedStores()
        {
            return GetSupportedStoresIncludingTarget();
        }

        static IEnumerable<string> GetSupportedStoresIncludingTarget()
        {
            return new List<string>(EditorUserBuildSettings.activeBuildTargetGroup.ToAppStoreDisplayNames());
        }

        static IEnumerable<string> GetOtherStores()
        {
            var supportedStores = GetSupportedStoresIncludingTarget();
            var otherStores = GetAllStores().ToList();
            otherStores.RemoveAll(store => supportedStores.Contains(store));

            return otherStores;
        }

        static string GetCurrentBuildTarget()
        {
            return EditorUserBuildSettings.activeBuildTargetGroup.ToPlatformDisplayName();
        }

        static string GetCurrentStoreTarget()
        {
            var currentStoreTargets = EditorUserBuildSettings.activeBuildTargetGroup.ToAppStoreDisplayNames();

            if (currentStoreTargets.Count == 1)
            {
                return currentStoreTargets.First();
            }

            return UnityPurchasingEditor.ConfiguredAppStore().ToDisplayName();
        }

        static void PopulatePlatform(VisualElement baseElement, string platform)
        {
            var tagContainer = GetClearedTagContainer(baseElement);

            tagContainer.Add(MakePlatformStoreTag(platform));
        }
    }
}
