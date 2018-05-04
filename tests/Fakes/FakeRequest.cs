namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
    public class FakeRequest
    {
        public string Name => "Name";
        public string Description => "Request description";
        public string Type => "string";
        public string Format => "format";
        public string Loc => "query";
        public bool Required => false;
        public bool Deprecated => false;
        public bool isArray => false;
    }
}
