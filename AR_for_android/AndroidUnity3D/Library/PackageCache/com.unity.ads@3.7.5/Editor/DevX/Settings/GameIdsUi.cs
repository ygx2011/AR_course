#if SERVICES_SDK_CORE_ENABLED
using UnityEditor.Advertisements;
using UnityEngine.UIElements;

namespace UnityEngine.Advertisements.Editor
{
    class GameIdsUi : VisualElement
    {
        VisualElement m_Container;

        public GameIdsUi()
        {
            m_Container = UiUtils.GetUiFromTemplate(UiConstants.UiTemplatePaths.GameIds);
            if (m_Container is null)
            {
                var message = string.Format(
                    UiConstants.Formats.TemplateNotFound, nameof(UiConstants.UiTemplatePaths.GameIds));
                Debug.LogError(message);
                return;
            }

            Add(m_Container);

            RefreshGameIds();
        }

        public void RefreshGameIds()
        {
            SetUpGameIdFor(RuntimePlatform.IPhonePlayer, UiConstants.UiElementNames.AppleGameId);
            SetUpGameIdFor(RuntimePlatform.Android, UiConstants.UiElementNames.AndroidGameId);

            MarkDirtyRepaint();
        }

        void SetUpGameIdFor(RuntimePlatform platform, string fieldName)
        {
            var gameIdField = m_Container.Q<TextField>(fieldName);
            if (gameIdField is null)
            {
                return;
            }

            var gameId = AdvertisementSettings.GetGameId(platform)
                ?? UiConstants.LocalizedStrings.Unavailable;
            gameIdField.SetValueWithoutNotify(gameId);
        }
    }
}
#endif
