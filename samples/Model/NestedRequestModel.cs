using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.DemoApplication.Model
{
    public class NestedRequestModel
    {
        [JsonProperty("primitiveProperty")]
        public string PrimitiveProperty { get; set; }

        [JsonProperty("simpleModel")]
        public SimpleRequestModel SimpleModel { get; set; }
    }
}