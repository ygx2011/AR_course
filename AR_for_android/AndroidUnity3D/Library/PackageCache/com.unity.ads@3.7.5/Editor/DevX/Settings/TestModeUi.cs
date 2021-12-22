#if SERVICES_SDK_CORE_ENABLED
using UnityEditor.Advertisements;
using UnityEngine.UIElements;

namespace UnityEngine.Advertisements.Editor
{
    class TestModeUi : VisualElement
    {
        public TestModeUi()
        {
            var container = UiUtils.GetUiFromTemplate(UiConstants.UiTemplatePaths.TestMode);
            if (container is null)
            {
                var message = string.Format(
                    UiConstants.Formats.TemplateNotFound, nameof(UiConstants.UiTemplatePaths.TestMode));
                Debug.LogError(message);
                return;
            }

            Add(container);

            var toggle = container.Q<Toggle>(UiConstants.UiElementNames.TestModeToggle);
            if (toggle is null)
            {
                return;
            }

            toggle.SetValueWithoutNotify(AdvertisementSettings.testMode);
            toggle.RegisterValueChangedCallback(OnTestModeToggleChanged);
        }

        static void OnTestModeToggleChanged(ChangeEvent<bool> changeEvent)
        {
            EditorGameServiceAnalyticsSender.SendProjectSettingsEnableTestModeEvent(changeEvent.newValue);
            AdvertisementSettings.testMode = changeEvent.newValue;
        }
    }
}
#endif
