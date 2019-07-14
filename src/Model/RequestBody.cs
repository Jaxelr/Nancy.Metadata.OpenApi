using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#request-body-object
    /// </summary>
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
