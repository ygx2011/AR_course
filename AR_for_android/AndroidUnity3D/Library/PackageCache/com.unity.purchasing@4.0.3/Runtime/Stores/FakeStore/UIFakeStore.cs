using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

using Uniject;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

namespace UnityEngine.Purchasing
{
	/// <summary>
	/// User interface fake store.
	/// </summary>
	internal class UIFakeStore : FakeStore
	{
		const string EnvironmentDescriptionPostfix = "\n\n[Environment: FakeStore]";
		const string SuccessString = "Success";
		const int RetrieveProductsDescriptionCount = 2;

		DialogRequest m_CurrentDialog;
		int m_LastSelectedDropdownIndex;

        GameObject m_UIFakeStoreWindowObject;

		GameObject m_EventSystem; // Dynamically created. Auto-null'd by UI system.

		#pragma warning disable 0414
		IUtil m_Util;
		#pragma warning restore 0414

		public UIFakeStore() {
		}

		public UIFakeStore (IUtil util)
		{
			m_Util = util;
		}

		/// <summary>
		/// Creates and displays a modal dialog UI. Note pointer events can "drill through" the
		/// UI activating underlying interface elements. Consider using techniques mentioned in
		/// http://forum.unity3d.com/threads/frequently-asked-ui-questions.264479/ in apps
		/// to mitigate this. Shows only one at a time.
		/// </summary>
		/// <returns><c>true</c>, if UI was started, <c>false</c> otherwise.</returns>
		/// <param name="model">Store model being shown; uses dialogType to decode.</param>
		/// <param name="dialogType">Dialog type.</param>
		/// <param name="callback">Callback called when dialog dismissed.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		protected override bool StartUI<T>(object model, DialogType dialogType, Action<bool,T> callback)
		{
			List<string> options = new List<string>();
			// Add a default option for "Success"
			options.Add(SuccessString);

			foreach (T code in Enum.GetValues(typeof(T)))
			{
				options.Add(code.ToString());
			}

			Action<bool,int> callbackWrapper = new Action<bool,int> ((bool result, int codeValue) => {
				// TRICKY: Would prefer to use .NET 4+'s dynamic keyword over double-casting to what I know is an enum type.
				T value = (T)(object)codeValue;
				callback (result, value);
			});

			string title = null, okayButton = null, cancelButton = null;
			if (dialogType == DialogType.Purchase)
			{
				title = CreatePurchaseQuestion ((ProductDefinition)model);
				if (UIMode == FakeStoreUIMode.DeveloperUser)
				{
					// Developer UIMode is one button, one option menu, so the button must support both pass and fail
					okayButton = "OK";
				}
				else
				{
					okayButton = "Buy";
				}
			}
			else if (dialogType == DialogType.RetrieveProducts)
			{
				title = CreateRetrieveProductsQuestion ((ReadOnlyCollection<ProductDefinition>)model);
				okayButton = "OK";
			}
			else
			{
				Debug.LogError ("Unrecognized DialogType " + dialogType);
			}
			cancelButton = "Cancel";

			return StartUI (title, okayButton, cancelButton, options, callbackWrapper);
		}

		/// <summary>
		/// Helper
		/// </summary>
		bool StartUI(string queryText, string okayButtonText, string cancelButtonText,
			List<string> options, Action<bool,int> callback)
		{
			// One dialog at a time please
			if (IsShowingDialog())
			{
				return false;
			}

			// Wrap this dialog request for later use
			DialogRequest dr = new DialogRequest ();
			dr.QueryText = queryText;
			dr.OkayButtonText = okayButtonText;
			dr.CancelButtonText = cancelButtonText;
			dr.Options = options;
			dr.Callback = callback;

			m_CurrentDialog = dr;

            InstantiateDialog();

			return true;
		}

        private void InstantiateDialog()
        {
            if (m_CurrentDialog != null)
            {
                var runtimeCanvas = GetOrCreateFakeStoreWindow();

                AddLifeCycleNotifierAndSetDestroyCallback(runtimeCanvas.gameObject);
                EnsureEventSystemCreated(runtimeCanvas.transform);
                ConfigureDialogWindow(runtimeCanvas);
            }
            else
            {
                Debug.LogError(this + " requires m_CurrentDialog. Not showing dialog.");
            }
        }

        private UIFakeStoreWindow GetOrCreateFakeStoreWindow()
        {
            if (m_UIFakeStoreWindowObject == null)
            {
                m_UIFakeStoreWindowObject = new GameObject("UIFakeStoreWindow", typeof(Transform));
                m_UIFakeStoreWindowObject.AddComponent<UIFakeStoreWindow>();
            }

            return m_UIFakeStoreWindowObject.GetComponent<UIFakeStoreWindow>();
        }

