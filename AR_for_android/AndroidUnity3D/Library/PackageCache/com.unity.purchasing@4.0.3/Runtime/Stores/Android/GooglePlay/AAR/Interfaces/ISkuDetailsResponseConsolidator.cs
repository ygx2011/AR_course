using System.Collections.Generic;
using UnityEngine.Purchasing.Models;
namespace UnityEngine.Purchasing.Interfaces
{
    interface ISkuDetailsResponseConsolidator
    {
        void Consolidate(IGoogleBillingResult billingResult, IEnumerable<AndroidJavaObject> skuDetails);
    }
}
