namespace UnityEngine.Purchasing
{
    /// <summary>
    /// The various reasons a purchase can fail. These codes are store-specific, so that developers can have access to
    /// to a wider range of debugging information.
    /// </summary>
    public enum StoreSpecificPurchaseErrorCode
    {
        /// <summary>
        /// Error code indicating that an unknown or unexpected error occurred. (Apple only)
        /// </summary>
        SKErrorUnknown,

        /// <summary>
        /// Error code indicating that the client is not allowed to perform the attempted action. (Apple only)
        /// </summary>
        SKErrorClientInvalid,

        /// <summary>
        /// Error code indicating that the user cancelled a payment request. (Apple only)
        /// </summary>
        SKErrorPaymentCancelled,

        /// <summary>
        /// Error code indicating that one of the payment parameters was not recognized by the App Store. (Apple only)
        /// </summary>
        SKErrorPaymentInvalid,

        /// <summary>
        /// Error code indicating that the user is not allowed to authorize payments. (Apple only)
        /// </summary>
        SKErrorPaymentNotAllowed,

        /// <summary>
        /// Error code indicating that the requested product is not available in the store. (Apple only)
        /// </summary>
        SKErrorStoreProductNotAvailable,


        /// <summary>
        /// Error code indicating that the user has not allowed access to Cloud service information. (Apple only)
        /// </summary>
        SKErrorCloudServicePermissionDenied,

        /// <summary>
        /// Error code indicating that the device could not connect to the network. (Apple only)
        /// </summary>
        SKErrorCloudServiceNetworkConnectionFailed,

        /// <summary>
        /// Error code indicating that the user has revoked permission to use this cloud service. (Apple only)
        /// </summary>
        SKErrorCloudServiceRevoked,

        /// <summary>
        /// Success. Note, this is technically not an error code, but is included for completeness. (Google Play only)
        /// </summary>
        BILLING_RESPONSE_RESULT_OK,

        /// <summary>
        /// User pressed back or canceled a dialog. (Google Play only)
        /// </summary>
        BILLING_RESPONSE_RESULT_USER_CANCELED,

        /// <summary>
        /// Network connection is down. (Google Play only)
        /// </summary>
        BILLING_RESPONSE_RESULT_SERVICE_UNAVAILABLE,

        /// <summary>
        /// Billing API version is not supported for the type requested. (Google Play only)
        /// </summary>
        BILLING_RESPONSE_RESULT_BILLING_UNAVAILABLE,

        /// <summary>
        /// Requested product is not available for purchase. (Google Play only)
        /// </summary>
        BILLING_RESPONSE_RESULT_ITEM_UNAVAILABLE,

        /// <summary>
        /// Invalid arguments provided to the API. This error can also indicate that the application was not correctly signed or properly set up for In-app Billing in Google Play, or does not have the necessary permissions in its manifest (Google Play only)
        /// </summary>
        BILLING_RESPONSE_RESULT_DEVELOPER_ERROR,

        /// <summary>
        /// Fatal error during the API action. (Google Play only)
        /// </summary>
        BILLING_RESPONSE_RESULT_ERROR,

        /// <summary>
        /// Failure to purchase since item is already owned. (Google Play only)
        /// </summary>
        BILLING_RESPONSE_RESULT_ITEM_ALREADY_OWNED,

        /// <summary>
        /// Failure to consume since item is not owned. (Google Play only)
        /// </summary>
        BILLING_RESPONSE_RESULT_ITEM_NOT_OWNED,

        // IAB HELPER ERROR CODES
        /// <summary>
        /// In-App Billing helper base error. (GooglePlay only)
        /// </summary>
        IABHELPER_ERROR_BASE,

        /// <summary>
        /// In-App Billing helper encountered a remote exception. (GooglePlay only)
        /// </summary>
        IABHELPER_REMOTE_EXCEPTION,

        /// <summary>
        /// In-App Billing helper received a bad response. (GooglePlay only)
        /// </summary>
        IABHELPER_BAD_RESPONSE,

        /// <summary>
        /// In-App Billing helper had a failed verification. (GooglePlay only)
        /// </summary>
        IABHELPER_VERIFICATION_FAILED,

        /// <summary>
        /// In-App Billing helper found a failed intent. (GooglePlay only)
        /// </summary>
        IABHELPER_SEND_INTENT_FAILED,

        /// <summary>
        /// In-App Billing helper encountered a user cancellation. (GooglePlay only)
        /// </summary>
        IABHELPER_USER_CANCELLED,

        /// <summary>
        /// In-App Billing helper receieved an unknown purchase response. (GooglePlay only)
        /// </summary>
        IABHELPER_UNKNOWN_PURCHASE_RESPONSE,

        /// <summary>
        /// In-App Billing helper is missing a token. (GooglePlay only)
        /// </summary>
        IABHELPER_MISSING_TOKEN,

        /// <summary>
        /// In-App Billing helper encountered an unknown error. (GooglePlay only)
        /// </summary>
        IABHELPER_UNKNOWN_ERROR,

        /// <summary>
        /// In-App Billing helper has no subscriptions available. (GooglePlay only)
        /// </summary>
        IABHELPER_SUBSCRIPTIONS_NOT_AVAILABLE,

        /// <summary>
        /// In-App Billing helper attempted to consume an invalid purchase. (GooglePlay only)
        /// </summary>
        IABHELPER_INVALID_CONSUMPTION,

        /// <summary>
        /// Indicates that the customer already owns the provided SKU. (Amazon only)
        /// </summary>
        Amazon_ALREADY_PURCHASED,

        /// <summary>
        /// Indicates that the purchase failed. (Amazon only)
        /// </summary>
        Amazon_FAILED,

        /// <summary>
        /// Indicates that the SKU originally provided to the PurchasingService.purchase(String) method is not valid. (Amazon only)
        /// </summary>
        Amazon_INVALID_SKU,

        /// <summary>
        /// Indicates this call is not supported. (Amazon only)
        /// </summary>
        Amazon_NOT_SUPPORTED,

        /// <summary>
        /// A catch-all for remaining purchase problems.
        /// </summary>
        Unknown
    }
}
