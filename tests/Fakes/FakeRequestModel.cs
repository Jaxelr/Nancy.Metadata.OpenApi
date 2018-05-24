namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
    public class FakeRequestModel
    {
        public string Name => "Name";
        public string Description => "Request description";
        public string Loc => "body";
        public bool Required => false;
        public string contentType => "application/json";
    }
}
