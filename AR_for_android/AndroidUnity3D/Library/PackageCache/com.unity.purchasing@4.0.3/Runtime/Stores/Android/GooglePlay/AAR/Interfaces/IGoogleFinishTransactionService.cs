using System;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing.Interfaces
{
    interface IGoogleFinishTransactionService
    {
        void FinishTransaction(ProductDefinition product, string purchaseToken, Action<ProductDefinition, GooglePurchase, IGoogleBillingResult, string> onConsume, Action<ProductDefinition, GooglePurchase, IGoogleBillingResult> onAcknowledge);
    }
}
