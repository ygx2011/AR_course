namespace UnityEngine.Purchasing.Models
{
    static class GoogleBillingStrings
    {
        internal const string errorPurchaseStateUnspecified = "PurchaseState is UNSPECIFIED_STATE, no more details exists";
        internal const string errorUserCancelled = "User cancelled purchase";
        internal const string errorItemAlreadyOwned = "Item already owned";

        internal static string getWarningMessageMoreThanOneSkuFound(string sku)
        {
            return "More than one SKU found when purchasing SKU " + sku + ". Please verify your Google Play Store in-app product settings.";
        }
    }
}
