using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#responseObject
    /// </summary>
    public class Response
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("headers")]
        public IDictionary<string, TypeDefinition> Headers { get; set; }

        [JsonProperty("schema")]
        public SchemaRef Schema { get; set; }
    }
}
