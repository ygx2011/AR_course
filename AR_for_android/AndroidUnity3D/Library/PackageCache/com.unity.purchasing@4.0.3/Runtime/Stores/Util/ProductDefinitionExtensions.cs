using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Collections.Generic;

namespace UnityEngine.Purchasing
{
    internal static class ProductDefinitionExtensions
    {
        /// <summary>
        /// Decodes the list of json objects for a storename.
        /// </summary>
        /// <returns>Hashset of ProductDefinitions</returns>
        /// <param name="productsList">Products list.</param>
        /// <param name="storeName">Store name.</param>
        internal static List<ProductDefinition> DecodeJSON(this List<object> productsList, string storeName)
        {
            var result = new List<ProductDefinition>();
            try
            {
                foreach (object product in productsList)
                {
                    var productDict = (Dictionary<string, object>)product;
                    object id, storeIDs, typeString;
                    productDict.TryGetValue("id", out id);
                    productDict.TryGetValue("store_ids", out storeIDs);
                    productDict.TryGetValue("type", out typeString);
                    var idHash = storeIDs as Dictionary<string, object>;
                    string storeSpecificId = (string)id;
                    if (idHash != null)
                    {
                        foreach (var storeInfo in idHash)
                        {
                            var storeKey = storeInfo.Key.ToLower();
                            var storeValue = (string)storeInfo.Value;
                            if (!String.IsNullOrEmpty(storeValue) && storeName.ToLower() == storeKey)
                            {
                                storeSpecificId = storeValue;
                            }
                        }
                    }
                    else
                    {
                        // Handles scenario where developer creates a single storeSpecificID via ProductDefinition
                        // and through FakeStore within editor adds ProductDefinition to ConfigurationBuilder
                        object singleStoreSpecificID;
                        productDict.TryGetValue("storeSpecificId", out singleStoreSpecificID);
                        string generalStoreSpecificStringID = (string)singleStoreSpecificID;
                        if (generalStoreSpecificStringID != null)
                        {
                            storeSpecificId = generalStoreSpecificStringID;
                        }
                    }
                    var type = (ProductType)Enum.Parse(typeof(ProductType), (string)typeString);
                    var definition = new ProductDefinition((string)id, storeSpecificId, type);
                    result.Add(definition);
                }
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
