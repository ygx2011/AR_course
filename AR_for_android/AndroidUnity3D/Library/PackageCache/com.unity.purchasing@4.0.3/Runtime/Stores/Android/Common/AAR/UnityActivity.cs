using System;

namespace UnityEngine.Purchasing
{
    class UnityActivity
    {
        const string k_AndroidClassName = "com.unity3d.player.UnityPlayer";

        static AndroidJavaClass GetUnityPlayerClass()
        {
            return new AndroidJavaClass(k_AndroidClassName);
        }

        internal static AndroidJavaObject GetCurrentActivity()
        {
            return GetUnityPlayerClass().GetStatic<AndroidJavaObject>("currentActivity");
        }
    }
}
