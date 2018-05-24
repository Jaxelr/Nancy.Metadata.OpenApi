using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    public class RequestBody
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("content")]
        public Dictionary<string, SchemaRef> Content { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }
    }
}
