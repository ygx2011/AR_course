namespace UnityEngine.Purchasing.Default {
    /// <summary>
    /// A common format for Billing Subsystems to use to
    /// describe available In App Purchases to the Biller,
    /// including purchase state via Receipt and Transaction
    /// Identifiers.
    /// </summary>
    public class WinProductDescription {
        /// <summary>
        /// The product's specific ID on the Windows Store.
        /// </summary>
        public string platformSpecificID { get; private set; }

        /// <summary>
        /// The product's price, as text.
        /// </summary>
        public string price { get; private set; }

        /// <summary>
        /// The product's title, or name.
        /// </summary>
        public string title { get; private set; }

        /// <summary>
        /// The product's description.
        /// </summary>
        public string description { get; private set; }

        /// <summary>
        /// The product's currency code.
        /// </summary>
        public string ISOCurrencyCode { get; private set; }

        /// <summary>
        /// The product's price, as a decimal.
        /// </summary>
        public decimal priceDecimal { get; private set; }

        /// <summary>
        /// The product's purchase receipt.
        /// </summary>
        public string receipt { get; private set; }

        /// <summary>
        /// The product's purchase transaction ID.
        /// </summary>
        public string transactionID { get; private set; }

        /// <summary>
        /// Whether or not the product is consumable.
        /// </summary>
        public bool consumable { get; private set; }

        /// <summary>
        /// Constructor which initialized member from parametars.
        /// </summary>
        /// <param name="id"> The product's specific ID on the Windows Store. </param>
        /// <param name="price"> The product's price, as text. </param>
        /// <param name="title"> The product's title, or name. </param>
        /// <param name="description"> The product's description. </param>
        /// <param name="isoCode"> The product's currency code. </param>
        /// <param name="priceD"> The product's price, as a decimal. </param>
        /// <param name="receipt"> The product's purchase receipt. </param>
        /// <param name="transactionId"> The product's purchase transaction ID. </param>
        /// <param name="consumable"> Whether or not the product is consumable. </param>
        public WinProductDescription (string id, string price, string title, string description,
                                   string isoCode, decimal priceD, string receipt = null, string transactionId = null, bool consumable = false) {
            platformSpecificID = id;
            this.price = price;
            this.title = title;
            this.description = description;
            this.ISOCurrencyCode = isoCode;
            this.priceDecimal = priceD;
            this.receipt = receipt;
            this.transactionID = transactionId;
            this.consumable = consumable;
        }
    }
}
