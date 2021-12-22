Initialization
==============

Your store’s ``Initialize`` method is called by Unity IAP with an ``IStoreCallback`` that your store uses to communicate back to Unity IAP asynchronously.

````
void Initialize(IStoreCallback callback) {
    // Keep a reference to the callback for communicating with Unity IAP.
    this.callback = callback;
}
````

