using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    public class Server
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}