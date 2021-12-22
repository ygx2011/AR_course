using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.Purchasing.Security
{
    /// <summary>
    /// THIS IS A STUB, WILL NOT EXECUTE CODE!
    ///
    /// This class will validate the Apple receipt is signed with the correct certificate.
    /// </summary>
	public class AppleValidator
	{
        /// <summary>
        /// THIS IS A STUB, WILL NOT EXECUTE CODE!
        ///
        /// Constructs an instance with Apple Certificate.
        /// </summary>
        /// <param name="appleRootCertificate">The apple certificate.</param>
		public AppleValidator (byte[] appleRootCertificate)
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// THIS IS A STUB, WILL NOT EXECUTE CODE!
        ///
        /// Validate that the Apple receipt is signed correctly.
        /// </summary>
        /// <param name="receiptData">The Apple receipt to validate.</param>
        /// <returns>The parsed AppleReceipt</returns>
        /// <exception cref="InvalidSignatureException">The exception thrown if the receipt is incorrectly signed.</exception>
		public AppleReceipt Validate (byte [] receiptData)
		{
			throw new NotImplementedException();
		}
	}

    /// <summary>
    /// THIS IS A STUB, WILL NOT EXECUTE CODE!
    ///
    /// This class with parse the Apple receipt data received in byte[] into a AppleReceipt object
    /// </summary>
	public class AppleReceiptParser
	{
        /// <summary>
        /// THIS IS A STUB, WILL NOT EXECUTE CODE!
        ///
        /// Parse the Apple receipt data into a AppleReceipt object
        /// </summary>
        /// <param name="receiptData">Apple receipt data</param>
        /// <returns>The converted AppleReceipt object from the Apple receipt data</returns>
		public AppleReceipt Parse (byte [] receiptData)
		{
			throw new NotImplementedException();
		}
	}
}
