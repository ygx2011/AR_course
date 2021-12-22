#if !SERVICES_SDK_CORE_ENABLED

namespace UnityEditor.Purchasing
{
    internal class NonGameServicesAnalyticsPackageKeyHolder : IAnalyticsPackageKeyHolder
    {
        public string GetPackageKey()
        {
            return PurchasingIdentifierKey.k_PurchasingKey;
        }
    }
}

#endif
