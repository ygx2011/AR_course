using System;
using System.Collections.ObjectModel;

namespace UnityEngine.Purchasing
{
    internal interface INativeUDPStore : INativeStore
    {
        /// <summary>
        /// Supplants INativeStore equivalent.
        /// </summary>
        void Initialize(Action<bool, string> callback);

        /// <summary>
        /// Richly typed input params. Supplants INativeStore equivalent.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="callback"></param>
        /// <param name="developerPayload"></param>
        void Purchase(string productId, Action<bool, string> callback, string developerPayload = null);

        /// <summary>
        /// Richly typed input params. Supplants INativeStore equivalent.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="callback"></param>
        void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products, Action<bool, string> callback);

        /// <summary>
        /// Richly typed input params. Supplants INativeStore equivalent.
        /// </summary>
        /// <param name="productDefinition"></param>
        /// <param name="transactionID"></param>
        void FinishTransaction(ProductDefinition productDefinition, string transactionID);
    }
}
