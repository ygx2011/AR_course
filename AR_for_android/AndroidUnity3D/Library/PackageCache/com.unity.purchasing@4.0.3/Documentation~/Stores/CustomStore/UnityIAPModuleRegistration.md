Registering your store
======================

Call the ``RegisterStore`` method supplying a name for your store and your implementation, which must implement the IStore interface.

````
public override void Configure() {
    RegisterStore(“GooglePlay”, InstantiateMyStore());
}

private void InstantiateMyStore() {
    if (Application.platform == RuntimePlatform.Android) {
        return new MyAlternativeGooglePlayImplementation ();
    }
    return null;
}
````

The store name must match the name developers use when defining products for your store so Unity IAP uses the correct product identifiers when addressing your store.


