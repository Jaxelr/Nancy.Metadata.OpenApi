using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    public class Info
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("termsOfService")]
        public string TermsOfService { get; set; }

        [JsonProperty("contact")]
        public Contact Contact { get; set; }

        [JsonProperty("license")]
        public License License { get; set; }
    }
}
