using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    public class RequestParameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("in")]
        public string In { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        [JsonProperty("deprecated")]
        public bool Deprecated { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("schema")]
        public SchemaRef Schema { get; set; }
    }
}
