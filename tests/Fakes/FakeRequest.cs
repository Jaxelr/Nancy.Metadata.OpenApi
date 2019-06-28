using System;
using Nancy.Metadata.OpenApi.Core;

namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
    public class FakeRequest
    {
        public string Name => "Name";
        public string Description => "Request description";
        public Type Type => typeof(string);
        public string Format => "format";
        public Loc Loc => Loc.Query;
        public bool Required => false;
        public bool Deprecated => false;
    }
}
