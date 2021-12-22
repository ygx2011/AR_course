using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace UnityEngine.Purchasing.Default
{
    /// <summary>
    /// A utility class to parse Universal Windows Platform products from XML data.
    /// </summary>
    public class XMLUtils
    {
        /// <summary>
        /// A utility class to parse Universal Windows Platform products from XML data.
        /// </summary>
        /// <param name="appReceipt"> The app receipt as raw XML data. </param>
        /// <returns> The <c>TransactionInfo</c> for the products parsed from the XML data. </returns>
        public static IEnumerable<TransactionInfo> ParseProducts(string appReceipt)
        {
            if (null == appReceipt)
            {
                return new List<TransactionInfo>();
            }

            try
            {
                var xml = XElement.Parse(appReceipt);
                return from product in xml.Descendants("ProductReceipt")
                       select new TransactionInfo() {
                           productId = (string)product.Attribute("ProductId"),
                           transactionId = (string)product.Attribute("Id")
                       };
            }
            catch (XmlException)
            {
                return new List<TransactionInfo>();
            }
        }

        /// <summary>
        /// Data of a transaction for Universal Windows Platform product purchases.
        /// </summary>
        public class TransactionInfo
        {
            /// <summary>
            /// The ID of the product.
            /// </summary>
            public string productId;

            /// <summary>
            /// The transaction ID for the purchase.
            /// </summary>
            public string transactionId;
        }
    }
}
