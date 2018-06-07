using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    public class Tag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocumentation { get; set; }
    }
}
