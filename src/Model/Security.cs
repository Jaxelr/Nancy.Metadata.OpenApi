using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    public class Security
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("in")]
        public string In { get; set; }

        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        [JsonProperty("bearerFormat")]
        public string BearerFormat { get; set; }

        [JsonProperty("flows")]
        public OAuth2 Flows { get; set; }

        [JsonProperty("openIdConnect")]
        public string OpenIdConnectUrl { get; set; }
    }
}
