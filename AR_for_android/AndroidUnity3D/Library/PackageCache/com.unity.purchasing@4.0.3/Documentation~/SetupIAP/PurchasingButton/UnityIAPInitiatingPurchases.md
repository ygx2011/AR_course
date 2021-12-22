# Coded

When the user wants to buy a product call the ``InitiatePurchase`` method of the ``IStoreController``, identifying the product the user wants to buy.

````
// Example method called when the user presses a 'buy' button
// to start the purchase process.
public void OnPurchaseClicked(string productId) {
    controller.InitiatePurchase(productId);
}
````

Your application will be notified asynchronously of the result, either with an invocation of ``ProcessPurchase`` for successful purchases or ``OnPurchaseFailed`` for failures.

