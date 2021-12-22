using UnityEngine.Purchasing.Interfaces;

namespace UnityEngine.Purchasing
{
    class GooglePlayStorePurchaseService : IGooglePlayStorePurchaseService
    {
        IGooglePlayStoreService m_GooglePlayStoreService;
        internal GooglePlayStorePurchaseService(IGooglePlayStoreService googlePlayStoreService)
        {
            m_GooglePlayStoreService = googlePlayStoreService;
        }

        public void Purchase(ProductDefinition product)
        {
            m_GooglePlayStoreService.Purchase(product);
        }
    }
}
