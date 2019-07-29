namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
    public class FakeResponse
    {
        public string StatusCode => "200";
        public HttpStatusCode HttpStatusCode => HttpStatusCode.OK;
        public string Description => "Response Description";
    }
}
