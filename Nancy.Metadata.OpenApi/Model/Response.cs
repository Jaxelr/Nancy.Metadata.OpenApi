using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
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
