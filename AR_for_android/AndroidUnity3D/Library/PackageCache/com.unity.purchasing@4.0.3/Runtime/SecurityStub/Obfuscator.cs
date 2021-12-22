using System;

namespace UnityEngine.Purchasing.Security
{
    /// <summary>
    /// Stub class to handle tangle files for platforms that do not support this feature.
    /// Will always throw an Exception if used.
    /// </summary>
    public static class Obfuscator
    {
        /// <summary>
        /// Deobfucscates tangle data.
        /// </summary>
        /// <param name="data"> The Apple or GooglePlay public key data to be deobfuscated. </param>
        /// <param name="order"> The array of the order of the data slices used to obfuscate the data when the tangle files were originally generated. </param>
        /// <param name="key"> The encryption key to deobfuscate the tangled data at runtime, previously generated with the tangle file. </param>
        /// <returns>The deobfucated public key</returns>
        public static byte [] DeObfuscate (byte [] data, int [] order, int key)
        {
            throw new NotImplementedException ();
        }
    }
}
