using UnityEditor;
using UnityEngine;
using UnityEngine.Purchasing;

namespace UnityEditor.Purchasing
{
    /// <summary>
    /// IAPListenerMenu class creates options in menus to create the <see cref="IAPListener"/>.
    /// </summary>
    public static class IAPListenerMenu
    {
        /// <summary>
        /// Add option to create a IAPListener from the GameObject menu.
        /// </summary>
        [MenuItem("GameObject/" + IapMenuConsts.PurchasingDisplayName + "/IAP Listener", false, 10)]
        public static void GameObjectCreateUnityIAPListener()
        {
            CreateUnityIAPListenerInternal();

            GenericEditorMenuItemClickEventSenderHelpers.SendGameObjectMenuAddIapListenerEvent();
        }

        /// <summary>
        /// Add option to create a IAPListener from the Window/UnityIAP menu.
        /// </summary>
        [MenuItem (IapMenuConsts.MenuItemRoot + "/Create IAP Listener", false, 100)]
        public static void CreateUnityIAPListener()
        {
            CreateUnityIAPListenerInternal();

            GenericEditorMenuItemClickEventSenderHelpers.SendIapMenuAddIapListenerEvent();
        }

        static void CreateUnityIAPListenerInternal()
        {
            GameObject listenerObject = CreateListenerObject();

            if (listenerObject) {
                listenerObject.AddComponent<IAPListener>();
                listenerObject.name = "IAP Listener";
            }
        }

        static GameObject CreateListenerObject()
        {
            EditorApplication.ExecuteMenuItem("GameObject/Create Empty");

            return Selection.activeGameObject;
        }
    }
}
