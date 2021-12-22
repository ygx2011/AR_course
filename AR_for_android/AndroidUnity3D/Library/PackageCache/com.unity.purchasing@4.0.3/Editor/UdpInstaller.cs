using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Purchasing;
#if UNITY_2019_3_OR_NEWER
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
#endif

namespace UnityEditor.Purchasing
{
    /// <summary>
    /// This class directs the developer to install UDP if it is not already installed through Package Manager.
    /// </summary>
    public class UdpInstaller
    {
#if UNITY_2019_3_OR_NEWER
        private const string k_PackManWindowTitle = "Get UDP via Package Manager";
#else
        private const string k_AssetStoreWindowTitle = "Get UDP via Asset Store";
#endif
        private static readonly Vector2 k_WindowDims = new Vector2(480, 120);

        internal static void PromptUdpInstallation()
        {
            var window = (UdpInstallInstructionsWindow) EditorWindow.GetWindow(typeof(UdpInstallInstructionsWindow));
#if UNITY_2019_3_OR_NEWER
            window.titleContent.text = k_PackManWindowTitle;
#else
            window.titleContent.text = k_AssetStoreWindowTitle;
#endif
            window.minSize = k_WindowDims;
            window.Show();
        }
    }

    /// <summary>
    /// Instructs user how to install UDP.
    /// </summary>
    internal class UdpInstallInstructionsWindow : RichEditorWindow
    {
#if UNITY_2019_3_OR_NEWER
        private const string k_InfoText = "In order to use this functionality, you must install or update the Unity Distribution Portal Package. Would you like to begin?";
        private const string k_UdpPackageName = "com.unity.purchasing.udp";
#else
        private const string k_InfoText = "In order to use this functionality, you must install or update the Unity Distribution Portal Plugin. Would you like to begin?";

        //Browser URL is "https://assetstore.unity.com/packages/add-ons/services/billing/unity-distribution-portal-138507".
        //Took the number at the end and dropped it into the pattern "content/{0}?assetID={1}".
        //This special URL fragment is required by the API and appended to the root URL pattern
        private const string k_UdpAssetStoreIdentifier = "content/138507?assetID=138507";
#endif

        private const string k_NotNowButtonText = "Not Now";
        private const string k_GoButtonText = "Go";

        private void OnGUI()
        {
            // Make fit entire window vertically
            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));

            // Align info text & values horizontally
            EditorGUILayout.BeginHorizontal();

            // Version labels - horizontal stack
            EditorGUILayout.BeginVertical();

            GUILayout.Label(k_InfoText, EditorStyles.wordWrappedLabel);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();  // END Info text row

            // Action button row

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            var notNowButton = GUILayout.Button(k_NotNowButtonText);
            var goButton = GUILayout.Button(k_GoButtonText);

            EditorGUILayout.EndHorizontal(); // END Action button row

            EditorGUILayout.EndVertical(); // END window alignment

            // Handle buttons
            if (notNowButton)
            {
                Close();
            }

            if (goButton)
            {
                // Direct user to install page. User must install manually. Close immediately
                GoToInstaller();
                Close();
            }
        }

        void GoToInstaller()
        {
#if UNITY_2019_3_OR_NEWER
            PackageManager.UI.Window.Open(k_UdpPackageName);
#else
            AssetStore.Open(k_UdpAssetStoreIdentifier);
#endif
        }
    }
}
