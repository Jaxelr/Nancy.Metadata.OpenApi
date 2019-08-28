using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#oasObject
    /// </summary>
    public class OpenApiSpecification
    {
        [JsonProperty("openapi")]
        public string OpenApiVersion => "3.0.2";

        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("servers")]
        public Server[] Servers { get; set; }

        [JsonProperty("security"), JsonConverter(typeof(Core.CustomCollectionJsonConverter))]
        public IList<Security> Security { get; set; }

        [JsonProperty("paths")]
        public IDictionary<string, Dictionary<string, Endpoint>> PathInfos { get; set; }

        [JsonProperty("components")]
        public Component Component { get; set; }

        [JsonProperty("tags")]
        public IEnumerable<Tag> Tags { get; set; }

        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }

        public OpenApiSpecification()
        {
            Component = new Component();
            ExternalDocs = new ExternalDocumentation();
        }
    }
}
