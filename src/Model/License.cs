using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    public class License
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}