using UnityEngine;
using UnityEngine.Purchasing;

namespace UnityEditor.Purchasing
{
    /// <summary>
    /// IAPButtonMenu class creates options in menus to create the <see cref="IAPButton"/>.
    /// </summary>
    public static class IAPButtonMenu
    {
        /// <summary>
        /// Add option to create a IAPButton from the GameObject menu.
        /// </summary>
        [MenuItem("GameObject/" + IapMenuConsts.PurchasingDisplayName + "/IAP Button", false, 10)]
        public static void GameObjectCreateUnityIAPButton()
        {
            CreateUnityIAPButtonInternal();

            GenericEditorMenuItemClickEventSenderHelpers.SendGameObjectMenuAddIapButtonEvent();
        }

        /// <summary>
        /// Add option to create a IAPButton from the Window/UnityIAP menu.
        /// </summary>
        [MenuItem (IapMenuConsts.MenuItemRoot + "/Create IAP Button", false, 100)]
        public static void CreateUnityIAPButton()
        {
            CreateUnityIAPButtonInternal();

            GenericEditorMenuItemClickEventSenderHelpers.SendIapMenuAddIapButtonEvent();
        }

        static void CreateUnityIAPButtonInternal()
        {
            GameObject buttonObject = CreateButtonObject();

            if (buttonObject)
            {
                IAPButton iapButton = buttonObject.AddComponent<IAPButton>();

                if (iapButton != null)
                {
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(iapButton);
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(iapButton);
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(iapButton);
                }
            }
        }

        static GameObject CreateButtonObject()
        {
            EditorApplication.ExecuteMenuItem("GameObject/UI/Button");

            return Selection.activeGameObject;
        }
    }
}
