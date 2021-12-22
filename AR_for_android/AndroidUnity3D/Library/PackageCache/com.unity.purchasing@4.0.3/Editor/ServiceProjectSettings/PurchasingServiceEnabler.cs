#if SERVICES_SDK_CORE_ENABLED

using System;
using System.Reflection;
using Unity.Services.Core.Editor;
using UnityEditor.Analytics;

namespace UnityEditor.Purchasing
{
    internal class PurchasingServiceEnabler : EditorGameServiceFlagEnabler
    {
        public event Action OnServiceEnabled;
        public event Action OnServiceDisabled;

        const string k_ServiceFlagName = "purchasing";
        const string k_LegacyEnabledSettingName = "Purchasing";

        protected override string FlagName
        {
            get { return k_ServiceFlagName; }
        }

        protected override void EnableLocalSettings()
        {
            SetPurchasingEnableSetting(true);
            AnalyticsSettings.enabled = true;

            OnServiceEnabled?.Invoke();
        }

        static void SetPurchasingEnableSetting(bool value)
        {
            PurchasingSettings.enabled = value;
            SetLegacyEnabledSetting(value);
        }

        static void SetLegacyEnabledSetting(bool value)
        {
            var playerSettingsType = Type.GetType("UnityEditor.PlayerSettings,UnityEditor.dll");
            if (playerSettingsType != null)
            {
                var setCloudServiceEnabledMethod = playerSettingsType.GetMethod("SetCloudServiceEnabled", BindingFlags.Static | BindingFlags.NonPublic);
                if (setCloudServiceEnabledMethod != null)
                {
                    setCloudServiceEnabledMethod.Invoke(null, new object[] {k_LegacyEnabledSettingName, value});
                }
            }
        }

        protected override void DisableLocalSettings()
        {
            SetPurchasingEnableSetting(false);

            OnServiceDisabled?.Invoke();
        }

        public override bool IsEnabled()
        {
            return GetLegacyEnabledSetting();
        }

        static bool GetLegacyEnabledSetting()
        {
            var isEnabled = false;

            var playerSettingsType = Type.GetType("UnityEditor.PlayerSettings,UnityEditor.dll");
            if (playerSettingsType != null)
            {
                var getCloudServiceEnabledMethod = playerSettingsType.GetMethod("GetCloudServiceEnabled", BindingFlags.Static | BindingFlags.NonPublic);
                if (getCloudServiceEnabledMethod != null)
                {
                    var enabledStateResult = getCloudServiceEnabledMethod.Invoke(null, new object[] {k_LegacyEnabledSettingName});
                    isEnabled = Convert.ToBoolean(enabledStateResult);
                }
            }

            return isEnabled;
        }
    }
}

#endif
