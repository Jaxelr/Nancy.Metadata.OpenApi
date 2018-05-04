using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    public class SchemaRef
    {
        [JsonProperty("$ref")]
        public string Ref { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("items")]
        public Item Item { get; set; }
    }
}
