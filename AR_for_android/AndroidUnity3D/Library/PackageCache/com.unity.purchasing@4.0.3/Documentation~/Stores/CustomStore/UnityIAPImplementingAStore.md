Implementing a Store
====================

Your store must implement the IStore interface, the methods of which are detailed in following sections.

````
using UnityEngine.Purchasing.Extension;

public class MyStore : IStore
{
    private IStoreCallback callback;
    public void Initialize (IStoreCallback callback)
    {
        this.callback = callback;   
    }

    public void RetrieveProducts (System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Purchasing.ProductDefinition> products)
    {
        // Fetch product information and invoke callback.OnProductsRetrieved();
    }

    public void Purchase (UnityEngine.Purchasing.ProductDefinition product, string developerPayload)
    {
        // Start the purchase flow and call either callback.OnPurchaseSucceeded() or callback.OnPurchaseFailed()
    }

    public void FinishTransaction (UnityEngine.Purchasing.ProductDefinition product, string transactionId)
    {
        // Perform transaction related housekeeping 
    }
}
````

