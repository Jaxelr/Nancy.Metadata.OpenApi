using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    public class TypeDefinition
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("required")]
        public List<string> RequiredProperties { get; set; }

        [JsonProperty("properties")]
        public IDictionary<string, TypeDefinition> Properties { get; set; }
    }
}
