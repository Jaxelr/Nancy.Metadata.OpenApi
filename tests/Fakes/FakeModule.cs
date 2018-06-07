using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Modules;
using Nancy.Routing;

namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
    public class FakeModule : OpenApiDocsModuleBase
    {
        public static Server Server => new Server() { Description = "My Descripton", Url = "http://localhost:5000/" };
        public static string DocsLocation = "/api/docs";
        public static string Title = "Sample API Documentation";
        public static string ApiVersion = "v1.0";
        public static string ApiBaseUrl = "/";
        public static string TermsOfService = "blah blah blah";
        public static Tag[] Tags = { new Tag() { Name = "Default" } };

        public FakeModule(IRouteCacheProvider routeCacheProvider) :
            base(routeCacheProvider,
                DocsLocation,
                Title,
                ApiVersion,
                termsOfService: TermsOfService,
                host: Server,
                tags: Tags)
        {
        }

        public void FillContact(string name, string email, string url) => WithContact(name, email, url);

        public void FillExternalDoc(string desc, string url) => WithExternalDocument(desc, url);

        public void FillLicense(string name, string url) => WithLicense(name, url);
    }
}
