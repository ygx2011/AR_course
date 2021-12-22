using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityEngine.Purchasing.Security
{
    /// <summary>
    /// Stub cross-platform receipt validator class for platforms not supported by this feature.
    /// Will always throw exceptions if used.
    /// </summary>
    public class CrossPlatformValidator
    {
        /// <summary>
        /// Constructs an instance and checks the validity of the certification keys
        /// which only takes input parameters for the supported platforms and uses a common bundle ID for Apple and GooglePlay.
        /// </summary>
        /// <param name="googlePublicKey"> The GooglePlay public key. </param>
        /// <param name="appleRootCert"> The Apple certification key. </param>
        /// <param name="appBundleId"> The bundle ID for all platforms. </param>
        public CrossPlatformValidator(byte[] googlePublicKey, byte[] appleRootCert, string appBundleId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs an instance and checks the validity of the certification keys
        /// which only takes input parameters for the supported platforms.
        /// </summary>
        /// <param name="googlePublicKey"> The GooglePlay public key. </param>
        /// <param name="appleRootCert"> The Apple certification key. </param>
        /// <param name="googleBundleId"> The GooglePlay bundle ID. </param>
        /// <param name="appleBundleId"> The Apple bundle ID. </param>
        public CrossPlatformValidator(byte[] googlePublicKey, byte[] appleRootCert, string googleBundleId, string appleBundleId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validates a receipt.
        /// </summary>
        /// <param name="unityIAPReceipt"> The receipt to be validated. </param>
        /// <returns> An array of receipts parsed from the validation process </returns>
        public IPurchaseReceipt[] Validate(string unityIAPReceipt)
        {
            throw new NotImplementedException();
        }
    }
}
