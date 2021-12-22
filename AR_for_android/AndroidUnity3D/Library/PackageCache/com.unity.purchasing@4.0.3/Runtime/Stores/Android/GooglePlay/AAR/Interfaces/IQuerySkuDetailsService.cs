using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing.Interfaces
{
    interface IQuerySkuDetailsService
    {
        void QueryAsyncSku(ProductDefinition product, Action<List<AndroidJavaObject>> onSkuDetailsResponse);
        void QueryAsyncSku(ReadOnlyCollection<ProductDefinition> products, Action<List<AndroidJavaObject>> onSkuDetailsResponse);

        void QueryAsyncSku(ReadOnlyCollection<ProductDefinition> products, Action<List<ProductDescription>> onSkuDetailsResponse);
    }
}
