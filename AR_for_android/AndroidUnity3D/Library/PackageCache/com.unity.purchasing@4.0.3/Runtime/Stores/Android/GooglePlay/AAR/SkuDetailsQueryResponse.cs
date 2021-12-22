using System.Collections.Generic;
using System.Linq;
using UnityEngine.Purchasing.Interfaces;
using UnityEngine.Purchasing.Models;
namespace UnityEngine.Purchasing
{
    class SkuDetailsQueryResponse : ISkuDetailsQueryResponse
    {
        List<(IGoogleBillingResult, IEnumerable<AndroidJavaObject>)> m_Responses = new List<(IGoogleBillingResult, IEnumerable<AndroidJavaObject>)>();

        public void AddResponse(IGoogleBillingResult billingResult, IEnumerable<AndroidJavaObject> skuDetails)
        {
            m_Responses.Add((billingResult, skuDetails));
        }

        public List<AndroidJavaObject> SkuDetails()
        {
            return m_Responses.Where(response => response.Item1.responseCode == GoogleBillingResponseCode.Ok)
                .SelectMany(response => response.Item2).ToList();
        }

        public bool IsRecoverable()
        {
            return m_Responses.Select(response => response.Item1).Any(IsRecoverable);
        }

        static bool IsRecoverable(IGoogleBillingResult billingResult)
        {
            return billingResult.responseCode == GoogleBillingResponseCode.ServiceUnavailable || billingResult.responseCode == GoogleBillingResponseCode.DeveloperError;
        }
    }
}
