using System;
using UnityEngine;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Lifecycle notifier waits to be destroyed before calling a callback.
    /// Use to notify script of hierarchy destruction for avoiding dynamic
    /// UI hierarchy collisions.
    /// </summary>
    internal class LifecycleNotifier : MonoBehaviour
    {
        public Action OnDestroyCallback;

        void OnDestroy()
        {
            if (OnDestroyCallback != null)
            {
                OnDestroyCallback();
            }
        }
    }
}
