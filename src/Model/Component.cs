using System.Collections.Generic;
using Newtonsoft.Json;

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
        public Dictionary<string, NJsonSchema.JsonSchema> ModelDefinitions { get; set; }

        [JsonProperty("responses")]
        public Dictionary<string, Response> Responses { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, Header> Headers { get; set; }

        [JsonProperty("parameters")]
        public Dictionary<string, Parameter> Parameters { get; set; }

        [JsonProperty("requestBodies")]
        public Dictionary<string, RequestBody> RequestBodies { get; set; }

        [JsonProperty("examples")]
        public Dictionary<string, Example> Examples { get; set; }

        [JsonProperty("securitySchemes")]
        public Dictionary<string, SecurityScheme> SecuritySchemes { get; set; }

        [JsonProperty("callbacks")]
        public Dictionary<string, Callback> Callbacks { get; set; }

        [JsonProperty("links")]
        public Dictionary<string, Link> Links { get; set; }

        public Component()
        {
            ModelDefinitions = new Dictionary<string, NJsonSchema.JsonSchema>();
        }
    }
}
