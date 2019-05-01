using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.DemoApplication.Model
{
    public class NestedRequestModel
    {
        public string PrimitiveProperty { get; set; }

        public SimpleRequestModel SimpleModel { get; set; }
    }
}
