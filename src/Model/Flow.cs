using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    public class Flow
    {
        [JsonProperty("authorizationUrl")]
        public string AuthorizationUrl { get; set; }

        [JsonProperty("tokenUrl")]
        public string TokenUrl { get; set; }

        [JsonProperty("refreshUrl")]
        public string RefreshUrl { get; set; }

        [JsonProperty("scopes"), JsonConverter(typeof(Core.CustomArrayJsonConverter))]
        public string[] Scopes { get; set; }
    }
}
