using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    public class Component
    {
        // TODO: Complex type properties of complex types under schemas are not properly generated
        // TODO: Properties of Array types are not properly generated

        [JsonProperty("schemas")]
        public IDictionary<string, NJsonSchema.JsonSchema4> ModelDefinitions { get; set; }
    }
}
