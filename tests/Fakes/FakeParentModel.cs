using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
    public class FakeParentModel
    {
        public string Name => "Name";
        public IEnumerable<FakeChildModel> Children { get; set; }
    }
}
