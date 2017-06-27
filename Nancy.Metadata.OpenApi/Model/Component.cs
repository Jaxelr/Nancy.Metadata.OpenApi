using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    public class Component
    {
        [JsonProperty("schemas"), JsonConverter(typeof(Core.CustomJsonConverter))]
        public Dictionary<string, NJsonSchema.JsonSchema4> ModelDefinitions { get; set; }
    }
}