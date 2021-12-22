namespace UnityEditor.Purchasing
{
    static class IapMenuConsts
    {
        #if ENABLE_EDITOR_GAME_SERVICES
        internal const string PurchasingDisplayName = "In-App Purchasing";
        internal const string MenuItemRoot = "Services/" + PurchasingDisplayName;
        #else
        internal const string PurchasingDisplayName = "Unity IAP";
        internal const string MenuItemRoot = "Window/" + PurchasingDisplayName;
        #endif
        internal const string SwitchStoreTitleText = "Store Selector";
    }
}
