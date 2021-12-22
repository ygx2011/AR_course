using System;

namespace UnityEngine.Purchasing.Interfaces
{
    interface IBillingClientStateListener
    {
        void RegisterOnConnected(Action onConnected);
        void RegisterOnDisconnected(Action onDisconnected);
    }
}