        private void AddLifeCycleNotifierAndSetDestroyCallback(GameObject gameObject)
        {
            LifecycleNotifier notifier = gameObject.AddComponent<LifecycleNotifier>() as LifecycleNotifier;
            notifier.OnDestroyCallback = () =>
            {
                m_CurrentDialog = null;
            };
        }

        private void EnsureEventSystemCreated(Transform rootTransform)
        {
            if (Object.FindObjectOfType<EventSystem>() == null)
            {
                CreateEventSystem(rootTransform);
            }
        }

        private void ConfigureDialogWindow(UIFakeStoreWindow dialogWindow)
        {
            bool doCancel = (UIMode != FakeStoreUIMode.DeveloperUser);
            bool doDropDown = (UIMode != FakeStoreUIMode.StandardUser);

            dialogWindow.ConfigureMainDialogText(m_CurrentDialog.QueryText, m_CurrentDialog.OkayButtonText, m_CurrentDialog.CancelButtonText);

            if (doDropDown)
            {
                dialogWindow.ConfigureDropdownOptions(m_CurrentDialog.Options);
            }

            ConfigureDialogWindowCallbacks(dialogWindow, doCancel, doDropDown);
        }

        void ConfigureDialogWindowCallbacks(UIFakeStoreWindow dialogWindow, bool assignCancelCallback, bool assignDropDownCallback)
        {
            Action cancelAction = null;
            Action<int> dropdownAction = null;

            if (assignCancelCallback)
            {
                cancelAction = this.CancelButtonClicked;
            }

            if (assignDropDownCallback)
            {
                dropdownAction = this.DropdownValueChanged;
            }

            dialogWindow.AssignCallbacks(this.OkayButtonClicked, cancelAction, dropdownAction);
        }

        private void CreateEventSystem(Transform rootTransform)
        {
            m_EventSystem = new GameObject("EventSystem", typeof(EventSystem));
            m_EventSystem.AddComponent<StandaloneInputModule>();
            m_EventSystem.transform.parent = rootTransform;
        }

		private string CreatePurchaseQuestion(ProductDefinition definition)
		{
			return "Do you want to Purchase " + definition.id + "?" + EnvironmentDescriptionPostfix;
		}

		private string CreateRetrieveProductsQuestion(ReadOnlyCollection<ProductDefinition> definitions)
		{
			string title = "Do you want to initialize purchasing for products {";
			title += string.Join(", ", definitions.Take(RetrieveProductsDescriptionCount).Select(pid => pid.id).ToArray());
			if (definitions.Count > RetrieveProductsDescriptionCount)
			{
				title += ", ...";
			}
			title += "}?" + EnvironmentDescriptionPostfix;

			return title;
		}

		/// <summary>
		/// Positive button clicked. For yes/no dialog will send true message. For
		/// multiselect (FakeStoreUIMode.DeveloperUser) dialog may send true or false
		/// message, along with chosen option.
		/// </summary>
		private void OkayButtonClicked()
		{
			bool result = false;

			// Return false if the user chose something other than Success, and is in Development mode.
			// True if the "Success" option was chosen, or if this is non-Development mode.
			if (m_LastSelectedDropdownIndex == 0 || UIMode != FakeStoreUIMode.DeveloperUser)
			{
				// Ensure we return true
				result = true;
			}

			int codeValue = Math.Max(0, m_LastSelectedDropdownIndex - 1); // Pop SuccessString

			m_CurrentDialog.Callback(result, codeValue);
			CloseDialog();
		}

		/// <summary>
		/// Negative button clicked. Sends false message.
		/// </summary>
		private void CancelButtonClicked()
		{
			int codeValue = Math.Max(0, m_LastSelectedDropdownIndex - 1); // Pop SuccessString

			// ASSUME: This is FakeStoreUIMode.StandardUser
			m_CurrentDialog.Callback(false, codeValue);
			CloseDialog();
		}

		private void DropdownValueChanged(int selectedItem)
		{
			m_LastSelectedDropdownIndex = selectedItem;
		}

		private void CloseDialog()
		{
			m_CurrentDialog = null;

            if (m_UIFakeStoreWindowObject != null)
            {
                GameObject.Destroy(m_UIFakeStoreWindowObject);
            }
		}

		public bool IsShowingDialog()
		{
			return m_CurrentDialog != null;
		}
	}
}
