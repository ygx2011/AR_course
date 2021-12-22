# Restoring Transactions

When a user reinstalls your application they should be granted any Non-Consumable or renewable Subscription products they already own. App stores maintain a permanent record of each user's Non-Consumable and renewable Subscription products which Unity IAP can retrieve. Non-renewing subscriptions on Apple platforms cannot be restored. If you use non-renewing subscription products on Apple platforms, it is up to you to keep a record of the active subscriptions and sync the subscription between devices.

On platforms that support it (e.g. Google Play and Universal Windows Applications) Unity IAP automatically restores any products the user owns during the first initialization following reinstallation; the ``ProcessPurchase`` method of your ``IStoreListener`` will be called for each owned item.

On Apple platforms users must enter their password to retrieve previous transactions so your application must provide users with a button letting them do so. During this process the ``ProcessPurchase`` method of your ``IStoreListener`` will be invoked for any items the user already owns.

````
/// <summary>
/// Your IStoreListener implementation of OnInitialized.
/// </summary>
public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
{
	extensions.GetExtension<IAppleExtensions> ().RestoreTransactions (result => {
		if (result) {
			// This does not mean anything was restored,
			// merely that the restoration process succeeded.
		} else {
			// Restoration failed.
		}
	});
}
````

