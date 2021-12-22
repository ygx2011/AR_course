using System;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
    interface IGooglePlayConfigurationInternal
    {
        void NotifyInitializationConnectionFailed();
        void NotifyDeferredPurchase(IStoreCallback storeCallback, string productId, string receipt, string transactionId);
        void NotifyDeferredProrationUpgradeDowngradeSubscription(IStoreCallback storeCallback, string productId);
    }
}
