using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using Codice.Client.BaseCommands;
using Codice.Client.BaseCommands.EventTracking;
using Codice.Client.BaseCommands.Merge;
using Codice.Client.Commands;
using Codice.Client.Common;
using Codice.Client.Common.FsNodeReaders;
using Codice.Client.Common.Threading;
using Codice.CM.Common;
using Codice.CM.Common.Merge;
using PlasticGui;
using PlasticGui.WorkspaceWindow;
using PlasticGui.WorkspaceWindow.BranchExplorer;
using PlasticGui.WorkspaceWindow.Diff;
using PlasticGui.WorkspaceWindow.IncomingChanges;
using PlasticGui.WorkspaceWindow.Merge;
using Unity.PlasticSCM.Editor.AssetUtils;
using Unity.PlasticSCM.Editor.Tool;
using Unity.PlasticSCM.Editor.UI;
using Unity.PlasticSCM.Editor.UI.Progress;
using Unity.PlasticSCM.Editor.UI.Tree;
using Unity.PlasticSCM.Editor.Views.IncomingChanges.Developer.DirectoryConflicts;

namespace Unity.PlasticSCM.Editor.Views.IncomingChanges.Developer
{
    internal class IncomingChangesTab :
        IIncomingChangesTab,
        IRefreshableView,
        MergeViewLogic.IMergeView,
        MergeChangesTree.IGetConflictResolution,
        IIncomingChangesViewMenuOperations,
        IncomingChangesViewMenu.IMetaMenuOperations
    {
        internal IncomingChangesTab(
            WorkspaceInfo wkInfo,
            IWorkspaceWindow workspaceWindow,
            IViewSwitcher switcher,
            NewIncomingChangesUpdater newIncomingChangesUpdater,
            EditorWindow parentWindow)
        {
            mWkInfo = wkInfo;
            mWorkspaceWindow = workspaceWindow;
            mSwitcher = switcher;
            mNewIncomingChangesUpdater = newIncomingChangesUpdater;
            mParentWindow = parentWindow;
            mGuiMessage = new UnityPlasticGuiMessage(parentWindow);

            BuildComponents(mWkInfo);

            mProgressControls = new ProgressControlsForViews();

            PlasticNotifier plasticNotifier = new PlasticNotifier();

            mMergeController = new MergeController(
                mWkInfo,
                null,
                null,
                EnumMergeType.IncomingMerge,
                true,
                plasticNotifier,
                null);

            mMergeViewLogic = new MergeViewLogic(
                mWkInfo,
                EnumMergeType.IncomingMerge,
                true,
                mMergeController,
                plasticNotifier,
                ShowIncomingChangesFrom.NotificationBar,
                null,
                mNewIncomingChangesUpdater,
                null,
                this,
                NewChangesInWk.Build(mWkInfo, new BuildWorkspacekIsRelevantNewChange()),
                mProgressControls,
                null);

            ((IRefreshableView)this).Refresh();
        }

        bool IIncomingChangesTab.IsVisible{ get; set; }

        void IIncomingChangesTab.OnDisable()
        {
            TreeHeaderSettings.Save(
                mIncomingChangesTreeView.multiColumnHeader.state,
                UnityConstants.DEVELOPER_INCOMING_CHANGES_TABLE_SETTINGS_NAME);
        }

        void IIncomingChangesTab.Update()
        {
            mProgressControls.UpdateProgress(mParentWindow);
        }

        void IIncomingChangesTab.OnGUI()
        {
            if (Event.current.type == EventType.Layout)
            {
                mHasPendingDirectoryConflicts =
                    MergeTreeResultParser.GetUnsolvedDirectoryConflictsCount(mResultConflicts) > 0;
                mIsOperationRunning = mProgressControls.IsOperationRunning();
            }

            DoConflictsTree(
                mIncomingChangesTreeView,
                mIsOperationRunning,
                mHasNothingToDownload);

            List<MergeChangeInfo> selectedIncomingChanges =
                mIncomingChangesTreeView.GetSelectedIncomingChanges();

            if (GetSelectedIncomingChangesGroupInfo.For(
                    selectedIncomingChanges).IsDirectoryConflictsSelection &&
                !Mouse.IsRightMouseButtonPressed(Event.current))
            {
                DoDirectoryConflictResolutionPanel(
                    selectedIncomingChanges,
                    new Action<MergeChangeInfo>(ResolveDirectoryConflict),
                    mConflictResolutionStates);
            }

            DoActionsToolbar(
                mIsMessageLabelVisible,
                mHasNothingToDownload,
                mIsErrorMessageLabelVisible,
                mIsProcessMergesButtonVisible,
                mIsCancelMergesButtonVisible,
                mIsProcessMergesButtonEnabled,
                mIsCancelMergesButtonEnabled,
                mProcessMergesButtonText,
                mHasPendingDirectoryConflicts,
                mIsOperationRunning,
                mWorkspaceWindow,
                mMergeViewLogic,
                mProgressControls.ProgressData);

            if (mProgressControls.HasNotification())
            {
                DrawProgressForViews.ForNotificationArea(
                    mProgressControls.ProgressData);
            }
        }

        void IIncomingChangesTab.AutoRefresh()
        {
            BranchInfo workingBranch = null;

            IThreadWaiter waiter = ThreadWaiter.GetWaiter(10);
            waiter.Execute(
                /*threadOperationDelegate*/ delegate
                {
                    workingBranch = OverlappedCalculator.GetWorkingBranch(
                        mWkInfo.ClientPath);
                },
                /*afterOperationDelegate*/ delegate
                {
                    if (waiter.Exception != null)
                    {
                        ExceptionsHandler.DisplayException(waiter.Exception);
                        return;
                    }

                    // No need for merge info if it's a label
                    if (workingBranch == null)
                        return;

                    mMergeController.UpdateMergeObjectInfoIfNeeded(workingBranch);
                    mMergeViewLogic.AutoRefresh();
                });
        }

        void IRefreshableView.Refresh()
        {
            BranchInfo workingBranch = null;

            IThreadWaiter waiter = ThreadWaiter.GetWaiter(10);
            waiter.Execute(
                /*threadOperationDelegate*/ delegate
                {
                    workingBranch = OverlappedCalculator.GetWorkingBranch(
                        mWkInfo.ClientPath);
                },
                /*afterOperationDelegate*/ delegate
                {
                    if (waiter.Exception != null)
                    {
                        ExceptionsHandler.DisplayException(waiter.Exception);
                        return;
                    }

                    // No need for merge info if it's a label
                    if (workingBranch == null)
                        return;

                    mMergeController.UpdateMergeObjectInfoIfNeeded(workingBranch);
                    mMergeViewLogic.Refresh();
                });
        }

        void MergeViewLogic.IMergeView.UpdateData(
            MergeTreeResult resultConflicts,
            ExplainMergeData explainMergeData,
            MergeSolvedFileConflicts solvedFileConflicts,
            MountPointWithPath rootMountPoint,
            bool isIncomingMerge,
            bool isMergeTo,
            bool isUpdateMerge,
            bool mergeHasFinished)
        {
            HideMessage();

            ShowProcessMergesButton(MergeViewTexts.GetProcessMergeButtonText(
                MergeTreeResultParser.GetFileConflictsCount(resultConflicts) > 0,
                true));

            mResultConflicts = resultConflicts;
            mSolvedFileConflicts = solvedFileConflicts;
            mRootMountPoint = rootMountPoint;
            mConflictResolutionStates.Clear();

            UpdateFileConflictsTree(
                MergeChangesTree.BuildForIncomingChangesView(
                    mResultConflicts,
                    this,
                    mRootMountPoint),
                mIncomingChangesTreeView);

            UpdateChangesOverview();
        }

        void MergeViewLogic.IMergeView.UpdateSolvedDirectoryConflicts()
        {
        }

        void MergeViewLogic.IMergeView.UpdateSolvedFileConflicts(
            MergeSolvedFileConflicts solvedFileConflicts)
        {
            mIncomingChangesTreeView.UpdateSolvedFileConflicts(
                solvedFileConflicts);
        }

        void MergeViewLogic.IMergeView.ShowMessage(
            string title,
            string message,
            bool isErrorMessage)
        {
            if (isErrorMessage)
            {
                mErrorMessageLabelText = message;
                mIsErrorMessageLabelVisible = true;
                return;
            }

            mMessageLabelText = message;
            mIsMessageLabelVisible = true;
            mHasNothingToDownload = message == PlasticLocalization.GetString(
                PlasticLocalization.Name.MergeNothingToDownloadForIncomingView);
        }

        string MergeViewLogic.IMergeView.GetComments(out bool bCancel)
        {
            bCancel = false;
            return string.Empty;
        }

        void MergeViewLogic.IMergeView.DisableProcessMergesButton()
        {
            mIsProcessMergesButtonEnabled = false;
        }

        void MergeViewLogic.IMergeView.ShowCancelButton()
        {
            mIsCancelMergesButtonEnabled = true;
            mIsCancelMergesButtonVisible = true;
        }

        void MergeViewLogic.IMergeView.HideCancelButton()
        {
            mIsCancelMergesButtonEnabled = false;
            mIsCancelMergesButtonVisible = false;
        }

        SelectedIncomingChangesGroupInfo IIncomingChangesViewMenuOperations.GetSelectedIncomingChangesGroupInfo()
        {
            return IncomingChangesSelection.GetSelectedGroupInfo(mIncomingChangesTreeView);
        }

        string MergeChangesTree.IGetConflictResolution.GetConflictResolution(
            DirectoryConflict conflict)
        {
            return mMergeViewLogic.GetConflictResolution(conflict);
        }

        void IIncomingChangesViewMenuOperations.MergeContributors()
        {
            if (LaunchTool.ShowDownloadPlasticExeWindow(
                mWkInfo,
                false,
                TrackFeatureUseEvent.Features.InstallPlasticCloudFromMergeSelectedFiles,
                TrackFeatureUseEvent.Features.InstallPlasticEnterpriseFromMergeSelectedFiles,
                TrackFeatureUseEvent.Features.CancelPlasticInstallationFromMergeSelectedFiles))
                return;

            List<string> selectedPaths = IncomingChangesSelection.
                GetPathsFromSelectedFileConflictsIncludingMeta(
                    mIncomingChangesTreeView);

            mMergeViewLogic.ProcessMerges(
                mWorkspaceWindow,
                mSwitcher,
                mGuiMessage,
                selectedPaths,
                MergeContributorType.MergeContributors,
                RefreshAsset.UnityAssetDatabase);
        }

        void IIncomingChangesViewMenuOperations.MergeKeepingSourceChanges()
        {
            List<string> selectedPaths = IncomingChangesSelection.
                GetPathsFromSelectedFileConflictsIncludingMeta(
                    mIncomingChangesTreeView);

            mMergeViewLogic.ProcessMerges(
                mWorkspaceWindow,
                mSwitcher,
                mGuiMessage,
                selectedPaths,
                MergeContributorType.KeepSource,
                RefreshAsset.UnityAssetDatabase);
        }

        void IIncomingChangesViewMenuOperations.MergeKeepingWorkspaceChanges()
        {
            List<string> selectedPaths = IncomingChangesSelection.
                GetPathsFromSelectedFileConflictsIncludingMeta(
                    mIncomingChangesTreeView);

            mMergeViewLogic.ProcessMerges(
                mWorkspaceWindow,
                mSwitcher,
                mGuiMessage,
                selectedPaths,
                MergeContributorType.KeepDestination,
                RefreshAsset.UnityAssetDatabase);
        }

        void IIncomingChangesViewMenuOperations.DiffYoursWithIncoming()
        {
            MergeChangeInfo incomingChange = IncomingChangesSelection.
                GetSingleSelectedIncomingChange(mIncomingChangesTreeView);

            if (incomingChange == null)
                return;

            DiffYoursWithIncoming(
                incomingChange,
                mWkInfo);
        }

        void IIncomingChangesViewMenuOperations.DiffIncomingChanges()
        {
            MergeChangeInfo incomingChange = IncomingChangesSelection.
                GetSingleSelectedIncomingChange(mIncomingChangesTreeView);

            if (incomingChange == null)
                return;

            DiffIncomingChanges(
                incomingChange,
                mWkInfo);
        }

        void IncomingChangesViewMenu.IMetaMenuOperations.DiffIncomingChanges()
        {
            MergeChangeInfo incomingChange = IncomingChangesSelection.
                GetSingleSelectedIncomingChange(mIncomingChangesTreeView);

            if (incomingChange == null)
                return;

            DiffIncomingChanges(
                mIncomingChangesTreeView.GetMetaChange(incomingChange),
                mWkInfo);
        }

        void IncomingChangesViewMenu.IMetaMenuOperations.DiffYoursWithIncoming()
        {
            MergeChangeInfo incomingChange = IncomingChangesSelection.
                GetSingleSelectedIncomingChange(mIncomingChangesTreeView);

            if (incomingChange == null)
                return;

            DiffYoursWithIncoming(
                mIncomingChangesTreeView.GetMetaChange(incomingChange),
                mWkInfo);
        }

        bool IncomingChangesViewMenu.IMetaMenuOperations.SelectionHasMeta()
        {
            return mIncomingChangesTreeView.SelectionHasMeta();
        }

        static void DiffYoursWithIncoming(
            MergeChangeInfo incomingChange,
            WorkspaceInfo wkInfo)
        {
            if (LaunchTool.ShowDownloadPlasticExeWindow(
                wkInfo,
                false,
                TrackFeatureUseEvent.Features.InstallPlasticCloudFromDiffYoursWithIncoming,
                TrackFeatureUseEvent.Features.InstallPlasticEnterpriseFromDiffYoursWithIncoming,
                TrackFeatureUseEvent.Features.CancelPlasticInstallationFromDiffYoursWithIncoming))
                return;


            DiffOperation.DiffYoursWithIncoming(
                wkInfo,
                incomingChange.GetMount(),
                incomingChange.GetRevision(),
                incomingChange.GetPath(),
                xDiffLauncher: null,
                imageDiffLauncher: null);
        }

        static void DiffIncomingChanges(
            MergeChangeInfo incomingChange,
            WorkspaceInfo wkInfo)
        {
            if (LaunchTool.ShowDownloadPlasticExeWindow(
                wkInfo,
                false,
                TrackFeatureUseEvent.Features.InstallPlasticCloudFromDiffIncomingChanges,
                TrackFeatureUseEvent.Features.InstallPlasticEnterpriseFromDiffIncomingChanges,
                TrackFeatureUseEvent.Features.CancelPlasticInstallationFromDiffIncomingChanges))
                return;

            DiffOperation.DiffRevisions(
                wkInfo,
                incomingChange.GetMount().RepSpec,
                incomingChange.GetBaseRevision(),
                incomingChange.GetRevision(),
                incomingChange.GetPath(),
                incomingChange.GetPath(),
                true,
                xDiffLauncher: null,
                imageDiffLauncher: null);
        }

        void ShowProcessMergesButton(string processMergesButtonText)
        {
            mProcessMergesButtonText = processMergesButtonText;
            mIsProcessMergesButtonEnabled = true;
            mIsProcessMergesButtonVisible = true;
        }

        void HideMessage()
        {
            mMessageLabelText = string.Empty;
            mIsMessageLabelVisible = false;
            mHasNothingToDownload = false;

            mErrorMessageLabelText = string.Empty;
            mIsErrorMessageLabelVisible = false;
        }

        static void DoDirectoryConflictResolutionPanel(
            List<MergeChangeInfo> selectedChangeInfos,
            Action<MergeChangeInfo> resolveDirectoryConflictAction,
            Dictionary<DirectoryConflict, ConflictResolutionState> conflictResolutionStates)
        {
            MergeChangeInfo selectedDirectoryConflict = selectedChangeInfos[0];

            if (selectedDirectoryConflict.DirectoryConflict.IsResolved())
                return;

            DirectoryConflictUserInfo conflictUserInfo;
            DirectoryConflictAction[] conflictActions;

            DirectoryConflictResolutionInfo.FromDirectoryConflict(
                selectedDirectoryConflict.GetMount(),
                selectedDirectoryConflict.DirectoryConflict,
                out conflictUserInfo,
                out conflictActions);

            ConflictResolutionState conflictResolutionState = GetConflictResolutionState(
                selectedDirectoryConflict.DirectoryConflict,
                conflictActions,
                conflictResolutionStates);

            int pendingSelectedConflictsCount = GetPendingConflictsCount(
                selectedChangeInfos);

            DrawDirectoryResolutionPanel.ForConflict(
                selectedDirectoryConflict,
                (pendingSelectedConflictsCount <= 1) ? 0 : pendingSelectedConflictsCount - 1,
                conflictUserInfo,
                conflictActions,
                resolveDirectoryConflictAction,
                ref conflictResolutionState);
        }

        void ResolveDirectoryConflict(MergeChangeInfo conflict)
        {
            ConflictResolutionState state;

            if (!mConflictResolutionStates.TryGetValue(conflict.DirectoryConflict, out state))
                return;

            List<DirectoryConflictResolutionData> conflictResolutions =
                new List<DirectoryConflictResolutionData>();

            AddConflictResolution(
                conflict,
                state.ResolveAction,
                state.RenameValue,
                conflictResolutions);

            MergeChangeInfo metaConflict =
                mIncomingChangesTreeView.GetMetaChange(conflict);

            if (metaConflict != null)
            {
                AddConflictResolution(
                    metaConflict,
                    state.ResolveAction,
                    MetaPath.GetMetaPath(state.RenameValue),
                    conflictResolutions);
            }

            if (state.IsApplyActionsForNextConflictsChecked)
            {
                foreach (MergeChangeInfo otherConflict in mIncomingChangesTreeView.GetSelectedIncomingChanges())
                {
                    AddConflictResolution(
                        otherConflict,
                        state.ResolveAction,
                        state.RenameValue,
                        conflictResolutions);
                }
            }

            mMergeViewLogic.ResolveDirectoryConflicts(conflictResolutions);
        }

        static void AddConflictResolution(
            MergeChangeInfo conflict,
            DirectoryConflictResolveActions resolveAction,
            string renameValue,
            List<DirectoryConflictResolutionData> conflictResolutions)
        {
            conflictResolutions.Add(new DirectoryConflictResolutionData(
                conflict.DirectoryConflict,
                conflict.Xlink,
                conflict.GetMount().Mount,
                resolveAction,
                renameValue));
        }

        static void DoConflictsTree(
            IncomingChangesTreeView incomingChangesTreeView,
            bool isOperationRunning,
            bool hasNothingToDownload)
        {
            GUI.enabled = !isOperationRunning;

            Rect rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);

            incomingChangesTreeView.OnGUI(rect);

            if (hasNothingToDownload)
            {
                DrawTreeViewEmptyState.For(
                    rect,
                    PlasticLocalization.GetString(PlasticLocalization.Name.NoIncomingChanges),
                    PlasticLocalization.GetString(PlasticLocalization.Name.WorkspaceIsUpToDate),
                    Images.Name.StepOk);
            }

            GUI.enabled = true;
        }

