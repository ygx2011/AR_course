using UnityEngine.Purchasing.Interfaces;

namespace UnityEngine.Purchasing
{
    class GoogleLastKnownProductService: IGoogleLastKnownProductService
    {
        string m_LastKnownProductId = null;
        GooglePlayProrationMode? m_LastKnownProrationMode = GooglePlayProrationMode.UnknownSubscriptionUpgradeDowngradePolicy;

        public string GetLastKnownProductId()
        {
            return m_LastKnownProductId;
        }

        public void SetLastKnownProductId(string lastKnownProductId)
        {
            m_LastKnownProductId = lastKnownProductId;
        }

        public GooglePlayProrationMode? GetLastKnownProrationMode()
        {
            return m_LastKnownProrationMode;
        }

        public void SetLastKnownProrationMode(GooglePlayProrationMode? lastKnownProrationMode)
        {
            m_LastKnownProrationMode = lastKnownProrationMode;
        }
    }
}
