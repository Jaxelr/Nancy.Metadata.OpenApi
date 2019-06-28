using System;
using System.Collections.Generic;
using Nancy.Metadata.OpenApi.Core;

namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
    public class FakeRequestCollection
    {
        public string Name => "Names";
        public string Description => "Request description";
        public Type Type => typeof(IEnumerable<string>);
        public string Format => "format";
        public Loc Loc => Loc.Query;
        public bool Required => false;
        public bool Deprecated => false;
    }
}
