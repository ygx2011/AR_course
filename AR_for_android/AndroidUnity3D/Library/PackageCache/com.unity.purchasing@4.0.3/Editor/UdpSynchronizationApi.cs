using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Purchasing;

namespace UnityEditor.Purchasing
{
    /// <summary>
    /// Synchronize store data from UDP and IAP
    /// </summary>
    public static class UdpSynchronizationApi
    {

        internal const string kOAuthClientId = "channel_editor";

        // Although a client secret is here, it doesn't matter
        // because the user information is also secured by user's token
        private const string kOAuthClientSecret = "B63AFB324DE3D12A13827340019D1EE3";

        private const string kHttpVerbGET = "GET";
        private const string kHttpVerbPOST = "POST";
        private const string kHttpVerbPUT = "PUT";

        private const string kContentType = "Content-Type";
        private const string kApplicationJson = "application/json";
        private const string kAuthHeader = "Authorization";

        private static string kUnityWebRequestTypeString = "UnityEngine.Networking.UnityWebRequest";
        private static string kUploadHandlerRawTypeString = "UnityEngine.Networking.UploadHandlerRaw";
        private static string kDownloadHandlerBufferTypeString = "UnityEngine.Networking.DownloadHandlerBuffer";
        private const string kUnityOAuthNamespace = "UnityEditor.Connect.UnityOAuth";

