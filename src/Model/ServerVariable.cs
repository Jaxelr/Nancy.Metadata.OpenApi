using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    public class ServerVariable
    {
        [JsonProperty("enum")]
        public IEnumerable<string> Enum { get; set; }

        [JsonProperty("default")]
        public string Default { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
