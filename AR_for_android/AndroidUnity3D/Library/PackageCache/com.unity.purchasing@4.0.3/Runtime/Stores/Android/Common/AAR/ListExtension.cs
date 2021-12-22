using System.Collections.Generic;

namespace UnityEngine.Purchasing
{
    static class ListExtension
    {
        internal static AndroidJavaObject ToJava(this List<string> values)
        {
            return ToJavaArray(values);
        }

        static AndroidJavaObject ToJavaArray(List<string> values)
        {
            AndroidJavaObject list = new AndroidJavaObject("java.util.ArrayList");
            foreach (string value in values)
            {
                list.Call<bool>("add", value);
            }
            return list;
        }
    }
}
