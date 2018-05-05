namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
    public class FakeRequestArray
    {
        public string Name => "Names";
        public string Description => "Request description";
        public string Type => "string";
        public string Format => "format";
        public string Loc => "query";
        public bool Required => false;
        public bool Deprecated => false;
        public bool isArray => true;
    }
}
