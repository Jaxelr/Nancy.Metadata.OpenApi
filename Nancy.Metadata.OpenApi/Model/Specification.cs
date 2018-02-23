using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    public class OpenApiSpecification
    {
        [JsonProperty("openapi")]
        public string OpenApiVersion => "3.0.0";

        [JsonProperty("info")]
        public Api Api { get; set; }

        [JsonProperty("servers")]
        public Server[] Servers { get; set; }

        [JsonProperty("paths")]
        public IDictionary<string, Dictionary<string, Endpoint>> PathInfos { get; set; }

        [JsonProperty("components")]
        public Component Component { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }
    }
}
