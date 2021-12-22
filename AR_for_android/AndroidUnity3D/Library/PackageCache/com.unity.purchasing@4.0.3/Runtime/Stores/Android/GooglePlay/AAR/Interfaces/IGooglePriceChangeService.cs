using System;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing
{
    interface IGooglePriceChangeService
    {
        void PriceChange(ProductDefinition product, Action<IGoogleBillingResult> onPriceChangedListener);
    }
}
