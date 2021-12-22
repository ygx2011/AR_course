# Browsing Product Metadata

Unity IAP retrieves localized product metadata during the initialization process, which you can access via the ``IStoreController`` products field.

````
foreach (var product in controller.products.all) {
    Debug.Log (product.metadata.localizedTitle);
    Debug.Log (product.metadata.localizedDescription);
    Debug.Log (product.metadata.localizedPriceString);
}
````

