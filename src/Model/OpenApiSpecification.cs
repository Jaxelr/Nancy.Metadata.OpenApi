using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    public class OpenApiSpecification
    {
        [JsonProperty("openapi")]
        public string OpenApiVersion => "3.0.0";

        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("servers")]
        public Server[] Servers { get; set; }

        [JsonProperty("security"), JsonConverter(typeof(Core.CustomJsonConverter))]
        public IList<Security> Security { get; set; }

        [JsonProperty("paths")]
        public IDictionary<string, Dictionary<string, Endpoint>> PathInfos { get; set; }

        [JsonProperty("components")]
        public Component Component { get; set; }

        [JsonProperty("tags")]
        public IEnumerable<Tag> Tags { get; set; }

        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }
    }
}
