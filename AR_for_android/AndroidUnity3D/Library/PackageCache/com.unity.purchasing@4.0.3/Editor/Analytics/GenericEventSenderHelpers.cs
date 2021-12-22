namespace UnityEditor.Purchasing
{
    internal static class GenericEventSenderHelpers
    {
        const string k_ComponentProjectSettings = "Project Settings";

        const string k_ActionValidatePublicKey = "ValidatePublicKey";

        internal static void SendProjectSettingsValidatePublicKey()
        {
            var newEvent = CreateProjectSettingsValidatePublicKeyEvent();
            PurchasingServiceAnalyticsSender.SendEvent(newEvent);
        }

        static GenericEditorGameServiceEvent CreateProjectSettingsValidatePublicKeyEvent()
        {
            return new GenericEditorGameServiceEvent(k_ComponentProjectSettings, k_ActionValidatePublicKey);
        }
    }
}
