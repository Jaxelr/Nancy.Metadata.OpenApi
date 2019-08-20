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
        public IDictionary<string, NJsonSchema.JsonSchema> ModelDefinitions { get; set; }

        [JsonProperty("responses")]
        public IDictionary<string, Response> Responses { get; set; }

        [JsonProperty("headers")]
        public IDictionary<string, Header> Headers { get; set; }

        [JsonProperty("parameters")]
        public IDictionary<string, Parameter> Parameters { get; set; }

        [JsonProperty("requestBodies")]
        public IDictionary<string, RequestBody> RequestBodies { get; set; }

        [JsonProperty("examples")]
        public IDictionary<string, Example> Examples { get; set; }

        [JsonProperty("securitySchemes")]
        public IDictionary<string, SecurityScheme> SecuritySchemes { get; set; }

        [JsonProperty("callbacks")]
        public IDictionary<string, Callback> Callbacks { get; set; }

        [JsonProperty("links")]
        public IDictionary<string, Link> Links { get; set; }
    }
}