        void DoActionsToolbar(
            bool isMessageLabelVisible,
            bool hasNothingToDownload,
            bool isErrorMessageLabelVisible,
            bool isProcessMergesButtonVisible,
            bool isCancelMergesButtonVisible,
            bool isProcessMergesButtonEnabled,
            bool isCancelMergesButtonEnabled,
            string processMergesButtonText,
            bool hasPendingDirectoryConflictsCount,
            bool isOperationRunning,
            IWorkspaceWindow workspaceWindow,
            MergeViewLogic mergeViewLogic,
            ProgressControlsForViews.Data progressData)
        {
            Rect result = GUILayoutUtility.GetRect(mParentWindow.position.width, 1);
            EditorGUI.DrawRect(result, UnityStyles.Colors.BarBorder);

            EditorGUILayout.BeginVertical(UnityStyles.IncomingChangesTab.ActionToolbar);
            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();

            if (!isOperationRunning)
            {
                DoActionsToolbarMessage(
                    isMessageLabelVisible,
                    hasNothingToDownload,
                    isErrorMessageLabelVisible);

                if (isProcessMergesButtonVisible)
                {
                    DoProcessMergesButton(
                        isProcessMergesButtonEnabled && !hasPendingDirectoryConflictsCount,
                        processMergesButtonText,
                        mSwitcher,
                        workspaceWindow,
                        mGuiMessage,
                        mergeViewLogic);
                }

                if (isCancelMergesButtonVisible)
                {
                    DoCancelMergesButton(
                        isCancelMergesButtonEnabled,
                        mergeViewLogic);
                }

                if (hasPendingDirectoryConflictsCount)
                {
                    GUILayout.Space(5);
                    DoWarningMessage();
                }
            }
            else
            {
                DrawProgressForViews.ForIndeterminateProgress(
                    progressData);
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }

        void DoActionsToolbarMessage(
            bool isMessageLabelVisible,
            bool hasNothingToDownload,
            bool isErrorMessageLabelVisible)
        {
            if (isMessageLabelVisible)
            {
                string message = mMessageLabelText;

                if (hasNothingToDownload)
                {
                    message = string.Format(
                        "{0}. {1}.",
                        PlasticLocalization.GetString(PlasticLocalization.Name.NoIncomingChanges),
                        PlasticLocalization.GetString(PlasticLocalization.Name.WorkspaceIsUpToDate));
                }

                DoInfoMessage(message);
            }
            
            if (isErrorMessageLabelVisible)
            {
                DoErrorMessage(mErrorMessageLabelText);
            }

            if (!isMessageLabelVisible && !isErrorMessageLabelVisible)
            {
                DoChangesOverview();
            }
        }

        void DoChangesOverview()
        {
            DrawChangesOverviewItem(
                Images.GetImage(Images.Name.IconConflicted),
                PlasticLocalization.Name.DirectoryConflictsTitleSingular,
                PlasticLocalization.Name.DirectoryConflictsTitlePlural,
                mDirectoryConflictCount,
                0,
                false);

            DrawChangesOverviewItem(
                Images.GetImage(Images.Name.IconConflicted),
                PlasticLocalization.Name.FileConflictsTitleSingular,
                PlasticLocalization.Name.FileConflictsTitlePlural,
                mFileConflictCount,
                0,
                false);

            DrawChangesOverviewItem(
                Images.GetImage(Images.Name.IconOutOfSync),
                PlasticLocalization.Name.MergeChangesMadeInSourceOfMergeOverviewSingular,
                PlasticLocalization.Name.MergeChangesMadeInSourceOfMergeOverviewPlural,
                mModifiedCount,
                mModifiedSize,
                true);

            DrawChangesOverviewItem(
                Images.GetImage(Images.Name.IconAddedLocal),
                PlasticLocalization.Name.MergeNewItemsToDownloadOverviewSingular,
                PlasticLocalization.Name.MergeNewItemsToDownloadOverviewPlural,
                mAddedCount,
                mAddedSize,
                true);

            DrawChangesOverviewItem(
                Images.GetImage(Images.Name.IconDeletedRemote),
                PlasticLocalization.Name.MergeDeletesToApplyOverviewSingular,
                PlasticLocalization.Name.MergeDeletesToApplyOverviewPlural,
                mDeletedCount,
                mDeletedSize,
                true);
        }

        static void DrawChangesOverviewItem(
            Texture icon,
            PlasticLocalization.Name singularLabel,
            PlasticLocalization.Name pluralLabel,
            int count,
            long size,
            bool showSize)
        {
            if (count == 0)
                return;

            EditorGUILayout.BeginHorizontal();

            GUIContent iconContent = new GUIContent(icon);
            GUILayout.Label(iconContent, GUILayout.Width(20f), GUILayout.Height(20f));

            string label = PlasticLocalization.GetString(count > 1 ? pluralLabel : singularLabel);
            if (showSize)
                label = string.Format(label, count, SizeConverter.ConvertToSizeString(size));
            else
                label = string.Format(label, count);

            GUIContent content = new GUIContent(label);
            GUILayout.Label(content, UnityStyles.IncomingChangesTab.ChangesToApplySummaryLabel);

            GUILayout.Space(5);

            EditorGUILayout.EndHorizontal();
        }

        static void AfterProcessMerges()
        {
            EditorWindow.GetWindow<PlasticWindow>().
                    mNotificationDrawer.Notify("Project successfully updated",
                    UnityEditor.MessageType.None,
                    Images.Name.StepOk);

            RefreshAsset.UnityAssetDatabase();
        }

        static void DoProcessMergesButton(
            bool isEnabled,
            string processMergesButtonText,
            IViewSwitcher switcher,
            IWorkspaceWindow workspaceWindow,
            GuiMessage.IGuiMessage guiMessage,
            MergeViewLogic mergeViewLogic)
        {
            GUI.enabled = isEnabled;

            if (DrawActionButton.For(processMergesButtonText))
            {
                mergeViewLogic.ProcessMerges(
                    workspaceWindow,
                    switcher,
                    guiMessage,
                    new List<string>(),
                    MergeContributorType.MergeContributors,
                    AfterProcessMerges);
            }

            GUI.enabled = true;
        }

        void DoCancelMergesButton(
            bool isEnabled,
            MergeViewLogic mergeViewLogic)
        {
            GUI.enabled = isEnabled;

            if (DrawActionButton.For(PlasticLocalization.GetString(
                    PlasticLocalization.Name.CancelButton)))
            {
                mergeViewLogic.Cancel();

                mIsCancelMergesButtonEnabled = false;
            }

            GUI.enabled = true;
        }

        static void DoWarningMessage()
        {
            string label = PlasticLocalization.GetString(PlasticLocalization.Name.SolveConflictsInLable);

            GUILayout.Label(
                new GUIContent(label, Images.GetWarnIcon()),
                UnityStyles.IncomingChangesTab.HeaderWarningLabel);
        }

        void UpdateFileConflictsTree(
            MergeChangesTree incomingChangesTree,
            IncomingChangesTreeView incomingChangesTreeView)
        {
            UnityIncomingChangesTree unityIncomingChangesTree = null;

            IThreadWaiter waiter = ThreadWaiter.GetWaiter(10);
            waiter.Execute(
                /*threadOperationDelegate*/ delegate
                {
                    unityIncomingChangesTree = UnityIncomingChangesTree.BuildIncomingChangeCategories(
                        incomingChangesTree);
                    incomingChangesTree.ResolveUserNames(
                        new MergeChangesTree.ResolveUserName());
                },
                /*afterOperationDelegate*/ delegate
                {
                    incomingChangesTreeView.BuildModel(unityIncomingChangesTree);
                    incomingChangesTreeView.Sort();
                    incomingChangesTreeView.Reload();

                    incomingChangesTreeView.SelectFirstUnsolvedDirectoryConflict();
                });
        }

        void UpdateChangesOverview()
        {
            if (mResultConflicts == null || mRootMountPoint == null)
                return;
            
            mModifiedCount = 0;
            mModifiedSize = 0;
            foreach (FileConflict modified in mResultConflicts.FilesModifiedOnSource)
            {
                if (modified.Src.RevInfo == null)
                    continue;

                mModifiedCount++;
                mModifiedSize += modified.Src.RevInfo.Size;
            }

            mAddedCount = 0;
            mAddedSize = 0;
            foreach (DiffChanged added in mResultConflicts.AddsToApply)
            {
                if (added.RevInfo == null)
                    continue;

                if (added.RevInfo.Type == EnumRevisionType.enDirectory)
                    continue;

                mAddedCount++;
                mAddedSize += added.RevInfo.Size;
            }

            mDeletedCount = 0;
            mDeletedSize = 0;
            foreach (DiffChanged deleted in mResultConflicts.DeletesToApply)
            {
                if (deleted.RevInfo == null)
                    continue;

                 if (deleted.RevInfo.Type == EnumRevisionType.enDirectory)
                    continue;

                mDeletedCount++;
                mDeletedSize += deleted.RevInfo.Size;
            }

            mFileConflictCount = MergeTreeResultParser.GetUnsolvedFileConflictsCount(
                mResultConflicts, mRootMountPoint.Id, mSolvedFileConflicts);

            mDirectoryConflictCount = MergeTreeResultParser.GetUnsolvedDirectoryConflictsCount(
                    mResultConflicts);
        }

        static void DoInfoMessage(string message)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(message, UnityStyles.IncomingChangesTab.ChangesToApplySummaryLabel);

            EditorGUILayout.EndHorizontal();
        }

