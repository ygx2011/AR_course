using UnityEditor;
using UnityEngine;

using Codice.CM.Common;
using PlasticGui;
using PlasticGui.Gluon;
using PlasticGui.WorkspaceWindow;
using Unity.PlasticSCM.Editor.UI;

namespace Unity.PlasticSCM.Editor
{
    internal static class DrawStatusBar
    {
        internal static void For(
            WorkspaceInfo workspaceInfo,
            WorkspaceWindow workspaceWindow,
            IMergeViewLauncher mergeViewLauncher,
            IGluonViewSwitcher gluonSwitcher,
            bool isGluonMode,
            IIncomingChangesNotificationPanel notificationPanel)
        {
            var barStyle = new GUIStyle();

            var barTexture = GetBarTexture();
            barTexture.SetPixel(0, 0, UnityStyles.Colors.BackgroundBar);
            barTexture.Apply();

            barStyle.normal.background = barTexture;

            EditorGUILayout.BeginVertical(barStyle, GUILayout.Height(mBarHeight));
            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();

            if (notificationPanel.IsVisible)
            {
                DrawIncomingChangesNotificationPanel.For(
                    workspaceInfo,
                    workspaceWindow,
                    mergeViewLauncher,
                    gluonSwitcher,
                    isGluonMode,
                    notificationPanel.IsVisible,
                    notificationPanel.Data);
            }

            GUILayout.FlexibleSpace();

            DrawStatusBarIcon();
            DrawStatusBarLabel(workspaceWindow.WorkspaceStatus);

            EditorGUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }

        static void DrawStatusBarIcon()
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            var icon = Images.GetImage(Images.Name.IconBranch);
            GUILayout.Label(
                icon,
                UnityStyles.PlasticWindow.StatusBarIcon,
                GUILayout.Height(mBarIconSize),
                GUILayout.Width(mBarIconSize));

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        static void DrawStatusBarLabel(WorkspaceStatusString.Data status)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            if (status != null)
            {
                GUILayout.Label(
                    string.Format("{0}@{1}@{2}",
                        status.ObjectSpec,
                        status.RepositoryName,
                        status.Server),
                    UnityStyles.PlasticWindow.StatusBarLabel);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        static Texture2D GetBarTexture()
        {
            if (mBarTexture == null)
            {
                mBarTexture = new Texture2D(1, 1);
            }

            return mBarTexture;
        }

        static Texture2D mBarTexture = null;
        static readonly float mBarHeight = 24f;
        static readonly float mBarIconSize = 16f;
    }
}