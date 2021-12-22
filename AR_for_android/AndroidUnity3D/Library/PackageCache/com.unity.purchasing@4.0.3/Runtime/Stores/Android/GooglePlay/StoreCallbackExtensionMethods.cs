using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// This class is an extension class on IStoreCallback to add the functionality to FindProductById
    /// </summary>
    public static class StoreCallbackExtensionMethods
    {
        /// <summary>
        /// Find a product by it's product ID or StoreSpecificID
        /// </summary>
        /// <param name="storeCallback">The StoreCallback to extend</param>
        /// <param name="sku">The ID to search</param>
        /// <returns>Returns the product with the ID or null</returns>
        public static Product FindProductById(this IStoreCallback storeCallback, string sku)
        {
            if (sku != null && storeCallback.products != null)
            {
                return storeCallback.products.WithID(sku) ?? storeCallback.products.WithStoreSpecificID(sku);
            }
            return null;
        }
    }
}
