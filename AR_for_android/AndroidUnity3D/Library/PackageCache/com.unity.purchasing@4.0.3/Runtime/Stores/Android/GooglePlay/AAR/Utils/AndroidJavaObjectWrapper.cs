using System;

namespace UnityEngine.Purchasing.Utils
{
    class AndroidJavaObjectWrapper : IAndroidJavaObjectWrapper
    {
        IDisposable androidJavaObject { get; }

        public AndroidJavaObjectWrapper(AndroidJavaObject obj)
        {
            androidJavaObject = obj;
        }

        public ReturnType Call<ReturnType>(string methodName, params object[] args)
        {
            var obj = (AndroidJavaObject) androidJavaObject;
            return obj.Call<ReturnType>(methodName, args);
        }
    }
}
