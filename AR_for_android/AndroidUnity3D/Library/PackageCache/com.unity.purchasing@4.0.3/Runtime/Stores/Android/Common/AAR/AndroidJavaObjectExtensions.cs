using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Purchasing.Models
{
    static class AndroidJavaObjectExtensions
    {
        internal static IEnumerable<T> Enumerate<T>(this AndroidJavaObject androidJavaList)
        {
            var size = androidJavaList?.Call<int>("size") ?? 0;
            return Enumerable.Range(0, size).Select(i => androidJavaList.Call<T>("get", i));
        }
    }
}
