namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
    public class FakeEndpoint
    {
        public string OperationName => "FakeOperation";
        public string Description => "Sample Description";
        public string Summary => "Operation Summary";
        public string[] Tags => new[] { "Tag1", "Tag2" };
        public string ExternalDocsUrl => "http://localhost:8080";
        public string ExternalDocs => "My Docs";
    }
}
