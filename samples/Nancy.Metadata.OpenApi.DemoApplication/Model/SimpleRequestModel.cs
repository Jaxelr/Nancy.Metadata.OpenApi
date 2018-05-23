using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nancy.Metadata.OpenApi.DemoApplication.Model
{
    public class SimpleRequestModel
    {
        [Required]
        public string Name { get; set; }

        [JsonProperty("array1")]
        public List<string> FirstArray { get; set; }

        [JsonProperty("array2")]
        public List<string> SecondArray { get; set; }
    }
}
