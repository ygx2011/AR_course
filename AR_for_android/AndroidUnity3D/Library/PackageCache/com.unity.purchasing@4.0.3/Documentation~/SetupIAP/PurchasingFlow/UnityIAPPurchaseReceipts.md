# Purchase Receipts

Unity IAP provides purchase receipts as a JSON hash containing the following keys and values:

|Key|Value|
|:---|:---|
|__Store__|The name of the store in use, such as **GooglePlay** or **AppleAppStore**|
|__TransactionID__|This transaction’s unique identifier, provided by the store|
|__Payload__|Varies by platform, details below.|
