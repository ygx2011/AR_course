using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    class GoogleObfuscatorSection : BaseObfuscatorSection
    {
        GoogleConfigurationData m_GoogleConfigDataRef;

        internal GoogleObfuscatorSection(GoogleConfigurationData googleData)
            : base()
        {
            m_GoogleConfigDataRef = googleData;
        }

        protected override void ObfuscateKeys()
        {
            m_ErrorMessage = ObfuscationGenerator.ObfuscateGoogleSecrets(GetGoogleKey());
        }

        string GetGoogleKey()
        {
            return m_GoogleConfigDataRef.googlePlayKey;
        }

        protected override bool DoesTangleFileExist()
        {
            return ObfuscationGenerator.DoesGooglePlayTangleClassExist();
        }
    }
}
