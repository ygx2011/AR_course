using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Purchasing
{
    class GoogleCachedQuerySkuDetailsService: IGoogleCachedQuerySkuDetailsService
    {
        Dictionary<string, AndroidJavaObject> m_CachedQueriedSkus = new Dictionary<string, AndroidJavaObject>();

        public IEnumerable<AndroidJavaObject> GetCachedQueriedSkus()
        {
            return m_CachedQueriedSkus.Values;
        }

        AndroidJavaObject GetCachedQueriedSku(string sku)
        {
            return m_CachedQueriedSkus[sku];
        }

        IEnumerable<AndroidJavaObject> GetCachedQueriedSkus(IEnumerable<string> skus)
        {
            return skus.Select(GetCachedQueriedSku);
        }

        public IEnumerable<AndroidJavaObject> GetCachedQueriedSkus(IEnumerable<ProductDefinition> products)
        {
            return GetCachedQueriedSkus(products.Select(product => product.storeSpecificId));
        }

        bool Contains(string sku)
        {
            return m_CachedQueriedSkus.ContainsKey(sku);
        }

        public bool Contains(ProductDefinition products)
        {
            return Contains(products.storeSpecificId);
        }

        public void AddCachedQueriedSkus(IEnumerable<AndroidJavaObject> queriedSkus)
        {
            foreach (var queriedSkuDetails in queriedSkus)
            {
                var queriedSku = queriedSkuDetails.Call<string>("getSku");
                m_CachedQueriedSkus[queriedSku] = queriedSkuDetails;
            }
        }
    }
}
