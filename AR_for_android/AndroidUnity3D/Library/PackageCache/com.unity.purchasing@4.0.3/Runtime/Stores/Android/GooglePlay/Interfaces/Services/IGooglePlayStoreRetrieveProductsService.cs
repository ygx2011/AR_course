using System.Collections.ObjectModel;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
    interface IGooglePlayStoreRetrieveProductsService
    {
        void SetStoreCallback(IStoreCallback storeCallback);
        void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products);
        void ResumeConnection();
    }
}