        static void DoErrorMessage(string message)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(message, UnityStyles.IncomingChangesTab.RedPendingConflictsOfTotalLabel);

            EditorGUILayout.EndHorizontal();
        }

        void BuildComponents(WorkspaceInfo wkInfo)
        {
            IncomingChangesTreeHeaderState incomingChangesHeaderState =
                IncomingChangesTreeHeaderState.GetDefault();

            TreeHeaderSettings.Load(incomingChangesHeaderState,
                UnityConstants.DEVELOPER_INCOMING_CHANGES_TABLE_SETTINGS_NAME,
                (int)IncomingChangesTreeColumn.Path, true);

            mIncomingChangesTreeView = new IncomingChangesTreeView(
                wkInfo, incomingChangesHeaderState,
                IncomingChangesTreeHeaderState.GetColumnNames(),
                new IncomingChangesViewMenu(this, this));

            mIncomingChangesTreeView.Reload();
        }

        static ConflictResolutionState GetConflictResolutionState(
            DirectoryConflict directoryConflict,
            DirectoryConflictAction[] conflictActions,
            Dictionary<DirectoryConflict, ConflictResolutionState> conflictResoltionStates)
        {
            ConflictResolutionState result;

            if (conflictResoltionStates.TryGetValue(directoryConflict, out result))
                return result;

            result = ConflictResolutionState.Build(directoryConflict, conflictActions);

            conflictResoltionStates.Add(directoryConflict, result);
            return result;
        }

        static int GetPendingConflictsCount(
            List<MergeChangeInfo> selectedChangeInfos)
        {
            int result = 0;
            foreach (MergeChangeInfo changeInfo in selectedChangeInfos)
            {
                if (changeInfo.DirectoryConflict.IsResolved())
                    continue;

                result++;
            }

            return result;
        }
        bool mIsProcessMergesButtonVisible;
        bool mIsCancelMergesButtonVisible;
        bool mIsMessageLabelVisible;
        bool mIsErrorMessageLabelVisible;
        bool mHasNothingToDownload;

        bool mIsProcessMergesButtonEnabled;
        bool mIsCancelMergesButtonEnabled;
        bool mHasPendingDirectoryConflicts;
        bool mIsOperationRunning;

        string mProcessMergesButtonText;
        string mMessageLabelText;
        string mErrorMessageLabelText;

        int mModifiedCount;
        long mModifiedSize;
        int mAddedCount;
        long mAddedSize;
        int mDeletedCount;
        long mDeletedSize;
        int mFileConflictCount;
        int mDirectoryConflictCount;

        IncomingChangesTreeView mIncomingChangesTreeView;

        MergeTreeResult mResultConflicts;
        MergeSolvedFileConflicts mSolvedFileConflicts;
        MountPointWithPath mRootMountPoint;

        Dictionary<DirectoryConflict, ConflictResolutionState> mConflictResolutionStates =
            new Dictionary<DirectoryConflict, ConflictResolutionState>();

        readonly ProgressControlsForViews mProgressControls;
        readonly MergeViewLogic mMergeViewLogic;
        readonly MergeController mMergeController;
        readonly GuiMessage.IGuiMessage mGuiMessage;
        readonly EditorWindow mParentWindow;
        readonly NewIncomingChangesUpdater mNewIncomingChangesUpdater;
        readonly IViewSwitcher mSwitcher;
        readonly IWorkspaceWindow mWorkspaceWindow;
        readonly WorkspaceInfo mWkInfo;
    }
}