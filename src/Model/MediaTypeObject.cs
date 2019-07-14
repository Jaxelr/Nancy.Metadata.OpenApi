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

        // TODO: Add example
        // TODO: Add examples
        // TODO: Add encoding
    }
}
