using System;

using UnityEditor;
using UnityEngine;

using Codice.Client.BaseCommands;
using Codice.Client.BaseCommands.Sync;
using Codice.Client.Common;
using Codice.Client.Common.Threading;
using Codice.CM.Common;
using Codice.LogWrapper;
using PlasticGui;
using Unity.PlasticSCM.Editor.Configuration.CloudEdition.Welcome;
using Unity.PlasticSCM.Editor.ProjectDownloader;
using Unity.PlasticSCM.Editor.UI;
using Unity.PlasticSCM.Editor.UI.Progress;
using Unity.PlasticSCM.Editor.WebApi;

namespace Unity.PlasticSCM.Editor.CollabMigration
{
    internal class MigrationDialog : PlasticDialog
    {
        protected override Rect DefaultRect
        {
            get
            {
                var baseRect = base.DefaultRect;
                return new Rect(baseRect.x, baseRect.y, 710, 260);
            }
        }

        protected override string GetTitle()
        {
            return "Upgrade your Collaborate project to Plastic SCM";
        }

        //TODO: localize the strings
        protected override void OnModalGUI()
        {
            GUILayout.BeginHorizontal();

            DoIconArea();

            GUILayout.Space(20);

            DoContentArea();

            GUILayout.EndHorizontal();

            DoButtonsArea();

            mProgressControls.ForcedUpdateProgress(this);
        }

        internal static bool Show(
            EditorWindow parentWindow,
            string unityAccessToken,
            string projectPath,
            string organizationName,
            RepId repId,
            long changesetId,
            long branchId,
            Action afterWorkspaceMigratedAction)
        {
            MigrationDialog dialog = Create(
                unityAccessToken,
                projectPath,
                organizationName,
                repId,
                changesetId,
                branchId,
                afterWorkspaceMigratedAction,
                new ProgressControlsForDialogs());

            return dialog.RunModal(parentWindow) == ResponseType.Ok;
        }

        void DoIconArea()
        {
            GUILayout.BeginVertical();

            GUILayout.Space(30);

            Rect iconRect = GUILayoutUtility.GetRect(
                GUIContent.none, EditorStyles.label,
                GUILayout.Width(60), GUILayout.Height(60));

            GUI.DrawTexture(
                iconRect,
                Images.GetImage(Images.Name.IconPlastic),
                ScaleMode.ScaleToFit);

            GUILayout.EndVertical();
        }

        void DoContentArea()
        {
            GUILayout.BeginVertical();

            Title("Upgrade your Collaborate project to Plastic SCM");

            GUILayout.Space(20);

            Paragraph("Your Unity project belongs to a Collaborate repository which has been "
                +"upgraded to Plastic SCM free of charge by your administrator. Your local "
                +"workspace will now be converted to a Plastic SCM workspace. Select 'Migrate' "
                +"to start the conversion process. This process may take a few minutes.");

            DrawProgressForDialogs.For(
               mProgressControls.ProgressData);

            GUILayout.Space(10);

            GUILayout.EndVertical();
        }

        //TODO: localize the strings
        void DoButtonsArea()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
    
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    DoOkButton();
                    DoCloseButton();
                    return;
                }

