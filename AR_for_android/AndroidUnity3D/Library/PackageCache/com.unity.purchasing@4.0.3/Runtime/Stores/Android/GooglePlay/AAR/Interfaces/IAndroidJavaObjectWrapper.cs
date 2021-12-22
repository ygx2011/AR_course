using System;

namespace UnityEngine.Purchasing.Utils
{
    interface IAndroidJavaObjectWrapper
    {
        ReturnType Call<ReturnType>(string methodName, params object[] args);
    }
}
