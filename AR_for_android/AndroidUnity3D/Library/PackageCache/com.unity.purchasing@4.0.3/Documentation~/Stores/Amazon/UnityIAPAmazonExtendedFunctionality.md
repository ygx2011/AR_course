# Extensions

## Extended functionality

### Amazon User ID

To fetch the current Amazon User ID for other Amazon services, use the `IAmazonExtensions`:

````
public void OnInitialized
    (IStoreController controller, IExtensionProvider extensions)
{
    string amazonUserId = 
        extensions.GetExtension<IAmazonExtensions>().amazonUserId;
    // ...
}
````

