using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace UnityEditor.Purchasing
{
    static class NetworkingUtils
    {
        internal static string GetProjectGuid()
        {
            return CloudProjectSettings.projectId;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        internal static string GetValueFromJsonDictionary(string rawJson, string key)
        {
            var container = (Dictionary<string, object>)MiniJson.JsonDecode(rawJson);

            object value;
            container.TryGetValue(key, out value);
            return value as string;
        }
    }
}
