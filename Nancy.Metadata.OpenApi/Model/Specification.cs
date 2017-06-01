using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    public class OpenApiSpecification
    {
        [JsonProperty("openapi")]
        public string OpenApiVersion { get { return "3.0.0"; } }

        [JsonProperty("info")]
        public Api Api { get; set; }

        [JsonProperty("servers")]
        public Server[] Servers { get; set; }

        [JsonProperty("schemes")]
        public string[] Schemes { get; set; }

        [JsonProperty("paths")]
        public Dictionary<string, Dictionary<string, Endpoint>> PathInfos { get; set; }

        [JsonProperty("definitions"), JsonConverter(typeof(Core.CustomJsonConverter))]
        public Dictionary<string, NJsonSchema.JsonSchema4> ModelDefinitions { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("externalDocs")]
        public ExternalDocumentation externalDocs { get; set; }
    }
}