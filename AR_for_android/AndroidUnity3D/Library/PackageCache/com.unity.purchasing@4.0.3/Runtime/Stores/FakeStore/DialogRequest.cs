using System;
using System.Collections.Generic;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Dialog request base model
    /// </summary>
    internal class DialogRequest
    {
        public string QueryText;
        public string OkayButtonText;
        public string CancelButtonText;
        public List<string> Options;
        public Action<bool,int> Callback;
    }
}
