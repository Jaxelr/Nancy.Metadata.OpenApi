using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    public class Component
    {
        [JsonProperty("schemas"), JsonConverter(typeof(Core.CustomJsonConverter))]
        public IDictionary<string, NJsonSchema.JsonSchema4> ModelDefinitions { get; set; }
    }
}
