﻿﻿using System;
using System.Collections.ObjectModel;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using System.Collections.Generic;
  using System.Globalization;
  using System.Reflection;
  using System.Threading;

namespace UnityEngine.Purchasing
{
	static class SerializationExtensions {
		public static string TryGetString(this Dictionary<string, object> dic, string key)
		{
			if (dic.ContainsKey (key)) {
				if (dic [key] != null) {
					return dic [key].ToString ();
				}
			}
			return null;
		}
	}

    internal class JSONSerializer
    {
        public static string SerializeProductDef(ProductDefinition product)
        {
            return MiniJson.JsonEncode(EncodeProductDef(product));
        }

        public static string SerializeProductDefs(IEnumerable<ProductDefinition> products)
        {
            List<object> result = new List<object>();
            foreach (var product in products)
            {
                result.Add(EncodeProductDef(product));
            }
            return MiniJson.JsonEncode(result);
        }

        public static string SerializeProductDescs(ProductDescription product)
        {
            return MiniJson.JsonEncode(EncodeProductDesc(product));
        }

        public static string SerializeProductDescs(IEnumerable<ProductDescription> products)
        {
            List<object> result = new List<object>();
            foreach (var product in products)
            {
                result.Add(EncodeProductDesc(product));
            }
            return MiniJson.JsonEncode(result);
        }

        public static List<ProductDescription> DeserializeProductDescriptions(string json)
        {
            var objects = (List<object>) MiniJson.JsonDecode(json);
            var result = new List<ProductDescription>();
            foreach (Dictionary<string, object> obj in objects)
            {
                var metadata = DeserializeMetadata((Dictionary<string, object>) obj["metadata"]);
                var product = new ProductDescription(
                    (string) obj["storeSpecificId"],
                    metadata,
                    obj.TryGetString("receipt"),
                    obj.TryGetString("transactionId"),
                    ProductType.NonConsumable);
                result.Add(product);
            }
            return result;
        }

        public static Dictionary<string, string> DeserializeSubscriptionDescriptions(string json)
        {
            var objects = (List<object>) MiniJson.JsonDecode(json);
            var result = new Dictionary<string, string>();
            foreach (Dictionary<string, object> obj in objects)
            {
                var subscription = new Dictionary<string, string>();
                if (obj.TryGetValue("metadata", out var metadata))
                {
                    var metadataDict = (Dictionary<string, object>) metadata;
                    subscription["introductoryPrice"] = metadataDict.TryGetString("introductoryPrice");
                    subscription["introductoryPriceLocale"] = metadataDict.TryGetString("introductoryPriceLocale");
                    subscription["introductoryPriceNumberOfPeriods"] = metadataDict.TryGetString("introductoryPriceNumberOfPeriods");
                    subscription["numberOfUnits"] = metadataDict.TryGetString("numberOfUnits");
                    subscription["unit"] = metadataDict.TryGetString("unit");

                    // this is a double check for Apple side's bug
                    if (!string.IsNullOrEmpty(subscription["numberOfUnits"]) && string.IsNullOrEmpty(subscription["unit"])) {
                        subscription["unit"] = "0";
                    }
                }
                else
                {
                    Debug.LogWarning("metadata key not found in subscription description json");
                }

                if (obj.TryGetValue("storeSpecificId", out var id))
                {
                    var idStr = (string) id;
                    result.Add(idStr, MiniJson.JsonEncode(subscription));
                }
                else
                {
                    Debug.LogWarning("storeSpecificId key not found in subscription description json");
                }
            }

            return result;
        }

        public static Dictionary<string, string> DeserializeProductDetails(string json)
        {
            var objects = (List<object>) MiniJson.JsonDecode(json);
            var result = new Dictionary<string, string>();
            foreach (Dictionary<string, object> obj in objects)
            {

                var details = new Dictionary<string, string>();
                if (obj.TryGetValue("metadata", out var metadata))
                {
                    var metadataStr = (Dictionary<string, object>) metadata;
                    details["subscriptionNumberOfUnits"] = metadataStr.TryGetString("subscriptionNumberOfUnits");
                    details["subscriptionPeriodUnit"] = metadataStr.TryGetString("subscriptionPeriodUnit");
                    details["localizedPrice"] = metadataStr.TryGetString("localizedPrice");
                    details["isoCurrencyCode"] = metadataStr.TryGetString("isoCurrencyCode");
                    details["localizedPriceString"] = metadataStr.TryGetString("localizedPriceString");
                    details["localizedTitle"] = metadataStr.TryGetString("localizedTitle");
                    details["localizedDescription"] = metadataStr.TryGetString("localizedDescription");
                    details["introductoryPrice"] = metadataStr.TryGetString("introductoryPrice");
                    details["introductoryPriceLocale"] = metadataStr.TryGetString("introductoryPriceLocale");
                    details["introductoryPriceNumberOfPeriods"] = metadataStr.TryGetString("introductoryPriceNumberOfPeriods");
                    details["numberOfUnits"] = metadataStr.TryGetString("numberOfUnits");
                    details["unit"] = metadataStr.TryGetString("unit");

                    // this is a double check for Apple side's bug
                    if (!string.IsNullOrEmpty(details["subscriptionNumberOfUnits"]) && string.IsNullOrEmpty(details["subscriptionPeriodUnit"])) {
                        details["subscriptionPeriodUnit"] = "0";
                    }

                    // this is a double check for Apple side's bug
                    if (!string.IsNullOrEmpty(details["numberOfUnits"]) && string.IsNullOrEmpty(details["unit"])) {
                        details["unit"] = "0";
                    }
                }
                else
                {
                    Debug.LogWarning("metadata key not found in product details json");
                }

                if (obj.TryGetValue("storeSpecificId", out var id))
                {
                    var idStr = (string) id;
                    result.Add(idStr, MiniJson.JsonEncode(details));
                }
                else
                {
                    Debug.LogWarning("storeSpecificId key not found in product details json");
                }
            }
            return result;
        }


