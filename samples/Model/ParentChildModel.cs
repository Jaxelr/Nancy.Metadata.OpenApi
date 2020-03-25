using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.DemoApplication.Model
{
    public class ParentChildModel
    {
        public IEnumerable<SimpleResponseModel> Responses { get; set; }
    }
}
