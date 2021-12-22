# Purchase Receipt

All receipts have the base receipt information found [here](../../SetupIAP/PurchasingFlow/UnityIAPPurchaseReceipts.md).

Google Play
-----------

Payload is a JSON hash with the following keys and values:

|Key|Value|
|:---|:---|
|__json__|A JSON encoded string provided by Google; [`INAPP_PURCHASE_DATA`](http://developer.android.com/google/play/billing/billing_reference.html)|
|__signature__|A signature for the json parameter, as provided by Google; [`INAPP_DATA_SIGNATURE`](http://developer.android.com/google/play/billing/billing_reference.html)|