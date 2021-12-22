# Testing

### Sandbox testing in Amazon

To use Amazon’s local Sandbox testing app, generate a JSON description of your product catalog on the device’s SD card using the `IAmazonConfiguration` extended configuration:

````
var builder = ConfigurationBuilder.Instance(
StandardPurchasingModule.Instance());
// Define your products.
builder.AddProduct("someConsumable", ProductType.Consumable);
// Write a product description to the SD card 
// in the appropriate location.
builder.Configure<IAmazonConfiguration>()
	.WriteSandboxJSON(builder.products);
````

When using this method to write product descriptions to the SD card, declare the Android permission to write to external storage in the test app’s manifest:

````
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" /> 
````

Remove this extra permission before publishing, if appropriate. 

Amazon Sandbox is now set up for local testing. For more information, please see Amazon's [App Tester documentation](https://developer.amazon.com/public/apis/earn/in-app-purchasing/docs-v2/installing-and-configuring-app-tester).