using System;
using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
    interface IGoogleFetchPurchases
    {
        void SetStoreCallback(IStoreCallback storeCallback);
        void FetchPurchases();
        void FetchPurchases(Action<List<Product>> onQueryPurchaseSucceed);
    }
}
