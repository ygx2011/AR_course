#if SERVICES_SDK_CORE_ENABLED
using System;
using System.Reflection;
using Unity.Services.Core.Editor;
using UnityEditor.Advertisements;

namespace UnityEngine.Advertisements.Editor
{
    class AdsServiceEnabler : EditorGameServiceFlagEnabler
    {
        const string k_ProjectSettingName = "Unity Ads";

        static MethodInfo s_GetCloudServiceEnabled;

        static MethodInfo s_SetCloudServiceEnabled;

        public event Action ServiceEnabled;

        public event Action ServiceDisabled;

        protected override string FlagName { get; } = "ads";

        static AdsServiceEnabler()
        {
            s_GetCloudServiceEnabled = TryGetPlayerSettingsMethod("GetCloudServiceEnabled");
            s_SetCloudServiceEnabled = TryGetPlayerSettingsMethod("SetCloudServiceEnabled");
        }

        static MethodInfo TryGetPlayerSettingsMethod(string methodName)
        {
            MethodInfo playerSettingsMethod = null;
            try
            {
                var playerSettingsType = Type.GetType("UnityEditor.PlayerSettings,UnityEditor.dll");
                playerSettingsMethod = playerSettingsType?.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
            }
            catch (Exception)
            {
                //Simply return null if something failed in the reflection.
            }

            return playerSettingsMethod;
        }

        public AdsServiceEnabler()
        {
            if (!(s_GetCloudServiceEnabled is null))
            {
                AdvertisementSettings.enabled = GetProjectSettingWithReflection();
            }
        }

        static bool GetProjectSettingWithReflection()
        {
            var enabledStateResult = s_GetCloudServiceEnabled.Invoke(null, new object[] { k_ProjectSettingName });
            var isEnabled = Convert.ToBoolean(enabledStateResult);
            return isEnabled;
        }

        public override bool IsEnabled()
        {
            return AdvertisementSettings.enabled;
        }

        protected override void EnableLocalSettings()
        {
            SetSettingFlag(true);

            ServiceEnabled?.Invoke();
        }

        protected override void DisableLocalSettings()
        {
            SetSettingFlag(false);

            ServiceDisabled?.Invoke();
        }

        static void SetSettingFlag(bool value)
        {
            AdvertisementSettings.enabled = value;
            s_SetCloudServiceEnabled?.Invoke(null, new object[] { k_ProjectSettingName, value });
        }
    }
}
#endif
