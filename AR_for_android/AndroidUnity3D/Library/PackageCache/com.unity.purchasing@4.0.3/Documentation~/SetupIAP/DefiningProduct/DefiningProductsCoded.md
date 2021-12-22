# Defining Products in scripts

You can declare your Product list programmatically using the [Purchasing Configuration Builder](xref:UnityEngine.Purchasing.ConfigurationBuilder). 
You must provide a unique cross-store __Product ID__ and __Product Type__ for each Product:

````
using UnityEngine;
using UnityEngine.Purchasing;

public class MyIAPManager {
    public MyIAPManager () {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("100_gold_coins", ProductType.Consumable);
        // Initialize Unity IAP...
    }
}
````
