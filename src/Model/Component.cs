using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#componentsObject
    /// </summary>
    public class Component
    {
        // TODO: Complex type properties of complex types under schemas are not properly generated
        // TODO: Properties of Array types are not properly generated

        [JsonProperty("schemas")]
        public IDictionary<string, NJsonSchema.JsonSchema> ModelDefinitions { get; set; }

        [JsonProperty("securitySchemes")]
        public IDictionary<string, SecurityScheme> SecuritySchemes { get; set; }
    }
}
