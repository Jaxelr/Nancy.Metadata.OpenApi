using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    // https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.0.md#request-body-object
    public class RequestBody
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("content")]
        public Dictionary<string, MediaTypeObject> Content { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }
    }
}
