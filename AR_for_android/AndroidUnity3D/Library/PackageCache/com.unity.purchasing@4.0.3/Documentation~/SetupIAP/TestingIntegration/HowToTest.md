# How to Test

## Running Fake Store

To run fake store there are two ways:
1. Press Play in the Unity Editor, this will always use the fake store
2. Set `StandardPurchasingModule.Instance().useFakeStoreAlways` to `true`

## Advance Testing
The Fake Store allows the developer to chose between three options for testing purposes. This testing feature can be set with `StandardPurchasingModule.Instance().useFakeStoreUIMode`.

### 1. Default
This option display no dialog.

### 2. StandardUser
This option will show a simple dialog is shown when Purchasing.

### 3. DeveloperUser
This option will show a dialog giving options for failure reason code selection when Initializing/Retrieving Products and when Purchasing.