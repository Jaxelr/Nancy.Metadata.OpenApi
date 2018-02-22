﻿using Nancy.Metadata.OpenApi.Model;
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
                new Server
                {
                    Url = "http://localhost:51637/",
                    Description = "My Descripton"
                },
                "/api")
        {
            Get("/", async (x, ct) => await Task.Run(() => Response.AsRedirect("/index.html")));
        }
    }
}
