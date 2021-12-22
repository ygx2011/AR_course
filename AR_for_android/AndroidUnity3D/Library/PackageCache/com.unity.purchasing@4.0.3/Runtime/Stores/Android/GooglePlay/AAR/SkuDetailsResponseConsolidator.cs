using System;
using System.Collections.Generic;
using UnityEngine.Purchasing.Interfaces;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing
{
    class SkuDetailsResponseConsolidator : ISkuDetailsResponseConsolidator
    {
        const int k_RequiredNumberOfCallbacks = 2;
        int m_NumberReceivedCallbacks;

        Action<ISkuDetailsQueryResponse> m_OnSkuDetailsResponseConsolidated;
        ISkuDetailsQueryResponse m_Responses = new SkuDetailsQueryResponse();

        internal SkuDetailsResponseConsolidator(Action<ISkuDetailsQueryResponse> onSkuDetailsResponseConsolidated)
        {
            m_OnSkuDetailsResponseConsolidated = onSkuDetailsResponseConsolidated;
        }

        public void Consolidate(IGoogleBillingResult billingResult, IEnumerable<AndroidJavaObject> skuDetails)
        {
            m_NumberReceivedCallbacks++;

            m_Responses.AddResponse(billingResult, skuDetails);

            if (m_NumberReceivedCallbacks >= k_RequiredNumberOfCallbacks)
            {
                m_OnSkuDetailsResponseConsolidated(m_Responses);
            }
        }
    }
}
