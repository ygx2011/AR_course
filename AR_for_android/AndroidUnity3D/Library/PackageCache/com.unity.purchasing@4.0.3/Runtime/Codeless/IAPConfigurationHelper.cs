using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Helps to set up ConfigurationBuilder.
    /// </summary>
    public static class IAPConfigurationHelper
    {
        /// <summary>
        /// Populate a ConfigurationBuilder with products from a ProductCatalog
        /// </summary>
        /// <param name="builder">Will be populated with product identifiers and payouts</param>
        /// <param name="catalog">Source of product identifiers and payouts</param>
        public static void PopulateConfigurationBuilder(ref ConfigurationBuilder builder, ProductCatalog catalog)
        {
            foreach (var product in catalog.allValidProducts)
            {
                IDs ids = null;

                if (product.allStoreIDs.Count > 0)
                {
                    ids = new IDs();
                    foreach (var storeID in product.allStoreIDs)
                    {
                        ids.Add(storeID.id, storeID.store);
                    }
                }

#if UNITY_2017_2_OR_NEWER

                var payoutDefinitions = new List<PayoutDefinition>();
                foreach (var payout in product.Payouts) {
                    payoutDefinitions.Add(new PayoutDefinition(payout.typeString, payout.subtype, payout.quantity, payout.data));
                }
                builder.AddProduct(product.id, product.type, ids, payoutDefinitions.ToArray());

#else

                builder.AddProduct(product.id, product.type, ids);

#endif
            }
        }
    }
}
