using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.Purchasing
{
    internal class UDPReflectionUtils
    {
        internal const BindingFlags k_InstanceBindingFlags = BindingFlags.Public | BindingFlags.Instance;
        internal const BindingFlags k_StaticBindingFlags = BindingFlags.Public | BindingFlags.Static;
        internal const BindingFlags k_PrivateStaticBindingFlags = BindingFlags.NonPublic | BindingFlags.Static;

        static Dictionary<Assembly, Type[]> s_assemblyTypeCache = new Dictionary<Assembly, Type[]>();
        static Dictionary<string, Type> s_typeCache = new Dictionary<string, Type>();
        static readonly string[] k_whiteListedAssemblies = {"UnityEngine", "UnityEditor", "UDP", "com.unity"};

        internal static Type GetTypeByName(string typeName)
        {
            if (s_typeCache.ContainsKey(typeName))
                return s_typeCache[typeName];

            var assemblies = GetAllAssemblies();

            foreach (var assembly in assemblies)
            {
                if (Array.Exists(k_whiteListedAssemblies, x => assembly.FullName.StartsWith(x)))
                {
                    var types = GetTypes(assembly);
                    foreach (var type in types)
                    {
                        if (type.FullName == typeName)
                        {
                            s_typeCache[type.FullName] = type;
                            return type;
                        }
                    }
                }
            }
            return null;
        }

        static IEnumerable<Assembly> GetAllAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            if (!s_assemblyTypeCache.TryGetValue(assembly, out var types))
            {
                types = assembly.GetTypes();
                s_assemblyTypeCache[assembly] = types;
            }

            return types;
        }
    }

    internal class AppStoreSettingsInterface
    {
        static Type s_typeCache;
        internal static Type GetClassType()
        {
            if (s_typeCache == null)
            {
                s_typeCache = UDPReflectionUtils.GetTypeByName(UDPReflectionConsts.k_AppStoreSettingsType);
            }
            return s_typeCache;
        }

        internal static FieldInfo GetClientIDField()
        {
            return GetClassType().GetField(UDPReflectionConsts.k_AppStoreSettingsClientIDField, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        internal static FieldInfo GetAppSlugField()
        {
            return GetClassType().GetField(UDPReflectionConsts.k_AppStoreSettingsAppSlugField, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        internal static FieldInfo GetAssetPathField()
        {
            return GetClassType().GetField(UDPReflectionConsts.k_AppStoreSettingsAssetPathField, UDPReflectionUtils.k_StaticBindingFlags);
        }
    }

    internal class BuildConfigInterface
    {
        static Type s_typeCache;

        internal static Type GetClassType()
        {
            if (s_typeCache == null)
            {
                s_typeCache = UDPReflectionUtils.GetTypeByName(UDPReflectionConsts.k_BuildConfigType);
            }
            return s_typeCache;
        }

        static FieldInfo GetApiEndpointField()
        {
            return GetClassType().GetField(UDPReflectionConsts.k_BuildConfigApiEndpointField, UDPReflectionUtils.k_StaticBindingFlags);
        }

        internal static string GetApiEndpoint()
        {
            return (string)GetApiEndpointField().GetValue(null);
        }

        static FieldInfo GetIdEndpointField()
        {
            return GetClassType().GetField(UDPReflectionConsts.k_BuildConfigIdEndpointField, UDPReflectionUtils.k_StaticBindingFlags);
        }

        internal static string GetIdEndpoint()
        {
            return (string)GetIdEndpointField().GetValue(null);
        }

        static FieldInfo GetUdpEndpointField()
        {
            return GetClassType().GetField(UDPReflectionConsts.k_BuildConfigUdpEndpointField, UDPReflectionUtils.k_StaticBindingFlags);
        }

        internal static string GetUdpEndpoint()
        {
            return (string)GetUdpEndpointField().GetValue(null);
        }

        static FieldInfo GetVersionField()
        {
            return GetClassType().GetField(UDPReflectionConsts.k_BuildConfigVersionField, UDPReflectionUtils.k_StaticBindingFlags);
        }

        internal static string GetVersion()
        {
            return (string)GetVersionField().GetValue(null);
        }
    }

    internal class InventoryInterface
    {
        static Type s_typeCache;

        internal static Type GetClassType()
        {
            if (s_typeCache == null)
            {
                s_typeCache = UDPReflectionUtils.GetTypeByName(UDPReflectionConsts.k_InventoryType);
            }
            return s_typeCache;
        }

        internal static MethodInfo GetProductListMethod()
        {
            return GetClassType().GetMethod(UDPReflectionConsts.k_InventoryGetProductListMethod, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        internal static MethodInfo GetPurchaseInfoMethod()
        {
            return GetClassType().GetMethod(UDPReflectionConsts.k_InventoryGetPurchaseInfoMethod, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        internal static MethodInfo HasPurchaseMethod()
        {
            return GetClassType().GetMethod(UDPReflectionConsts.k_InventoryHasPurchaseMethod, UDPReflectionUtils.k_InstanceBindingFlags);
        }
    }

    internal class ProductInfoInterface
    {
        static Type s_typeCache;

        static Type GetClassType()
        {
            if (s_typeCache == null)
            {
                s_typeCache = UDPReflectionUtils.GetTypeByName(UDPReflectionConsts.k_ProductInfoType);
            }
            return s_typeCache;
        }

        public static PropertyInfo GetCurrencyProp()
        {
            return GetClassType().GetProperty(UDPReflectionConsts.k_ProductInfoCurrencyProp, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        public static PropertyInfo GetDescriptionProp()
        {
            return GetClassType().GetProperty(UDPReflectionConsts.k_ProductInfoDescProp, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        public static PropertyInfo GetPriceProp()
        {
            return GetClassType().GetProperty(UDPReflectionConsts.k_ProductInfoPriceProp, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        public static PropertyInfo GetPriceAmountMicrosProp()
        {
            return GetClassType().GetProperty(UDPReflectionConsts.k_ProductnfoPriceAmountMicrosProp, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        public static PropertyInfo GetProductIdProp()
        {
            return GetClassType().GetProperty(UDPReflectionConsts.k_ProductInfoIdProp, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        public static PropertyInfo GetTitleProp()
        {
            return GetClassType().GetProperty(UDPReflectionConsts.k_ProductInfoTitleProp, UDPReflectionUtils.k_InstanceBindingFlags);
        }
    }

    internal class StoreServiceInterface
    {
        static Type s_typeCache;
        internal static Type GetClassType()
        {
            if (s_typeCache == null)
            {
                s_typeCache = UDPReflectionUtils.GetTypeByName(UDPReflectionConsts.k_StoreServiceType);
            }
            return s_typeCache;
        }

        static PropertyInfo GetNameProp()
        {
            return GetClassType()?.GetProperty(UDPReflectionConsts.k_StoreServiceNameProp, UDPReflectionUtils.k_StaticBindingFlags);
        }

        internal static string GetName()
        {
            return (string)GetNameProp()?.GetValue(null, null);
        }

        internal static MethodInfo GetEnableDebugLoggingMethod()
        {
            return GetClassType().GetMethod(UDPReflectionConsts.k_StoreServiceEnableDebugLoggingMethod, UDPReflectionUtils.k_StaticBindingFlags);
        }
    }

    internal class UdpIapBridgeInterface
    {
        static Type s_typeCache;
        internal static Type GetClassType()
        {
            if (s_typeCache == null)
            {
                s_typeCache = UDPReflectionUtils.GetTypeByName(UDPReflectionConsts.k_UdpIapBridgeType);
            }
            return s_typeCache;
        }

        internal static MethodInfo GetInitMethod()
        {
            return GetClassType().GetMethod(UDPReflectionConsts.k_UdpIapBridgeInitMethod, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        internal static MethodInfo GetPurchaseMethod()
        {
            return GetClassType().GetMethod(UDPReflectionConsts.k_UdpIapBridgePurchaseMethod, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        internal static MethodInfo GetRetrieveProductsMethod()
        {
            return GetClassType().GetMethod(UDPReflectionConsts.k_UdpIapBridgeRetrieveProductsMethod, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        internal static MethodInfo GetFinishTransactionMethod()
        {
            return GetClassType().GetMethod(UDPReflectionConsts.k_UdpIapBridgeFinishTransactionMethod, UDPReflectionUtils.k_InstanceBindingFlags);
        }
    }

    internal class UserInfoInterface
    {
        static Type s_typeCache;
        internal static Type GetClassType()
        {
            if (s_typeCache == null)
            {
                s_typeCache = UDPReflectionUtils.GetTypeByName(UDPReflectionConsts.k_UserInfoType);
            }
            return s_typeCache;
        }

        internal static PropertyInfo GetChannelProp()
        {
            return GetClassType().GetProperty(UDPReflectionConsts.k_UserInfoChannelProp, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        internal static PropertyInfo GetIdProp()
        {
            return GetClassType().GetProperty(UDPReflectionConsts.k_UserInfoIdProp, UDPReflectionUtils.k_InstanceBindingFlags);
        }

        internal static PropertyInfo GetLoginTokenProp()
        {
            return GetClassType().GetProperty(UDPReflectionConsts.k_UserInfoLoginTokenProp, UDPReflectionUtils.k_InstanceBindingFlags);
        }
    }
}
