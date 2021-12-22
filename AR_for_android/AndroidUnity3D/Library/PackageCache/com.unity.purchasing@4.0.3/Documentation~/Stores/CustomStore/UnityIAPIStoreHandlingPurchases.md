Handling purchases
==================

Your Store's Purchase method is called when the user opts to make a purchase. Your store should take the user through the checkout process and call either the ``OnPurchaseSucceeded`` or ``OnPurchaseFailed`` method of the ``IStoreCallback``.

Your store should supply a receipt and unique transaction ID; if the application has not already processed a purchase with the supplied tranasaction ID, Unity IAP will invoke the application's ``ProcessPurchase`` method.

Finishing Transactions
----------------------

When the application acknowledges that a transaction has been processed, or if the transaction has already been processed, Unity IAP invokes your store’s FinishTransaction method.

Stores should use FinishTransaction to perform any housekeeping following a purchase, such as closing transactions or consuming consumable products.



