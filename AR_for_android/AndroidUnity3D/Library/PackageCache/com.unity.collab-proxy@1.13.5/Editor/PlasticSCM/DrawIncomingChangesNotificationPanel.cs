using UnityEditor;
using UnityEngine;

using Codice.CM.Common;
using PlasticGui;
using PlasticGui.Gluon;
using PlasticGui.WorkspaceWindow.PendingChanges;
using Unity.PlasticSCM.Editor.UI;

using GluonShowIncomingChanges = PlasticGui.Gluon.WorkspaceWindow.ShowIncomingChanges;

namespace Unity.PlasticSCM.Editor
{
    internal class NotificationPanelData
    {
        internal enum StyleType
        {
            None,
            Red,
            Green,
        }

        internal bool HasUpdateAction;
        internal string InfoText;
        internal string ActionText;
        internal string TooltipText;
        internal StyleType NotificationStyle;

        internal void Clear()
        {
            HasUpdateAction = false;
            InfoText = string.Empty;
            ActionText = string.Empty;
            TooltipText = string.Empty;
            NotificationStyle = StyleType.None;
        }
    }

    interface IIncomingChangesNotificationPanel
    {
        bool IsVisible
        {
            get;
        }

        NotificationPanelData Data
        {
            get;
        }
    }

    internal static class DrawIncomingChangesNotificationPanel
    {
        internal static void For(WorkspaceInfo workspaceInfo,
            WorkspaceWindow workspaceWindow,
            IMergeViewLauncher mergeViewLauncher,
            IGluonViewSwitcher gluonSwitcher,
            bool isGluonMode,
            bool isVisible,
            NotificationPanelData notificationPanelData)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            
            var icon = Images.GetImage(
                notificationPanelData.NotificationStyle == NotificationPanelData.StyleType.Green
                ? Images.Name.DownloadIconGreen : Images.Name.DownloadIconRed);
            GUILayout.Label(icon, GUILayout.Height(16), GUILayout.Width(16));

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();


            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label(notificationPanelData.InfoText, labelStyle);

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUIStyle buttonStyle = new GUIStyle(EditorStyles.miniButtonLeft);
            buttonStyle.fixedWidth = 60;
            DoActionButton(
                workspaceInfo, workspaceWindow,
                mergeViewLauncher, gluonSwitcher, isGluonMode,
                notificationPanelData.HasUpdateAction,
                new GUIContent(
                notificationPanelData.ActionText, notificationPanelData.TooltipText), buttonStyle);

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        static void DoActionButton(WorkspaceInfo workspaceInfo,
            WorkspaceWindow workspaceWindow,
            IMergeViewLauncher mergeViewLauncher,
            IGluonViewSwitcher gluonSwitcher,
            bool isGluonMode,
            bool isUpdateAction,
            GUIContent buttonContent,
            GUIStyle buttonStyle)
        {
            if (GUILayout.Button(buttonContent, buttonStyle))
            {
                if (isUpdateAction)
                {
                    workspaceWindow.UpdateWorkspace();
                    return;
                }

                ShowIncomingChangesForMode(
                    workspaceInfo, mergeViewLauncher,
                    gluonSwitcher, isGluonMode);
            }
        }

        static void ShowIncomingChangesForMode(
            WorkspaceInfo workspaceInfo,
            IMergeViewLauncher mergeViewLauncher,
            IGluonViewSwitcher gluonSwitcher,
            bool isGluonMode)
        {
            if (isGluonMode)
            {
                GluonShowIncomingChanges.FromNotificationBar(
                    workspaceInfo, gluonSwitcher);
                return;
            }

            ShowIncomingChanges.FromNotificationBar(
                workspaceInfo, mergeViewLauncher);
        }
    }
}