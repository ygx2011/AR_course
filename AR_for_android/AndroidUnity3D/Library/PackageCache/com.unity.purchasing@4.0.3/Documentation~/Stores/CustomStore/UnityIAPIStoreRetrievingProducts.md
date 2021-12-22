Retrieving products
===================

When your store's ``RetrieveProducts`` method is called it should fetch the latest product metadata and, optionally, ownership status for the current user.

When this process completes your store should call the ``OnProductsRetrieved`` method of the ``IStoreCallback`` supplied to your store upon initialisation, supplying a collection of ``ProductDescription`` that represent the items available for purchase.

Where products are owned by the user, your store may fill in the receipt and transaction ID fields of ``ProductDescription``; Unity IAP will invoke the application’s ``ProcessPurchase`` method for any transactions the application has not already processed.

Note that if the user is offline your store should retry until the user regains connectivity, taking care to avoid impacting game performance through aggressive polling.

Handling errors
---------------

If products cannot be retrieved due to an unrecoverable error, such as the developer making an error with their store configuration, you should call the ``OnSetupFailed`` method of the ``IStoreCallback``, indicating the ``InitializationFailureReason`` responsible.

