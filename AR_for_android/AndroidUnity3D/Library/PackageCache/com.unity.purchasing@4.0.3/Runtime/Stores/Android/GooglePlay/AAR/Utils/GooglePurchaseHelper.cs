using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing.Utils
{
    static class GooglePurchaseHelper
    {
        internal static GooglePurchase MakeGooglePurchase(IEnumerable<AndroidJavaObject> skuDetails, AndroidJavaObject purchase)
        {
            var sku = purchase.Call<string>("getSku");
            var skuDetail = skuDetails.FirstOrDefault(skuDetailJavaObject =>
            {
                var skuDetailsSku = skuDetailJavaObject.Call<string>("getSku");
                return sku == skuDetailsSku;
            });
            return skuDetail != null ? new GooglePurchase(purchase, skuDetail) : null;
        }
    }
}
