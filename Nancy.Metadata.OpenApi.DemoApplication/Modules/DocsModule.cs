using Nancy.Metadata.OpenApi.Modules;
using Nancy.Routing;
using System.Threading.Tasks;

namespace Nancy.Metadata.OpenApi.DemoApplication.Modules
{
    public class DocsModule : OpenApiDocsModuleBase
    {
        public DocsModule(IRouteCacheProvider routeCacheProvider) : base(routeCacheProvider, "/api/docs", "Sample API documentation", "v1.0", "localhost:5000", "/api", "http")
        {
            Get("/", async (x, ct) => await Task.Run(() => Response.AsRedirect("/index.html")));
        }
    }
}