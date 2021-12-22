#if SERVICES_SDK_CORE_ENABLED
using UnityEngine.UIElements;

namespace UnityEngine.Advertisements.Editor
{
    class GettingStartedUi : VisualElement
    {
        public GettingStartedUi()
        {
            var container = UiUtils.GetUiFromTemplate(UiConstants.UiTemplatePaths.GettingStarted);
            if (container is null)
            {
                var message = string.Format(
                    UiConstants.Formats.TemplateNotFound, nameof(UiConstants.UiTemplatePaths.GettingStarted));
                Debug.LogError(message);
                return;
            }

            Add(container);

            container.AddOnClickedForElement(OnLearnMoreClicked, UiConstants.UiElementNames.LearnMoreLink);
        }

        static void OnLearnMoreClicked()
        {
            EditorGameServiceAnalyticsSender.SendProjectSettingsLearnMoreEvent();
            Application.OpenURL(UiConstants.Urls.LearnMore);
        }
    }
}
#endif
