namespace UnityEngine.Purchasing
{
    internal struct UDPReflectionConsts
    {
        //Assemblies
        const string k_UdpEngineNamespace = "UnityEngine.UDP";

        //Types;
        internal const string k_AppStoreSettingsType = k_UdpEngineNamespace + ".AppStoreSettings";
        internal const string k_BuildConfigType = k_UdpEngineNamespace + ".BuildConfig";
        internal const string k_InventoryType = k_UdpEngineNamespace + ".Inventory";
        internal const string k_ProductInfoType = k_UdpEngineNamespace + ".ProductInfo";
        internal const string k_StoreServiceType = k_UdpEngineNamespace + ".StoreService";
        internal const string k_UdpIapBridgeType = k_UdpEngineNamespace + ".UdpIapBridge";
        internal const string k_UserInfoType = k_UdpEngineNamespace + ".UserInfo";

        //AppStoreSettings Fields
        internal const string k_AppStoreSettingsClientIDField = "UnityClientID";
        internal const string k_AppStoreSettingsAppSlugField = "AppSlug";
        internal const string k_AppStoreSettingsAssetPathField = "appStoreSettingsAssetPath";

        //BuildConfig Fields
        internal const string k_BuildConfigApiEndpointField = "API_ENDPOINT";
        internal const string k_BuildConfigIdEndpointField = "ID_ENDPOINT";
        internal const string k_BuildConfigUdpEndpointField = "UDP_ENDPOINT";
        internal const string k_BuildConfigVersionField = "VERSION";

        //Inventory Methods
        internal const string k_InventoryGetProductListMethod = "GetProductList";
        internal const string k_InventoryGetPurchaseInfoMethod = "GetPurchaseInfo";
        internal const string k_InventoryHasPurchaseMethod = "HasPurchase";

        //ProductInfo Properties
        internal const string k_ProductInfoCurrencyProp = "Currency";
        internal const string k_ProductInfoDescProp = "Description";
        internal const string k_ProductInfoPriceProp = "Price";
        internal const string k_ProductnfoPriceAmountMicrosProp = "PriceAmountMicros";
        internal const string k_ProductInfoIdProp = "ProductId";
        internal const string k_ProductInfoTitleProp = "Title";

        //StoreService
        internal const string k_StoreServiceNameProp = "StoreName";
        internal const string k_StoreServiceEnableDebugLoggingMethod = "EnableDebugLogging";

        //UdpIapBridge Methods
        internal const string k_UdpIapBridgeInitMethod = "Initialize";
        internal const string k_UdpIapBridgePurchaseMethod = "Purchase";
        internal const string k_UdpIapBridgeRetrieveProductsMethod = "RetrieveProducts";
        internal const string k_UdpIapBridgeFinishTransactionMethod = "FinishTransaction";

        //UserInfo Properties
        internal const string k_UserInfoChannelProp = "Channel";
        internal const string k_UserInfoIdProp = "UserId";
        internal const string k_UserInfoLoginTokenProp = "UserLoginToken";
    }
}
