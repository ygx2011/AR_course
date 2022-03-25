using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEditor;
using UnityEngine;

using Codice.CM.Common;
using PlasticGui;
using PlasticGui.Gluon;
using PlasticGui.WorkspaceWindow;
using PlasticGui.WorkspaceWindow.PendingChanges;

using GluonShowIncomingChanges = PlasticGui.Gluon.WorkspaceWindow.ShowIncomingChanges;

namespace Unity.PlasticSCM.Editor.UI
{
    internal class IncomingChangesNotification
    {
        internal string InfoText;
        internal string ActionText;
        internal string TooltipText;
        internal bool HasUpdateAction;
        internal PlasticNotification.Status Status;

        internal void Clear()
        {
            InfoText = string.Empty;
            ActionText = string.Empty;
            TooltipText = string.Empty;
            HasUpdateAction = false;
            Status = PlasticNotification.Status.None;
        }
    }

    interface IIncomingChangesNotifier
    {
        bool HasNotification { get; }
        IncomingChangesNotification Notification { get; }
    }

    internal class StatusBar
    {
        internal StatusBar()
        {
            mNotificationClearAction = new CooldownWindowDelayer(
                ClearNotification,
                UnityConstants.NOTIFICATION_CLEAR_INTERVAL);
        }

        internal void Notify(string message, MessageType type, Images.Name imageName)
        {
            mNotification = new Notification(message, type, imageName);
            mHasNotification = true;
            mNotificationClearAction.Ping();
        }

        internal void OnGUI(
            WorkspaceInfo wkInfo,
            WorkspaceWindow workspaceWindow,
            IMergeViewLauncher mergeViewLauncher,
            IGluonViewSwitcher gluonViewSwitcher,
            IIncomingChangesNotifier incomingChangesNotifier,
            bool isGluonMode)
        {
            EditorGUILayout.BeginVertical(
                GetBarStyle(),
                GUILayout.Height(UnityConstants.STATUS_BAR_HEIGHT));
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();

            if (incomingChangesNotifier.HasNotification)
            {
                DrawIncomingChangesNotification(
                    wkInfo,
                    workspaceWindow,
                    mergeViewLauncher,
                    gluonViewSwitcher,
                    incomingChangesNotifier.Notification,
                    isGluonMode);
            }

            if (mHasNotification)
                DrawNotification(mNotification);

            GUILayout.FlexibleSpace();

            DrawWorkspaceStatus(workspaceWindow.WorkspaceStatus);

            EditorGUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }

        static void DrawIncomingChangesNotification(
            WorkspaceInfo wkInfo,
            WorkspaceWindow workspaceWindow,
            IMergeViewLauncher mergeViewLauncher,
            IGluonViewSwitcher gluonViewSwitcher,
            IncomingChangesNotification notification,
            bool isGluonMode)
        {
            Images.Name iconName = 
                notification.Status == PlasticNotification.Status.Conflicts ?
                Images.Name.IconConflicted :
                Images.Name.IconOutOfSync;

            DrawIcon(iconName);

            DrawNotificationLabel(notification.InfoText);

            if (DrawButton(notification.ActionText, notification.TooltipText))
            {
                if (notification.HasUpdateAction)
                {
                    workspaceWindow.UpdateWorkspace();
                    return;
                }

                ShowIncomingChangesForMode(
                    wkInfo,
                    mergeViewLauncher,
                    gluonViewSwitcher,
                    isGluonMode);
            }
        }

        static void DrawNotification(Notification notification)
        {
            DrawIcon(notification.ImageName);
            DrawNotificationLabel(notification.Message);
        }

        static void DrawWorkspaceStatus(WorkspaceStatusString.Data status)
        {
            DrawIcon(Images.Name.IconBranch);

            if (status != null)
            {
                DrawLabel(string.Format(
                    "{0}@{1}@{2}",
                    status.ObjectSpec,
                    status.RepositoryName,
                    status.Server));
            }
        }

        static void DrawIcon(Images.Name iconName)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            var icon = Images.GetImage(iconName);
            GUILayout.Label(
                icon,
                UnityStyles.StatusBar.Icon,
                GUILayout.Height(UnityConstants.STATUS_BAR_ICON_SIZE),
                GUILayout.Width(UnityConstants.STATUS_BAR_ICON_SIZE));

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        
        static void DrawLabel(string label)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.Label(
                label,
                UnityStyles.StatusBar.Label);

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        static void DrawNotificationLabel(string label)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.Label(
                label,
                UnityStyles.StatusBar.NotificationLabel);

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        static bool DrawButton(string label, string tooltip)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            bool buttonClicked = GUILayout.Button(
                new GUIContent(label, tooltip),
                UnityStyles.StatusBar.Button);

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            return buttonClicked;
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

        void ClearNotification()
        {
            mHasNotification = false;
        }

        GUIStyle GetBarStyle()
        {
            if (mBarTexture == null)
                mBarTexture = new Texture2D(1, 1);

            if (mBarStyle == null)
                mBarStyle = new GUIStyle();

            mBarTexture.SetPixel(0, 0, UnityStyles.Colors.BackgroundBar);
            mBarTexture.Apply();
            mBarStyle.normal.background = mBarTexture;

            return mBarStyle;
        }

        struct Notification
        {
            internal string Message { get; private set; }
            internal MessageType MessageType { get; private set; }
            internal Images.Name ImageName { get; private set; }

            internal Notification(string message, MessageType messageType, Images.Name imageName) : this()
            {
                this.Message = message;
                this.MessageType = messageType;
                this.ImageName = imageName;
            }
        }

        Texture2D mBarTexture;
        GUIStyle mBarStyle;

        bool mHasNotification;
        Notification mNotification;

        readonly CooldownWindowDelayer mNotificationClearAction;
    }
}