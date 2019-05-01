using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nancy.Metadata.OpenApi.DemoApplication.Model
{
    public class SimpleRequestModel
    {
        [Required]
        public string Name { get; set; }

        public IEnumerable<string> FirstArray { get; set; }

        public IEnumerable<string> SecondArray { get; set; }

    }
}
