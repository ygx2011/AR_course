namespace UnityEngine.Purchasing
{
    internal static class ProductPurchaseUpdater
    {
        internal static void UpdateProductReceiptAndTransactionID(Product product, string receipt, string transactionId, string storeName)
        {
            product.receipt =  UnifiedReceiptFormatter.FormatUnifiedReceipt(receipt, transactionId, storeName);
            product.transactionID = transactionId;
        }
    }
}
