using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#oauthFlowsObject
    /// </summary>
    public class OAuth2
    {
        [JsonProperty("implicit")]
        public Flow Implicit { get; set; }

        [JsonProperty("password")]
        public Flow Password { get; set; }

        [JsonProperty("clientCredentials")]
        public Flow ClientCredentials { get; set; }

        [JsonProperty("authorizationCode")]
        public Flow AuthorizationCode { get; set; }
    }
}
