# Fetching Additional Products

This step is optional.

If your Application has a very large catalog of available products, you may find it takes too long to fetch them all in one batch.

An alternative is to initialise Unity IAP with an initial set of products, then fetch additional products in batches using the FetchAdditionalProducts method of the IStoreController:

````
/// <summary>
/// This will be called when Unity IAP has finished initialising.
/// </summary>
public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
{
	var additional = new HashSet<ProductDefinition>() {
		new ProductDefinition("coins.500", ProductType.Consumable),
		new ProductDefinition("armour", ProductType.NonConsumable) 
	};

	Action onSuccess = () => {
		Debug.Log("Fetched successfully!");
		// The additional products are added to the set of
		// previously retrieved products and are browseable
		// and purchasable.
		foreach (var product in controller.products.all) {
			Debug.Log(product.definition.id);
		}
	};

	Action<InitializationFailureReason> onFailure = (InitializationFailureReason i) => {
		Debug.Log("Fetching failed for the specified reason: " + i);
	};

	controller.FetchAdditionalProducts(additional, onSuccess, onFailure);
}
````

Once additional products have been fetched they are added to the set of existing products previously retrieved and may be iterated over and purchased.

FetchAdditionalProducts behaves similarly to initialization:

* If network is unavailable it will wait until network become available to retrieve products
* It will fail for the same unrecoverable reasons initialization can fail, such as IAP being disabled in device settings

**You must not call FetchAdditionalProducts while a previous call to it is still pending.**
