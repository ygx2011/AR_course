namespace UnityEngine.Purchasing
{
    internal static class UnifiedReceiptFormatter
    {
        internal static string FormatUnifiedReceipt(string platformReceipt, string transactionId, string storeName)
        {
            UnifiedReceipt unifiedReceipt = new UnifiedReceipt()
            {
                Store = storeName,
                TransactionID = transactionId,
                Payload = platformReceipt
            };
            return JsonUtility.ToJson(unifiedReceipt);
        }
    }
}
