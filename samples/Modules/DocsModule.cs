using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Modules;
using Nancy.Routing;

namespace Nancy.Metadata.OpenApi.DemoApplication.Modules
{
    public class DocsModule : OpenApiDocsModuleBase
    {
        public static Server Server => new Server() { Description = "My Descripton", Url = "http://localhost:51637/" };

        public DocsModule(IRouteCacheProvider routeCacheProvider) :
            base(routeCacheProvider,
                "/api/docs",
                "Sample API documentation",
                "v1.0",
                host: Server,
                apiBaseUrl: "/api")
        {
            //Optional information.
            WithContact("Contact Information", "jaxelrojas@email.com", "https://jaxelr.github.io");

            //Optional information.
            WithLicense("MIT", "https://opensource.org/licenses/MIT");

            //Optional Information.
            WithExternalDocument("This is an external doc, maybe a tutorial or a spec doc.", "https://jaxelr.github.io");

            Get("/", async (x, ct) => await Response.AsRedirect("/index.html"));
        }
    }
}