        public static PurchaseFailureDescription DeserializeFailureReason(string json)
        {
            var dic = (Dictionary<string, object>) MiniJson.JsonDecode(json);
            var reason = PurchaseFailureReason.Unknown;

            if (dic.TryGetValue("reason", out var reasonStr))
            {
                if (Enum.IsDefined(typeof(PurchaseFailureReason), (string) reasonStr))
                {
                    reason = (PurchaseFailureReason) Enum.Parse(typeof(PurchaseFailureReason), (string) reasonStr);
                }

                if (dic.TryGetValue("productId", out var productId))
                {
                    return new PurchaseFailureDescription( (string) productId, reason, dic.TryGetString("message"));
                }
            }
            else
            {
                Debug.LogWarning("Reason key not found in purchase failure json: " + json);
            }

            return new PurchaseFailureDescription( "Unknown ProductID", reason, dic.TryGetString("message"));
        }

        private static ProductMetadata DeserializeMetadata(Dictionary<string, object> data)
        {
            // We are seeing an occasional exception when converting a string to a decimal here. It may be related to
            // a mono bug with certain cultures' number formatters: https://bugzilla.xamarin.com/show_bug.cgi?id=4814
            //
            // It's not a great idea to set the price to 0 when this happens, but it's probably better than throwing
            // an exception. The best solution is to pass a number for localizedPrice when possible, to avoid any string
            // parsing issues.
            decimal localizedPrice = 0.0m;
			try {
                localizedPrice = Convert.ToDecimal(data["localizedPrice"]);
			} catch {
                localizedPrice = 0.0m;
            }

            return new ProductMetadata(
                data.TryGetString("localizedPriceString"),
                data.TryGetString("localizedTitle"),
                data.TryGetString("localizedDescription"),
                data.TryGetString("isoCurrencyCode"),
                localizedPrice);
        }

        private static Dictionary<string, object> EncodeProductDef(ProductDefinition product)
        {
            var prod = new Dictionary<string, object>
            {
                {"id", product.id}, {"storeSpecificId", product.storeSpecificId}, {"type", product.type.ToString()}
            };

            bool enabled = true;
            var enabledProp = typeof(ProductDefinition).GetProperty("enabled");
			if (enabledProp != null) {
				try {
                    enabled = Convert.ToBoolean(enabledProp.GetValue(product, null));
				} catch {
                    enabled = true;
                }
            }
            prod.Add("enabled", enabled);

            var payoutsArray = new List<object>();
            var payoutsProp = typeof(ProductDefinition).GetProperty("payouts");
			if (payoutsProp != null) {
                var payoutsObject = payoutsProp.GetValue(product, null);
                Array payouts = payoutsObject as Array;
				if (payouts != null) {
					foreach (object payout in payouts) {
                        var payoutDict = new Dictionary<string, object>();
                        var payoutType = payout.GetType();
                        payoutDict["t"] = payoutType.GetField("typeString").GetValue(payout);
                        payoutDict["st"] = payoutType.GetField("subtype").GetValue(payout);
                        payoutDict["q"] = payoutType.GetField("quantity").GetValue(payout);
                        payoutDict["d"] = payoutType.GetField("data").GetValue(payout);
                        payoutsArray.Add(payoutDict);
                    }
                }
            }
            prod.Add("payouts", payoutsArray);

            return prod;
        }

        private static Dictionary<string, object> EncodeProductDesc(ProductDescription product)
        {
            var prod = new Dictionary<string, object> {{"storeSpecificId", product.storeSpecificId}};

            // ProductDescription.type field available in Unity 5.4+. Access by reflection here.
            Type pdClassType = typeof(ProductDescription);
            FieldInfo pdClassFieldType = pdClassType.GetField("type");
            if (pdClassFieldType != null)
            {
                var typeValue = pdClassFieldType.GetValue(product);
                prod.Add("type", typeValue.ToString());
            }
            prod.Add("metadata", EncodeProductMeta(product.metadata));
            prod.Add("receipt", product.receipt);
            prod.Add("transactionId", product.transactionId);

            return prod;
        }

        private static Dictionary<string, object> EncodeProductMeta(ProductMetadata product)
        {
            var prod = new Dictionary<string, object>
            {
                {"localizedPriceString", product.localizedPriceString},
                {"localizedTitle", product.localizedTitle},
                {"localizedDescription", product.localizedDescription},
                {"isoCurrencyCode", product.isoCurrencyCode},
                {"localizedPrice", Convert.ToDouble(product.localizedPrice)}
            };


            return prod;
        }
    }
}
