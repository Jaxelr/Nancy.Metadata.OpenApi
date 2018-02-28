using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Modules;
using Nancy.Routing;
using System.Threading.Tasks;

namespace Nancy.Metadata.OpenApi.DemoApplication.Modules
{
    public class DocsModule : OpenApiDocsModuleBase
    {
        public DocsModule(IRouteCacheProvider routeCacheProvider) :
            base(routeCacheProvider,
                "/api/docs",
                "Sample API documentation",
                "v1.0",
                host: new Server
                {
                    Url = "http://localhost:51637/",
                    Description = "My Descripton"
                },
                apiBaseUrl: "/api")
        {
            //Optional information.
            WithContact("myContact", "contactsample@email.com", "http://randomurl.com");

            //Optional information.
            WithLicense("MIT", "https://opensource.org/licenses/MIT");

            //Optional Information.
            WithExternalDocument("This is my external doc.", "https://www.google.com");

            Get("/", async (x, ct) => await Task.Run(() => Response.AsRedirect("/index.html")));
        }
    }
}