                DoCloseButton();
                DoOkButton();
            }
        }

        void DoOkButton()
        {
            if (mIsMigrationCompleted)
            {
                DoOpenPlasticButton();
                return;
            }

            DoMigrateButton();
        }

        void DoCloseButton()
        {
            GUI.enabled = !mProgressControls.ProgressData.IsWaitingAsyncResult;

            if (NormalButton(PlasticLocalization.GetString(
                    PlasticLocalization.Name.CloseButton)))
            {
                CloseButtonAction();
            }

            GUI.enabled = true;
        }

        void DoOpenPlasticButton()
        {
            if (!NormalButton("Open Plastic SCM"))
                return;

            ((IPlasticDialogCloser)this).CloseDialog();
            ShowWindow.Plastic();
        }

        void DoMigrateButton()
        {
            GUI.enabled = !mProgressControls.ProgressData.IsWaitingAsyncResult;

            if (NormalButton("Migrate"))
            {
                if (EditorUtility.DisplayDialog(
                        "Collab migration to Plastic SCM",
                        "Are you sure to start the migration process?",
                        PlasticLocalization.GetString(PlasticLocalization.Name.YesButton),
                        PlasticLocalization.GetString(PlasticLocalization.Name.NoButton)))
                {
                    LaunchMigration(
                        mUnityAccessToken, mProjectPath,
                        mOrganizationName, mRepId,
                        mChangesetId, mBranchId,
                        mAfterWorkspaceMigratedAction,
                        mProgressControls);
                }
            }

            GUI.enabled = true;
        }

        void LaunchMigration(
            string unityAccessToken,
            string projectPath,
            string organizationName,
            RepId repId,
            long changesetId,
            long branchId,
            Action afterWorkspaceMigratedAction,
            IProgressControls progressControls)
        {
            string serverName = string.Format(
                "{0}@cloud", organizationName);

            progressControls.ShowProgress(
                "Migrating project to Plastic SCM...");

            TokenExchangeResponse tokenExchangeResponse = null;
            WorkspaceInfo workspaceInfo = null;

            IThreadWaiter waiter = ThreadWaiter.GetWaiter(10);
            waiter.Execute(
            /*threadOperationDelegate*/
            delegate
            {
                // we just migrate a cloud project,
                // so let's assume we're going to use Cloud Edition
                SetupUnityEditionToken.CreateCloudEditionTokenIfNeeded();

                if (!ClientConfig.IsConfigured())
                {
                    AutoConfigClientConf.FromUnityAccessToken(
                        unityAccessToken, serverName, projectPath);
                }

                tokenExchangeResponse = WebRestApiClient.
                    PlasticScm.TokenExchange(unityAccessToken);

                if (tokenExchangeResponse.Error != null)
                    return;

                CloudEditionWelcomeWindow.JoinCloudServer(
                    serverName,
                    tokenExchangeResponse.User,
                    tokenExchangeResponse.AccessToken);

                RepositoryInfo repInfo = new BaseCommandsImpl().
                    GetRepositoryInfo(repId, serverName);

                if (repInfo == null)
                    return;

                repInfo.SetExplicitServer(serverName);

                workspaceInfo = CreateWorkspaceFromCollab.Create(
                    projectPath, repInfo.Name, repInfo,
                    changesetId, branchId,
                    new CreateWorkspaceFromCollab.Progress());
            },
            /*afterOperationDelegate*/
            delegate
            {
                progressControls.HideProgress();

                if (waiter.Exception != null)
                {
                    DisplayException(progressControls, waiter.Exception);
                    return;
                }

                if (tokenExchangeResponse.Error != null)
                {
                    mLog.ErrorFormat(
                        "Unable to get TokenExchangeResponse: {0} [code {1}]",
                        tokenExchangeResponse.Error.Message,
                        tokenExchangeResponse.Error.ErrorCode);
                }

                if (tokenExchangeResponse.Error != null ||
                    workspaceInfo == null)
                {
                    progressControls.ShowError(
                        "Failed to migrate the project to Plastic SCM");
                    return;
                }

                progressControls.ShowSuccess(
                    "The project migration to Plastic SCM is completed successfully");

                mIsMigrationCompleted = true;

                afterWorkspaceMigratedAction();
            });
        }

        static void DisplayException(
            IProgressControls progressControls,
            Exception ex)
        {
            ExceptionsHandler.LogException(
                "MigrationDialog", ex);

            progressControls.ShowError(
                ExceptionsHandler.GetCorrectExceptionMessage(ex));
        }

        static MigrationDialog Create(
            string unityAccessToken,
            string projectPath,
            string organizationName,
            RepId repId,
            long changesetId,
            long branchId,
            Action afterWorkspaceMigratedAction,
            ProgressControlsForDialogs progressControls)
        {
            var instance = CreateInstance<MigrationDialog>();
            instance.IsResizable = false;
            instance.mUnityAccessToken = unityAccessToken;
            instance.mProjectPath = projectPath;
            instance.mOrganizationName = organizationName;
            instance.mRepId = repId;
            instance.mChangesetId = changesetId;
            instance.mBranchId = branchId;
            instance.mAfterWorkspaceMigratedAction = afterWorkspaceMigratedAction;
            instance.mProgressControls = progressControls;
            instance.mEscapeKeyAction = instance.CloseButtonAction;
            return instance;
        }

        bool mIsMigrationCompleted;

        ProgressControlsForDialogs mProgressControls;
        Action mAfterWorkspaceMigratedAction;
        long mChangesetId;
        long mBranchId;
        RepId mRepId;
        string mOrganizationName;
        string mProjectPath;
        string mUnityAccessToken;

        static readonly ILog mLog = LogManager.GetLogger("MigrationDialog");
    }
}
