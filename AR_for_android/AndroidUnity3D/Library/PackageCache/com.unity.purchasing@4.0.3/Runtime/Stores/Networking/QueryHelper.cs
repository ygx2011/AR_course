using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.Purchasing
{
    internal static class QueryHelper
    {
        internal static string ToQueryString(this Dictionary<string, object> parameters)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string key in parameters.Keys) {
                string val = parameters[key].ToString();

                if (val == null)
                    continue;

                sb.Append(sb.Length == 0 ? "?" : "&");
                sb.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(val));
            }
            return sb.ToString();
        }
    }
}
