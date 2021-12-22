using System;
using System.Collections;
using UnityEngine;

namespace Uniject
{
    /// <summary>
    /// An interface allowing Engine APIs to be stubbed out
    /// because unit tests do not have access to Engine APIs.
    /// </summary>
    internal interface IUtil
    {
        RuntimePlatform platform { get; }
        bool isEditor { get; }
        string persistentDataPath { get; }
        string cloudProjectId { get; }
        /// <summary>
        /// WARNING: Reading from this may require special application privileges.
        /// </summary>
        string deviceUniqueIdentifier { get; }
        string unityVersion { get;  }
        string userId { get; }
        string gameVersion { get; }
        UInt64 sessionId { get; }
        DateTime currentTime { get; }
        string deviceModel { get; }
        string deviceName { get; }
        DeviceType deviceType { get; }
        string operatingSystem { get; }
        int screenWidth { get; }
        int screenHeight { get; }
        float screenDpi { get; }
        string screenOrientation { get; }

        T[] GetAnyComponentsOfType<T>() where T : class;
        object InitiateCoroutine(IEnumerator start);
        object GetWaitForSeconds(int seconds);
        void InitiateCoroutine(IEnumerator start, int delayInSeconds);
        void RunOnMainThread(Action runnable);
        // Have the specified action called when the app is paused or resumed.
        // The parameter is true if paused, false if resuming.
        // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationPause.html
        void AddPauseListener(Action<bool> runnable);
        /// <summary>
        /// Returns true if potentialDescendant is-a potentialBase (or a subclass of potentialBase).
        /// </summary>
        /// <param name="potentialBase"></param>
        /// <param name="potentialDescendant"></param>
        /// <returns></returns>
        bool IsClassOrSubclass(Type potentialBase, Type potentialDescendant);
    }
}
