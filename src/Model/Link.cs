using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#link-object
    /// </summary>
    public class Link
    {
        [JsonProperty("operationRef")]
        public string OperationRef { get; set; }

        [JsonProperty("operationId")]
        public string OperationId { get; set; }

        [JsonProperty("parameters")]
        public IDictionary<string, string> Parameters { get; set; }

        [JsonProperty("requestBody")]
        public IDictionary<string, string> RequestBody { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("server")]
        public Server Server { get; set; }
    }
}
