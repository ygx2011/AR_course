using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    abstract class PlatformsAndStoresServiceSettingsBlock : IPurchasingSettingsUIBlock
    {
        static readonly List<string> k_StoreNames = new List<string>()
        {
            "Amazon Appstore",
            "Google Play",
            "Apple App Store",
            "Mac App Store",
            "Unity Distribution Portal",
            "Microsoft Store"
        };

        static readonly string k_TagClass = "platform-tag";
        static readonly string k_TagContainerClass = "tag-container";

        static readonly string k_CurrentBuildTargetSectionName = "CurrentBuildTargetSection";
        static readonly string k_CurrentStoreSectionName = "CurrentStoreSection";
        static readonly string k_SupportedStoresSectionName = "SupportedStoresSection";
        static readonly string k_OtherStoresSectionName = "OtherStoresSection";
        static readonly string k_Label = "Label";

        protected VisualElement currentStoreSection { get; set; }

        public static PlatformsAndStoresServiceSettingsBlock CreateStateSpecificBlock(bool enabled)
        {
            if (enabled)
            {
                return new PlatformsAndStoresEnabledServiceSettingsBlock();
            }
            else
            {
                return new PlatformsAndStoresDisabledServiceSettingsBlock();
            }
        }

        public VisualElement GetUIBlockElement()
        {
            return SetupPlatformAndStoresBlock();
        }

        VisualElement SetupPlatformAndStoresBlock()
        {
            var rootElement = SettingsUIUtils.CloneUIFromTemplate(UIResourceUtils.platformSupportUxmlPath);

            currentStoreSection = rootElement.Q(k_CurrentStoreSectionName);

            SetupStyleSheets(rootElement);
            PopulateSections(rootElement);

            return rootElement;
        }

        static void SetupStyleSheets(VisualElement rootElement)
        {
            rootElement.AddStyleSheetPath(UIResourceUtils.platformSupportCommonUssPath);
            rootElement.AddStyleSheetPath(EditorGUIUtility.isProSkin ? UIResourceUtils.platformSupportDarkUssPath : UIResourceUtils.platformSupportLightUssPath);
        }

        void PopulateSections(VisualElement rootElement)
        {
            var currentBuildTargetSection = rootElement.Q(k_CurrentBuildTargetSectionName);
            var otherStoresSection = rootElement.Q(k_OtherStoresSectionName);

            PopulateStateSensitiveSections(rootElement, currentBuildTargetSection, otherStoresSection);
            PopulateSupportedStoresSection(rootElement.Q(k_SupportedStoresSectionName));
        }

        protected abstract void PopulateStateSensitiveSections(VisualElement rootElement, VisualElement currentBuildTargetSection, VisualElement otherStoresSection);

        void PopulateSupportedStoresSection(VisualElement baseElement)
        {
            PopulateStores(baseElement, GetStoresForState());
        }

        protected static void PopulateStores(VisualElement baseElement, IEnumerable<string> stores)
        {
            var tagContainer = GetClearedTagContainer(baseElement);

            foreach (var store in stores)
            {
                tagContainer.Add(MakePlatformStoreTag(store));
            }
        }

        protected static VisualElement GetClearedTagContainer(VisualElement baseElement)
        {
            var tagContainer = GetTagContainer(baseElement);
            tagContainer.Clear();
            return tagContainer;
        }

        protected static VisualElement GetTagContainer(VisualElement baseElement)
        {
            return baseElement.Q(className: k_TagContainerClass);
        }

        protected static VisualElement MakePlatformStoreTag(string assetDisplayName)
        {
            var storeLabel = MakeLabel();

            var label = storeLabel.Q(name: k_Label) as Label;
            AddText(label, assetDisplayName);

            return storeLabel;
        }

        static VisualElement MakeLabel()
        {
            var label = SettingsUIUtils.CloneUIFromTemplate(UIResourceUtils.labelUxmlPath);
            label.AddToClassList(k_TagClass);
            return label;
        }
        static void AddText(Label label, string assetDisplayName)
        {
            if (label == null)
            {
                return;
            }

            label.text = assetDisplayName;
        }

        protected abstract IEnumerable<string> GetStoresForState();

        protected static IEnumerable<string> GetAllStores()
        {
            return k_StoreNames;
        }
    }
}
