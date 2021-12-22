using System.Collections.Generic;

namespace UnityEngine.Purchasing.Utils
{
    static class GoogleReceiptEncoder
    {
        internal static string EncodeReceipt(string transactionId, string purchaseOriginalJson, string purchaseSignature, string skuDetailsJson)
        {
            return FormatPayload(purchaseOriginalJson, purchaseSignature, skuDetailsJson);
        }

        static string FormatPayload(string json, string signature, string skuDetails) {
            var dic = new Dictionary<string, object>
            {
                ["json"] = json,
                ["signature"] = signature,
                ["skuDetails"] = skuDetails
            };
            return MiniJson.JsonEncode(dic);
        }
    }
}
