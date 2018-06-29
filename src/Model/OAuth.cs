using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    public class OAuth
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
