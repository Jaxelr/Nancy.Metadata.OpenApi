using System;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
    public class FakeRequestCollection
    {
        public string Name => "Names";
        public string Description => "Request description";
        public Type Type => typeof(IEnumerable<string>);
        public string Format => "format";
        public string Loc => "query";
        public bool Required => false;
        public bool Deprecated => false;
    }
}
