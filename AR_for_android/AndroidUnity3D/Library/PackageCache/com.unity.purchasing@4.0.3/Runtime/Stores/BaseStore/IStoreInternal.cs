using System;
using System.Collections.Generic;

namespace UnityEngine.Purchasing
{
    internal interface IStoreInternal
    {
        // Internal mechanism for informing the store about the SPM (formerly used via reflection)
        void SetModule(StandardPurchasingModule module);

    }
}
