Store Modules
=============

Store modules extend the ``AbstractPurchasingModule`` class, acting as factories Unity IAP can use to obtain an instance of your store along with any configuration and extensions.

Developers can supply multiple modules to Unity IAP, allowing them to use your custom store implementation alongside the default Unity-provided stores:

````
ConfigurationBuilder.Instance (MyCustomModule.Instance(), StandardPurchasingModule.Instance ());
````

Where two or more modules have implementations available for a given platform, precedence is given in order the modules were supplied to the ``ConfigurationBuilder``; any implementation provided by ``MyCustomModule`` will be used in preference to ``StandardPurchasingModule``.

Note that a module can support multiple stores; the ``StandardPurchasingModule`` handles all of Unity IAPs default store implementations.

