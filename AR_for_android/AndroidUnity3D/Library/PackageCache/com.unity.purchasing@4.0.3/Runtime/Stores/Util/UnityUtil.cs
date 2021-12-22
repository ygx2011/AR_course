using System;
using System.Collections;
using System.Collections.Generic;
using Uniject;

namespace UnityEngine.Purchasing.Extension
{
	[HideInInspector]
	[AddComponentMenu("")]
	internal class UnityUtil : MonoBehaviour, IUtil
	{
		private static List<Action> s_Callbacks = new List<Action>();
		private static volatile bool s_CallbacksPending;

		private static List<RuntimePlatform> s_PcControlledPlatforms = new List<RuntimePlatform>
		{
			RuntimePlatform.LinuxPlayer,
			RuntimePlatform.OSXEditor,
			RuntimePlatform.OSXPlayer,
			RuntimePlatform.WindowsEditor,
			RuntimePlatform.WindowsPlayer,
		};

		public T[] GetAnyComponentsOfType<T>() where T : class
		{
			GameObject[] objects = (GameObject[]) FindObjectsOfType(typeof (GameObject));
			List<T> result = new List<T>();
			foreach (GameObject o in objects)
			{
				foreach (MonoBehaviour mono in o.GetComponents<MonoBehaviour>())
				{
					if (mono is T)
						result.Add(mono as T);
				}
			}

			return result.ToArray();
		}

		public DateTime currentTime
		{
			get { return DateTime.Now; }
		}

		public string persistentDataPath
		{
			get { return Application.persistentDataPath; }
		}

        /// <summary>
        /// WARNING: Reading from this may require special application privileges.
        /// </summary>
		public string deviceUniqueIdentifier {
			get { return SystemInfo.deviceUniqueIdentifier; }
		}

		public string unityVersion {
			get { return Application.unityVersion; }
		}

		public string cloudProjectId {
			get { return Application.cloudProjectId; }
		}

		public string userId {
			get { return PlayerPrefs.GetString("unity.cloud_userid", String.Empty); }
		}

		public string gameVersion {
			get { return Application.version; }
		}

		public UInt64 sessionId {
			get { return UInt64.Parse(PlayerPrefs.GetString("unity.player_sessionid", "0")); }
		}

		public RuntimePlatform platform
		{
			get { return Application.platform; }
		}

		public bool isEditor
		{
			get { return Application.isEditor; }
		}

		public string deviceModel
		{
			get { return SystemInfo.deviceModel; }
		}

		public string deviceName
		{
			get { return SystemInfo.deviceName; }
		}

		public DeviceType deviceType
		{
			get { return SystemInfo.deviceType; }
		}

		public string operatingSystem
		{
			get { return SystemInfo.operatingSystem; }
		}

        public int screenWidth
        {
            get { return Screen.width; }
        }

        public int screenHeight
        {
            get { return Screen.height; }
        }

        public float screenDpi
        {
            get { return Screen.dpi; }
        }

        public string screenOrientation
        {
            get { return Screen.orientation.ToString(); }
        }

		object IUtil.InitiateCoroutine(IEnumerator start)
		{
			return StartCoroutine(start);
		}

		void IUtil.InitiateCoroutine(IEnumerator start, int delay)
		{
			DelayedCoroutine(start, delay);
		}

		public void RunOnMainThread(Action runnable)
		{
			lock (s_Callbacks)
			{
				s_Callbacks.Add(runnable);
				s_CallbacksPending = true;
			}
		}

		public object GetWaitForSeconds(int seconds)
		{
			return new WaitForSeconds(seconds);
		}

		private void Start()
		{
			DontDestroyOnLoad(gameObject);
		}

		public static T FindInstanceOfType<T>() where T : MonoBehaviour
		{
			return (T) FindObjectOfType(typeof (T));
		}

		public static T LoadResourceInstanceOfType<T>() where T : MonoBehaviour
		{
			return ((GameObject) Instantiate(Resources.Load(typeof (T).ToString()))).GetComponent<T>();
		}

		public static bool PcPlatform()
		{
			return s_PcControlledPlatforms.Contains(Application.platform);
		}

		private IEnumerator DelayedCoroutine(IEnumerator coroutine, int delay)
		{
			yield return new WaitForSeconds(delay);
			StartCoroutine(coroutine);
		}

		private void Update()
		{
			if (!s_CallbacksPending)
				return;
			// We copy our actions to another array to avoid
			// locking the queue whilst we process them.
			Action[] copy;
			lock (s_Callbacks)
			{
				if (s_Callbacks.Count == 0)
					return;

				copy = new Action[s_Callbacks.Count];
				s_Callbacks.CopyTo(copy);
				s_Callbacks.Clear();
				s_CallbacksPending = false;
			}

			foreach (var action in copy)
				action();
		}

		private List<Action<bool>> pauseListeners = new List<Action<bool>>();
		public void AddPauseListener(Action<bool> runnable) {
			pauseListeners.Add(runnable);
		}

		public void OnApplicationPause(bool paused) {
			foreach (var listener in pauseListeners) {
				listener(paused);
			}
		}

	    public bool IsClassOrSubclass(Type potentialBase, Type potentialDescendant)
	    {
	        return potentialDescendant.IsSubclassOf(potentialBase) || potentialDescendant == potentialBase;
	    }
	}
}
