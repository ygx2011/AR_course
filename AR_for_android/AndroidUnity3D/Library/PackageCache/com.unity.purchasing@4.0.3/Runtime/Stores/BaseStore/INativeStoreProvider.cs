using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
    internal interface INativeStoreProvider
    {
        INativeStore GetAndroidStore (IUnityCallback callback, AppStore store, IPurchasingBinder binder, Uniject.IUtil util);
        INativeAppleStore GetStorekit(IUnityCallback callback);
    }
}
