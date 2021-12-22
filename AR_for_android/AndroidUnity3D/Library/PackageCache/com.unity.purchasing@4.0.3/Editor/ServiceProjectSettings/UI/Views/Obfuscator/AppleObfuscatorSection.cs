namespace UnityEditor.Purchasing
{
    class AppleObfuscatorSection : BaseObfuscatorSection
    {
        protected override void ObfuscateKeys()
        {
            m_ErrorMessage = ObfuscationGenerator.ObfuscateAppleSecrets();
        }

        protected override bool DoesTangleFileExist()
        {
            return ObfuscationGenerator.DoesAppleTangleClassExist();
        }
    }
}