        private static void CheckUdpBuildConfig()
        {
            Type udpBuildConfig = BuildConfigInterface.GetClassType();
            if (udpBuildConfig == null)
            {
                Debug.LogError("Cannot Retrieve Build Config Endpoints for UDP. Please make sure the UDP package is installed");
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Get Access Token according to authCode.
        /// </summary>
        /// <param name="authCode"> Acquired by UnityOAuth</param>
        /// <returns></returns>
        public static object GetAccessToken(string authCode)
        {
            CheckUdpBuildConfig();

            TokenRequest req = new TokenRequest();
            req.code = authCode;
            req.client_id = kOAuthClientId;
            req.client_secret = kOAuthClientSecret;
            req.grant_type = "authorization_code";
            req.redirect_uri = BuildConfigInterface.GetIdEndpoint();
            return asyncRequest(kHttpVerbPOST, BuildConfigInterface.GetApiEndpoint(), "/v1/oauth2/token", null, req);
        }

        /// <summary>
        /// Call UDP store asynchronously to retrieve the Organization Identifier.
        /// </summary>
        /// <param name="accessToken">The bearer token to UDP.</param>
        /// <param name="projectGuid">The project id.</param>
        /// <returns>The HTTP GET Request to get the organization identifier.</returns>
        public static object GetOrgId(string accessToken, string projectGuid)
        {
            CheckUdpBuildConfig();

            string api = "/v1/core/api/projects/" + projectGuid;
            return asyncRequest(kHttpVerbGET, BuildConfigInterface.GetApiEndpoint(), api, accessToken, null);
        }

        /// <summary>
        /// Call UDP store asynchronously to create a store item.
        /// </summary>
        /// <param name="accessToken">The bearer token to UDP.</param>
        /// <param name="orgId">The organization identifier to create the store item under.</param>
        /// <param name="iapItem">The store item to create.</param>
        /// <returns>The HTTP POST Request to create a store item.</returns>
        public static object CreateStoreItem(string accessToken, string orgId, IapItem iapItem)
        {
            CheckUdpBuildConfig();

            string api = "/v1/store/items";
            iapItem.ownerId = orgId;
            return asyncRequest(kHttpVerbPOST, BuildConfigInterface.GetUdpEndpoint(), api, accessToken, iapItem);
        }

        /// <summary>
        /// Call UDP store asynchronously to update a store item.
        /// </summary>
        /// <param name="accessToken">The bearer token to UDP.</param>
        /// <param name="iapItem">The updated store item.</param>
        /// <returns>The HTTP PUT Request to update a store item.</returns>
        public static object UpdateStoreItem(string accessToken, IapItem iapItem)
        {
            CheckUdpBuildConfig();

            string api = "/v1/store/items/" + iapItem.id;
            return asyncRequest(kHttpVerbPUT, BuildConfigInterface.GetUdpEndpoint(), api, accessToken, iapItem);
        }

        /// <summary>
        /// Call UDP store asynchronously to search for a store item.
        /// </summary>
        /// <param name="accessToken">The bearer token to UDP.</param>
        /// <param name="orgId">The organization identifier where to find the store item.</param>
        /// <param name="appItemSlug">The store item slug name.</param>
        /// <returns>The HTTP GET Request to update a store item.</returns>
        public static object SearchStoreItem(string accessToken, string orgId, string appItemSlug)
        {
            CheckUdpBuildConfig();

            string api = "/v1/store/items/search?ownerId=" + orgId +
                         "&ownerType=ORGANIZATION&start=0&count=20&type=IAP&masterItemSlug=" + appItemSlug;
            return asyncRequest(kHttpVerbGET, BuildConfigInterface.GetUdpEndpoint(), api, accessToken, null);
        }

        // Return UnityWebRequest instance
        private static object asyncRequest(string method, string url, string api, string token,
            object postObject)
        {
            Type unityWebRequestType = UnityWebRequestType();
            object request = Activator.CreateInstance(unityWebRequestType, url + api, method);

            if (postObject != null)
            {
                string postData = HandlePostData(JsonUtility.ToJson(postObject));
                byte[] postDataBytes = Encoding.UTF8.GetBytes(postData);

                // Set UploadHandler
                // Equivalent : request.uploadHandler = (UploadHandler) new UploadHandlerRaw(postDataBytes);
                Type uploadHanlderRawType = UDPReflectionUtils.GetTypeByName(kUploadHandlerRawTypeString);

                var uploadHandlerRaw = Activator.CreateInstance(uploadHanlderRawType, postDataBytes);
                PropertyInfo uploadHandlerInfo =
                    unityWebRequestType.GetProperty("uploadHandler", UDPReflectionUtils.k_InstanceBindingFlags);
                uploadHandlerInfo.SetValue(request, uploadHandlerRaw, null);
            }

            // Set up downloadHandler
            // Equivalent: request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            var downloadHandlerInstance = Activator.CreateInstance(UDPReflectionUtils.GetTypeByName(kDownloadHandlerBufferTypeString));
            var downloadHandlerProperty =
                unityWebRequestType.GetProperty("downloadHandler", UDPReflectionUtils.k_InstanceBindingFlags);
            downloadHandlerProperty.SetValue(request, downloadHandlerInstance, null);


            // Set up header
            // Equivalent : request.SetRequestHeader("key", "value");
            MethodInfo setRequestHeaderMethodInfo =
                unityWebRequestType.GetMethod("SetRequestHeader", UDPReflectionUtils.k_InstanceBindingFlags);

            setRequestHeaderMethodInfo.Invoke(request, new object[] {kContentType, kApplicationJson});
            if (token != null)
            {
                setRequestHeaderMethodInfo.Invoke(request, new object[] {kAuthHeader, "Bearer " + token});
            }

            // Send Web Request
            // Equivalent: request.SendWebRequest()/request.Send()
            MethodInfo sendWebRequest = unityWebRequestType.GetMethod("SendWebRequest");
            if (sendWebRequest == null)
            {
                sendWebRequest = unityWebRequestType.GetMethod("Send");
            }

            sendWebRequest.Invoke(request, null);

            return request;
        }

        // Try to find UnityOAuth in assembly, if not found, udp will not be available.
        // Also, the version must larger or equal to 5.6.1
        internal static bool CheckUdpAvailability()
        {
            bool hasOAuth = GetUnityOAuthType() != null;
            return hasOAuth;
        }

        internal static bool CheckUdpCompatibility()
        {
            Type udpBuildConfig = BuildConfigInterface.GetClassType();
            if (udpBuildConfig == null)
            {
                Debug.LogError("Cannot Retrieve Build Config Endpoints for UDP. Please make sure the UDP package is installed");
                return false;
            }

			var udpVersion = BuildConfigInterface.GetVersion();
			int majorVersion = 0;
			int.TryParse(udpVersion.Split('.')[0], out majorVersion);

			return majorVersion >= 2;
		}

        // A very tricky way to deal with the json string, need to be improved
        // en-US and zh-CN will appear in the JSON and Unity JsonUtility cannot
        // recognize them to variables. So we change this to a string (remove "-").
        private static string HandlePostData(string oldData)
        {
            string newData = oldData.Replace("thisShouldBeENHyphenUS", "en-US");
            newData = newData.Replace("thisShouldBeZHHyphenCN", "zh-CN");
            Regex re = new Regex("\"\\w+?\":\"\",");
            newData = re.Replace(newData, "");
            re = new Regex(",\"\\w+?\":\"\"");
            newData = re.Replace(newData, "");
            re = new Regex("\"\\w+?\":\"\"");
            newData = re.Replace(newData, "");
            return newData;
        }

        #region Reflection Utils

        // Using UnityOAuth through reflection to avoid error on Unity lower than 5.6.1.
        internal static Type GetUnityOAuthType()
        {
            return UDPReflectionUtils.GetTypeByName(kUnityOAuthNamespace);
        }

        internal static Type UnityWebRequestType()
        {
            return UDPReflectionUtils.GetTypeByName(kUnityWebRequestTypeString);
        }

        // get UnityWebRequest.isDone property
        internal static bool IsUnityWebRequestDone(object request)
        {
            var isDoneProperty =
                UnityWebRequestType().GetProperty("isDone", UDPReflectionUtils.k_InstanceBindingFlags);

            return (bool) isDoneProperty.GetValue(request, null);
        }

        // Get UnityWebRequest.error property
        internal static string UnityWebRequestError(object request)
        {
            var errorProperty = UnityWebRequestType().GetProperty("error", UDPReflectionUtils.k_InstanceBindingFlags);

            return (string) errorProperty.GetValue(request, null);
        }

        // UnityWebRequest.responseCode
        internal static long UnityWebRequestResponseCode(object request)
        {
            var responseProperty = UnityWebRequestType()
                .GetProperty("responseCode", UDPReflectionUtils.k_InstanceBindingFlags);
            return (long) responseProperty.GetValue(request, null);
        }

        // UnityWebRequest.DownloadHandler.text
        internal static string UnityWebRequestResultString(object request)
        {
            var downloadHandlerProperty =
                UnityWebRequestType().GetProperty("downloadHandler", UDPReflectionUtils.k_InstanceBindingFlags);

            object downloadHandler = downloadHandlerProperty.GetValue(request, null);

            var textProperty = UDPReflectionUtils.GetTypeByName(kDownloadHandlerBufferTypeString)
                .GetProperty("text", UDPReflectionUtils.k_InstanceBindingFlags);

            return (string) textProperty.GetValue(downloadHandler, null);
        }

        #endregion
    }

    #region model

    /// <summary>
    /// This class is used to authenticate the API call to UDP. In OAuth2.0 authentication format.
    /// </summary>
    [Serializable]
    public class TokenRequest
    {
        /// <summary>
        /// The access token. Acquired by UnityOAuth
        /// </summary>
        public string code;
        /// <summary>
        /// The client identifier
        /// </summary>
        public string client_id;
        /// <summary>
        /// The client secret key
        /// </summary>
        public string client_secret;
        /// <summary>
        /// The type of OAuth2.0 code granting.
        /// </summary>
        public string grant_type;
        /// <summary>
        /// Redirect use after a successful authorization.
        /// </summary>
        public string redirect_uri;
        /// <summary>
        /// When the access token is expire. This token is used to renew it.
        /// </summary>
        public string refresh_token;
    }

    /// <summary>
    /// PriceSets holds the PurchaseFee. Used for IapItem.
    /// </summary>
    [Serializable]
    public class PriceSets
    {
        /// <summary>
        /// Get the PurchaseFee
        /// </summary>
        public PurchaseFee PurchaseFee = new PurchaseFee();
    }

    /// <summary>
    /// A PurchaseFee contains the PriceMap which contains the prices and currencies.
    /// </summary>
    [Serializable]
    public class PurchaseFee
    {
        /// <summary>
        /// The PurchaseFee type
        /// </summary>
        public string priceType = "CUSTOM";
        /// <summary>
        /// Holds a list of prices with their currencies
        /// </summary>
        public PriceMap priceMap = new PriceMap();
    }

    /// <summary>
    /// PriceMap hold a list of PriceDetail.
    /// </summary>
    [Serializable]
    public class PriceMap
    {
        /// <summary>
        /// List of prices with their currencies.
        /// </summary>
        public List<PriceDetail> DEFAULT = new List<PriceDetail>();
    }

    /// <summary>
    /// Price and the currency of a IAPItem.
    /// </summary>
    [Serializable]
    public class PriceDetail
    {
        /// <summary>
        /// Price of a IAPItem.
        /// </summary>
        public string price;
        /// <summary>
        /// Currency of the price.
        /// </summary>
        public string currency = "USD";
    }

    /// <summary>
    /// The Response from and HTTP response converted into an object.
    /// </summary>
    [Serializable]
    public class GeneralResponse
    {
        /// <summary>
        /// The body from the HTTP response.
        /// </summary>
        public string message;
    }

    /// <summary>
    /// The properties of a IAPItem.
    /// </summary>
    [Serializable]
    public class Properties
    {
        /// <summary>
        /// The description of a IAPItem.
        /// </summary>
        public string description;
    }

    /// <summary>
    /// The response used when creating/updating IAP item succeeds
    /// </summary>
    [Serializable]
    public class IapItemResponse : GeneralResponse
    {
        /// <summary>
        /// The IapItem identifier.
        /// </summary>
        public string id;
    }

    /// <summary>
    /// IapItem is the representation of a purchasable product from the UDP store
    /// </summary>
    [Serializable]
    public class IapItem
    {
        /// <summary>
        /// A unique identifier to identify the product.
        /// </summary>
        public string id;
        /// <summary>
        /// The product url stripped of all unsafe characters.
        /// </summary>
        public string slug;
        /// <summary>
        /// The product name.
        /// </summary>
        public string name;
        /// <summary>
        /// The organization url stripped of all unsafe characters.
        /// </summary>
        public string masterItemSlug;
        /// <summary>
        /// Is product a consumable type. If set to false it is a subscriptions.
        /// Consumables may be purchased more than once.
        /// Subscriptions have a finite window of validity.
        /// </summary>
        public bool consumable = true;
        /// <summary>
        /// The product type.
        /// </summary>
        public string type = "IAP";
        /// <summary>
        /// The product status.
        /// </summary>
        public string status = "STAGE";
        /// <summary>
        /// The organization id.
        /// </summary>
        public string ownerId;
        /// <summary>
        /// The organization type.
        /// </summary>
        public string ownerType = "ORGANIZATION";

        /// <summary>
        /// The product's prices with currencies.
        /// </summary>
        public PriceSets priceSets = new PriceSets();

        /// <summary>
        /// The properties of the product.
        /// </summary>
        public Properties properties = new Properties();

        /// <summary>
        /// Validates that the IapItem has at least the minimum amount of information set.
        /// </summary>
        /// <returns>A string error of missing information to the IapItem.</returns>
        public string ValidationCheck()
        {
            if (string.IsNullOrEmpty(slug))
            {
                return "Please fill in the ID";
            }

            if (string.IsNullOrEmpty(name))
            {
                return "Please fill in the title";
            }

            if (properties == null || string.IsNullOrEmpty(properties.description))
            {
                return "Please fill in the description";
            }

            return "";
        }
    }

    /// <summary>
    /// TokenInfo holds all the authentication token required to authenticate the API call.
    /// </summary>
    [Serializable]
    public class TokenInfo : GeneralResponse
    {
        /// <summary>
        /// The OAuth2.0 access token.
        /// </summary>
        public string access_token;
        /// <summary>
        /// The OAuth2.0 refresh token.
        /// </summary>
        public string refresh_token;
    }

    /// <summary>
    /// The response used when searching for IAP item.
    /// </summary>
    [Serializable]
    public class IapItemSearchResponse : GeneralResponse
    {
        /// <summary>
        /// The total amount of IAP item found.
        /// </summary>
        public int total;
        /// <summary>
        /// The list of IAP item found.
        /// </summary>
        public List<IapItem> results;
    }

    struct ReqStruct
    {
        public object request; // UnityWebRequest object
        public GeneralResponse resp;
        public ProductCatalogEditor.ProductCatalogItemEditor itemEditor;
        public IapItem iapItem;
    }

    /// <summary>
    /// The response used when searching for Organization identifier.
    /// </summary>
    [Serializable]
    public class OrgIdResponse : GeneralResponse
    {
        /// <summary>
        /// The organization identifier.
        /// </summary>
        public string org_foreign_key;
    }

    /// <summary>
    /// The response used when searching for Organization roles.
    /// </summary>
    [Serializable]
    public class OrgRoleResponse : GeneralResponse
    {
        /// <summary>
        /// The organization roles.
        /// </summary>
        public List<string> roles;
    }

    /// <summary>
    /// The response used when getting an error.
    /// </summary>
    [Serializable]
    public class ErrorResponse : GeneralResponse
    {
        /// <summary>
        /// The http error code.
        /// </summary>
        public string code;
        /// <summary>
        /// The details of an error.
        /// </summary>
        public ErrorDetail[] details;
    }

    /// <summary>
    /// The details of an error return from the api.
    /// </summary>
    [Serializable]
    public class ErrorDetail
    {
        /// <summary>
        /// The error context where it occured.
        /// </summary>
        public string field;
        /// <summary>
        /// The error message reason.
        /// </summary>
        public string reason;
    }

    #endregion
}
