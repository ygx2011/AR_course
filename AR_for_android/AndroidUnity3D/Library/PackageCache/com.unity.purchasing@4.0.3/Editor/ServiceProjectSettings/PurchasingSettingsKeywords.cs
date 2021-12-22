using System.Collections.Generic;

namespace UnityEditor.Purchasing
{
    internal static class PurchasingSettingsKeywords
    {
        const string k_KeywordPurchasing = "purchasing";
        const string k_KeywordInApp = "in-app";
        const string k_KeywordPurchase = "purchase";
        const string k_KeywordRevenue = "revenue";
        const string k_KeywordPlatforms = "platforms";
        const string k_KeywordGooglePlay = "Google Play";
        const string k_KeywordPublicKey = "public key";
        const string k_KeywordReceipt = "receipt";
        const string k_KeywordObfuscator = "obfuscator";
        const string k_KeywordCatalog = "catalog";

        internal static List<string> GetKeywords()
        {
            return new List<string>()
            {
                k_KeywordPurchasing,
                k_KeywordInApp,
                k_KeywordPurchase,
                k_KeywordRevenue,
                k_KeywordPlatforms,
                k_KeywordGooglePlay,
                k_KeywordPublicKey,
                k_KeywordReceipt,
                k_KeywordObfuscator,
                k_KeywordCatalog
            };
        }
    }
}
