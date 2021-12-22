# Purchase Receipt

All receipts have the base receipt information found [here](../../SetupIAP/PurchasingFlow/UnityIAPPurchaseReceipts.md).

iOS
---

Payload varies depending upon the device's iOS version.

|iOS version|Payload|
|:---|:---|
|__iOS &gt;= 7__|payload is a base 64 encoded [App Receipt](https://developer.apple.com/library/ios/releasenotes/General/ValidateAppStoreReceipt/Chapters/ReceiptFields.html#/apple_ref/doc/uid/TP40010573-CH106-SW1).|
|__iOS &lt; 7__|payload is a [SKPaymentTransaction transactionReceipt](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1617722-transactionreceipt?language=objc).|

Mac App Store
-------------

Payload is a base 64 encoded [App Receipt](https://developer.apple.com/library/ios/releasenotes/General/ValidateAppStoreReceipt/Chapters/ReceiptFields.html#/apple_ref/doc/uid/TP40010573-CH106-SW1).