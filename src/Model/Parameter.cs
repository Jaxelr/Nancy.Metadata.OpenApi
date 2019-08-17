using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#parameterObject
    /// </summary>
    public class Parameter
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

        [JsonProperty("schema")]
        public SchemaRef Schema { get; set; }
    }
}
