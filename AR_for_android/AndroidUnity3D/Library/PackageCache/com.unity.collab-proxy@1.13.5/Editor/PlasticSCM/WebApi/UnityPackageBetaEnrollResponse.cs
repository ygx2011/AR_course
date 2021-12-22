using Unity.Plastic.Newtonsoft.Json;

using PlasticGui.WebApi.Responses;

namespace Unity.PlasticSCM.Editor.WebApi
{
    /// <summary>
    /// Response to Unity Package beta enrollment request.
    /// </summary>
    public class UnityPackageBetaEnrollResponse
    {
        /// <summary>
        /// Error caused by the request.
        /// </summary>
        [JsonProperty("error")]
        public ErrorResponse.ErrorFields Error { get; set; }

        /// <summary>
        /// Whether the beta is enabled.
        /// </summary>
        [JsonProperty("isBetaEnabled")]
        public bool IsBetaEnabled { get; set; }
    }
}
