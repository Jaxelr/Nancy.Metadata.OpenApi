using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    public class Component
    {
        [JsonProperty("schemas")]
        public IDictionary<string, NJsonSchema.JsonSchema4> ModelDefinitions { get; set; }
    }
}
