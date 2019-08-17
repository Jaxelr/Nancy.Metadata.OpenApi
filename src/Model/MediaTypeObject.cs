using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#media-type-object
    /// </summary>
    public class MediaTypeObject
    {
        [JsonProperty("schema")]
        public SchemaRef Schema { get; set; }

        [JsonProperty("encoding")]
        public Encoding Encoding { get; set; }

        [JsonProperty("examples")]
        public IDictionary<string, Example> Examples { get; set; }
    }
}
