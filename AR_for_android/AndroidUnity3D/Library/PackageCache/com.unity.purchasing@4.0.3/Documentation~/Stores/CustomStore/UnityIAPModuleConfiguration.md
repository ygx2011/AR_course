Store Configuration
===================

Your store may require developers to supply additional configuration information during initialization, for which your module can register a configuration instance that implements the ``IStoreConfiguration`` interface:

````
var config = new MyConfiguration(); // Implements IStoreConfiguration
BindConfiguration<MyConfiguration>(new MyConfiguration());
````

When developers request an instance of your configuration type, Unity IAP first tries to cast your store implementation to the configuration type. Only if that cast fails will any instance bound via ``BindConfiguration`` will be used.

