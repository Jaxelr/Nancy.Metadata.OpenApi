using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#encodingObject
    /// </summary>
    public class Encoding
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("explode")]
        public bool Explode { get; set; }

        [JsonProperty("allowReserved")]
        public bool AllowReserved { get; set; }
    }
}
