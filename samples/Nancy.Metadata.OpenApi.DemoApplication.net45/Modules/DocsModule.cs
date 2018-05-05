using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Modules;
using Nancy.Routing;

namespace Nancy.Metadata.OpenApi.DemoApplication.net45.Modules
{
    public class DocsModule : OpenApiDocsModuleBase
    {
        public static Server Server => new Server() { Description = "My Descripton", Url = "http://localhost:5000/" };

        public DocsModule(IRouteCacheProvider routeCacheProvider) :
        base(routeCacheProvider,
        "/api/docs",
        "Sample API documentation",
        "v1.0",
        host: Server,
        apiBaseUrl: "/api")
        {
            Get["/"] = _ => Response.AsRedirect("/Content/index.html");
        }
    }
}
