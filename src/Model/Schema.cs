using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#schemaObject
    /// </summary>
    public class Schema
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("$ref")]
        public string Ref { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("items")]
        public Schema Items { get; set; }

        [JsonProperty("deprecated")]
        public bool Deprecated { get; set; }

        [JsonProperty("nullable")]
        public bool Nullable { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, Schema>  Properties { get; set; }
    }
}
