using System.Collections.Generic;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing.Interfaces
{
    interface ISkuDetailsQueryResponse
    {
        void AddResponse(IGoogleBillingResult billingResult, IEnumerable<AndroidJavaObject> skuDetails);
        List<AndroidJavaObject> SkuDetails();
        bool IsRecoverable();
    }
}
