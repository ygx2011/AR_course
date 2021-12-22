Store Extensions
================

Your store may offer additional functionality that does not fit into the cross platform purchase flow, for example the ability to refresh app receipts on Apple’s stores.

You should create an interface that defines the extended functionality, itself implementing the ``IStoreExtension`` interface:

````
/// <summary>
/// Functionality specific to my store.
/// </summary>
public interface IMyExtensions : IStoreExtension
{
    // Hypothetical method for a store that provides User IDs.
    String GetUserStoreId();
}
````
Applications request extended functionality via the ``IExtensionProvider``. When they do so Unity IAP first tries to cast the active store implementation to the requested type.

If that cast fails, Unity IAP will provide any instance registered via a call your store module has provided via ``RegisterExtension``, or null if no instance has been provided.

Modules should provide instances for the extension interfaces they define even when running on unsupported platforms, so as to avoid forcing application developers to use platform dependent compilation.


