using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    internal class GooglePlayConfigurationSettingsBlock : IPurchasingSettingsUIBlock
    {
        const string k_UpdateGooglePlayKeyBtn = "UpdateGooglePlayKeyBtn";
        const string k_GooglePlayLink = "GooglePlayLink";
        const string k_GooglePlayKeyEntry = "GooglePlayKeyEntry";

        const string k_VerifiedMode = "verified-mode";
        const string k_UnverifiedMode = "unverified-mode";
        const string k_ErrorKeyFormat = "error-key-format";
        const string k_ErrorUnauthorized = "error-unauthorized-user";
        const string k_ErrorServer = "error-server-error";
        const string k_ErrorFetchKey = "error-fetch-key";

        const string k_GooglePlayKeyBtnUpdateLabel = "Update";
        const string k_GooglePlayKeyBtnVerifyLabel = "Verify";

        GoogleConfigurationData m_GooglePlayDataRef;
        GoogleConfigurationWebRequests m_WebRequests;

        VisualElement m_ConfigurationBlock;

        GoogleObfuscatorSection m_ObfuscatorSection;

        internal GooglePlayConfigurationSettingsBlock(GoogleConfigurationData remoteData)
        {
            m_GooglePlayDataRef = remoteData;
            m_WebRequests = new GoogleConfigurationWebRequests(remoteData, this.OnGetGooglePlayKey, this.OnUpdateGooglePlayKey);

            m_ObfuscatorSection = new GoogleObfuscatorSection(m_GooglePlayDataRef);
        }

        public VisualElement GetUIBlockElement()
        {
            return SetupConfigurationBlock();
        }

        VisualElement SetupConfigurationBlock()
        {
            m_ConfigurationBlock = SettingsUIUtils.CloneUIFromTemplate(UIResourceUtils.googlePlayConfigUxmlPath);

            SetupStyleSheets();
            PopulateConfigBlock();
            m_ObfuscatorSection.SetupObfuscatorBlock(m_ConfigurationBlock);

            return m_ConfigurationBlock;
        }

        void SetupStyleSheets()
        {
            m_ConfigurationBlock.AddStyleSheetPath(UIResourceUtils.purchasingCommonUssPath);
            m_ConfigurationBlock.AddStyleSheetPath(EditorGUIUtility.isProSkin ? UIResourceUtils.purchasingDarkUssPath : UIResourceUtils.purchasingLightUssPath);
        }

        void PopulateConfigBlock()
        {
            ObtainExistingGooglePlayKey();
            ToggleGoogleKeyStateDisplay();
            SetupButtonActions();
        }

        void ObtainExistingGooglePlayKey()
        {
            m_WebRequests.RequestRetrieveKeyOperation();
        }

        void SetupButtonActions()
        {
            m_ConfigurationBlock.Q<Button>(k_UpdateGooglePlayKeyBtn).clicked += UpdateGooglePlayKey;
            m_ConfigurationBlock.Q<Button>(k_GooglePlayLink).clicked += OpenGooglePlayDevConsole;

            m_ConfigurationBlock.Q<TextField>(k_GooglePlayKeyEntry).RegisterValueChangedCallback(evt => {
                m_GooglePlayDataRef.googlePlayKey = evt.newValue;
            });
        }

        void UpdateGooglePlayKey()
        {
            m_WebRequests.RequestUpdateOperation();
        }

        void OpenGooglePlayDevConsole()
        {
            Application.OpenURL(PurchasingUrls.googlePlayDevConsoleUrl);
        }

        void ToggleGoogleKeyStateDisplay()
        {
            ToggleUpdateButtonDisplay();
            ToggleVerifiedModeDisplay();
            ToggleUnverifiedModeDisplay();
        }

        void ToggleUpdateButtonDisplay()
        {
            var updateGooglePlayKeyBtn = m_ConfigurationBlock.Q<Button>(k_UpdateGooglePlayKeyBtn);
            if (updateGooglePlayKeyBtn != null)
            {
                if (GetTrackingKeyState() == GooglePlayRevenueTrackingKeyState.Verified)
                {
                    updateGooglePlayKeyBtn.text = k_GooglePlayKeyBtnUpdateLabel;
                }
                else
                {
                    updateGooglePlayKeyBtn.text = k_GooglePlayKeyBtnVerifyLabel;
                }
            }
        }

        GooglePlayRevenueTrackingKeyState GetTrackingKeyState()
        {
            return m_GooglePlayDataRef.revenueTrackingState;
        }

        void ToggleVerifiedModeDisplay()
        {
            var verifiedMode = m_ConfigurationBlock.Q(k_VerifiedMode);
            if (verifiedMode != null)
            {
                if (GetTrackingKeyState() == GooglePlayRevenueTrackingKeyState.Verified)
                {
                    verifiedMode.style.display = DisplayStyle.Flex;
                }
                else
                {
                    verifiedMode.style.display = DisplayStyle.None;
                }
            }
        }

        void ToggleUnverifiedModeDisplay()
        {
            var unVerifiedMode = m_ConfigurationBlock.Q(k_UnverifiedMode);
            if (unVerifiedMode != null)
            {
                unVerifiedMode.style.display = (GetTrackingKeyState() == GooglePlayRevenueTrackingKeyState.Verified)
                    ? DisplayStyle.None
                    : DisplayStyle.Flex;

                ToggleErrorStateBlockVisibility(GooglePlayRevenueTrackingKeyState.InvalidFormat, k_ErrorKeyFormat);
                ToggleErrorStateBlockVisibility(GooglePlayRevenueTrackingKeyState.UnauthorizedUser, k_ErrorUnauthorized);
                ToggleErrorStateBlockVisibility(GooglePlayRevenueTrackingKeyState.ServerError, k_ErrorServer);
                ToggleErrorStateBlockVisibility(GooglePlayRevenueTrackingKeyState.CantFetch, k_ErrorFetchKey);
            }
        }

        void ToggleErrorStateBlockVisibility(GooglePlayRevenueTrackingKeyState matchingBlockState, string blockName)
        {
            var errorSection = m_ConfigurationBlock.Q(blockName);
            if (errorSection != null)
            {
                errorSection.style.display = (GetTrackingKeyState() == matchingBlockState)
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            }
        }

        void OnGetGooglePlayKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                m_GooglePlayDataRef.revenueTrackingState = GooglePlayRevenueTrackingKeyState.Verified;
                m_ConfigurationBlock.Q<TextField>(k_GooglePlayKeyEntry).SetValueWithoutNotify(key);
            }
            else
            {
                m_GooglePlayDataRef.revenueTrackingState = GooglePlayRevenueTrackingKeyState.CantFetch;
            }

            ToggleGoogleKeyStateDisplay();
        }

        void OnUpdateGooglePlayKey(GooglePlayRevenueTrackingKeyState keyState)
        {
            m_GooglePlayDataRef.revenueTrackingState = keyState;

            GenericEventSenderHelpers.SendProjectSettingsValidatePublicKey();

            ToggleGoogleKeyStateDisplay();
        }
    }
}
