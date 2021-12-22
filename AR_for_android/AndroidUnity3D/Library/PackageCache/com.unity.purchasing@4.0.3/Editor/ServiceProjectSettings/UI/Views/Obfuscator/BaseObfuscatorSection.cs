using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    /// <summary>
    /// This is an internal API.
    /// We recommend that you do not use it as it will be removed in a future release.
    /// </summary>
    [Obsolete("Internal API, it will be removed soon.")]
    public abstract class BaseObfuscatorSection
    {
        const string k_ErrorLabelClass = "warning-message";
        const string k_ObfuscateKeysBtn = "ObfuscateKeysButton";
        const string k_VerificationSection = "verification";
        const string k_ErrorSection = "error-message";

        VisualElement m_ObfuscatorBlock;

        /// <summary>
        /// This is an internal API.
        /// We recommend that you do not use it as it will be removed in a future release.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Internal API, it will be removed soon.")]
        protected string m_ErrorMessage;

        internal BaseObfuscatorSection()
        {
        }

        internal void SetupObfuscatorBlock(VisualElement parentBlock)
        {
            m_ObfuscatorBlock = parentBlock;

            PopulateConfigBlock();
        }

        void PopulateConfigBlock()
        {
            SetupButtonActions();
            SetupVerification();
            SetupErrorMessages();
        }

        void SetupButtonActions()
        {
            m_ObfuscatorBlock.Q<Button>(k_ObfuscateKeysBtn).clicked += OnObfuscateClicked;
        }

        void OnObfuscateClicked()
        {
            ObfuscateKeys();
            UpdateVerificationSection();
        }

        /// <summary>
        /// This is an internal API.
        /// We recommend that you do not use it as it will be removed in a future release.
        /// </summary>
        [Obsolete("Internal API, it will be removed soon.")]
        protected abstract void ObfuscateKeys();

        void SetupVerification()
        {
            UpdateVerificationSection();
        }

        void UpdateVerificationSection()
        {
            var verificationSection = m_ObfuscatorBlock.Q(k_VerificationSection);

            verificationSection.style.display = DoesTangleFileExist()
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }

        /// <summary>
        /// This is an internal API.
        /// We recommend that you do not use it as it will be removed in a future release.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Internal API, it will be removed soon.")]
        protected abstract bool DoesTangleFileExist();

        void SetupErrorMessages()
        {
            HandleErrorSection(m_ErrorMessage);
        }

        void HandleErrorSection(string errorMessage)
        {
            var errorSection = m_ObfuscatorBlock.Q(k_ErrorSection);

            errorSection.style.display = (string.IsNullOrEmpty(errorMessage))
                ? DisplayStyle.None
                : DisplayStyle.Flex;

            var errorText = errorSection.Q<Label>(className: k_ErrorLabelClass);
            errorText.text = errorMessage;
        }
    }
}
