using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Model;
using Nancy.Routing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nancy.Metadata.OpenApi.Modules
{
    public abstract class OpenApiDocsModuleBase : NancyModule
    {
        private const string CONTENT_TYPE = "application/json";

        private OpenApiSpecification openApiSpecification;
        private readonly IRouteCacheProvider routeCacheProvider;
        private readonly string title;
        private readonly string apiVersion;
        private readonly Server host;
        private readonly string apiBaseUrl;

        protected OpenApiDocsModuleBase(IRouteCacheProvider routeCacheProvider,
            string docsLocation = "/api/docs",
            string title = "API documentation",
            string apiVersion = "1.0",
            string host = "localhost:5000",
            string hostDescription = "My Localhost",
            string apiBaseUrl = "/")
        {
            this.routeCacheProvider = routeCacheProvider;
            this.title = title;
            this.apiVersion = apiVersion;
            this.host = new Server { Url = host, Description = hostDescription };
            this.apiBaseUrl = apiBaseUrl;

            Get(docsLocation, r => GetDocumentation());
        }


        public virtual Response GetDocumentation()
        {
            if (openApiSpecification == null)
            {
                GenerateSpecification();
            }

            return Response
                    .AsText(JsonConvert.SerializeObject(openApiSpecification,
                    Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }))
                    .WithContentType(CONTENT_TYPE);
        }

        private void GenerateSpecification()
        {
            openApiSpecification = new OpenApiSpecification
            {
                Api = new Api
                {
                    Title = title,
                    Version = apiVersion,
                },
                Servers = new Server[] { host }
            };

            // generate documentation
            IEnumerable<OpenApiRouteMetadata> metadata = routeCacheProvider.GetCache().RetrieveMetadata<OpenApiRouteMetadata>();

            var endpoints = new Dictionary<string, Dictionary<string, Endpoint>>();

            foreach (OpenApiRouteMetadata m in metadata)
            {
                if (m == null)
                {
                    continue;
                }

                string path = m.Path;

                //Swagger doesnt handle these special characters on the url path construction, but Nancy allows it.
                path = Regex.Replace(path, "[?:.*]", string.Empty);

                if (!endpoints.ContainsKey(path))
                {
                    endpoints[path] = new Dictionary<string, Endpoint>();
                }

                endpoints[path].Add(m.Method, m.Info);

                // add definitions
                if (openApiSpecification.Component == null)
                {
                    openApiSpecification.Component = new Component();

                    if (openApiSpecification.Component.ModelDefinitions == null)
                    {
                        openApiSpecification.Component.ModelDefinitions = new Dictionary<string, NJsonSchema.JsonSchema4>();
                    }
                }

                foreach (string key in SchemaCache.Cache.Keys)
                {
                    if (openApiSpecification.Component.ModelDefinitions.ContainsKey(key))
                    {
                        continue;
                    }

                    openApiSpecification.Component.ModelDefinitions.Add(key, SchemaCache.Cache[key]);
                }
            }

            openApiSpecification.PathInfos = endpoints;
        }
    }
}
