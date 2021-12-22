using UnityEditor;

using PlasticGui.WorkspaceWindow;

namespace Unity.PlasticSCM.Editor.Developer
{
    internal class IncomingChangesNotificationPanel :
        IIncomingChangesNotificationPanel,
        CheckIncomingChanges.IUpdateIncomingChanges
    {
        bool IIncomingChangesNotificationPanel.IsVisible
        {
            get { return mIsVisible; }
        }

        NotificationPanelData IIncomingChangesNotificationPanel.Data
        {
            get { return mPanelData; }
        }

        internal IncomingChangesNotificationPanel(
            PlasticWindow plasticWindow)
        {
            mPlasticWindow = plasticWindow;
        }

        void CheckIncomingChanges.IUpdateIncomingChanges.Hide()
        {
            mPlasticWindow.SetNotificationStatus(PlasticNotification.Status.None);
            mPanelData.Clear();

            mIsVisible = false;

            mPlasticWindow.Repaint();
        }

        void CheckIncomingChanges.IUpdateIncomingChanges.Show(
            string infoText,
            string actionText,
            string tooltipText,
            CheckIncomingChanges.Severity severity,
            CheckIncomingChanges.Action action)
        {
            PlasticNotification.Status status = PlasticNotification.Status.None;
            if (severity == CheckIncomingChanges.Severity.Info)
                status = PlasticNotification.Status.IncomingChanges;
            else if (severity == CheckIncomingChanges.Severity.Warning)
                status = PlasticNotification.Status.Conflicts;
            mPlasticWindow.SetNotificationStatus(status);

            UpdateData(
                mPanelData, infoText, actionText,
                tooltipText, severity, action);

            mIsVisible = true;

            mPlasticWindow.Repaint();
        }

        static void UpdateData(
            NotificationPanelData data,
            string infoText,
            string actionText,
            string tooltipText,
            CheckIncomingChanges.Severity severity,
            CheckIncomingChanges.Action action)
        {
            data.HasUpdateAction =
                action == CheckIncomingChanges.Action.Update;
            data.InfoText = infoText;
            data.ActionText = actionText;
            data.TooltipText = tooltipText;
            data.NotificationStyle =
                severity == CheckIncomingChanges.Severity.Info ?
                NotificationPanelData.StyleType.Green :
                NotificationPanelData.StyleType.Red;
        }

        bool mIsVisible;

        NotificationPanelData mPanelData = new NotificationPanelData();

        PlasticWindow mPlasticWindow;
    }
}