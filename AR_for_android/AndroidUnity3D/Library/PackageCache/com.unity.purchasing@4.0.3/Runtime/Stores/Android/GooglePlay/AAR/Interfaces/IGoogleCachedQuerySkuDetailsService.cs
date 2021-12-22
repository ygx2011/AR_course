using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Purchasing
{
    interface IGoogleCachedQuerySkuDetailsService
    {
        IEnumerable<AndroidJavaObject> GetCachedQueriedSkus();
        IEnumerable<AndroidJavaObject> GetCachedQueriedSkus(IEnumerable<ProductDefinition> products);
        bool Contains(ProductDefinition products);
        void AddCachedQueriedSkus(IEnumerable<AndroidJavaObject> queriedSkus);
    }
}
