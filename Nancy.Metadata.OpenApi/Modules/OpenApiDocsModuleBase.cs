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
        private const string DOCS_LOCATION = "/api/docs";
        private const string API_VERSION = "1.0";
        private const string TITLE = "My Title";
        private const string HOST = "localhost:5000";
        private const string HOST_DESCRIPTION = "My Localhost";
        private const string API_BASE_URL = "/";
        private Server defaultServer = new Server { Url = HOST, Description = HOST_DESCRIPTION };

        private OpenApiSpecification openApiSpecification;
        private readonly IRouteCacheProvider routeCacheProvider;
        private readonly string title;
        private readonly string apiVersion;
        private readonly Server[] hosts;
        private readonly string apiBaseUrl;
        private readonly string termsOfService;
        private Contact contact;
        private License license;

        [System.Obsolete("Deprecated in favor of the usage of constructor with Server model.")]
        protected OpenApiDocsModuleBase(IRouteCacheProvider routeCacheProvider,
            string docsLocation = DOCS_LOCATION,
            string title = TITLE,
            string apiVersion = API_VERSION,
            string host = HOST,
            string hostDescription = HOST_DESCRIPTION,
            string apiBaseUrl = API_BASE_URL) : this(
                    routeCacheProvider,
                    docsLocation,
                    title,
                    apiVersion,
                    hosts: new Server[] { new Server { Url = host, Description = hostDescription } },
                    apiBaseUrl: apiBaseUrl)
        {
        }

        protected OpenApiDocsModuleBase(IRouteCacheProvider routeCacheProvider,
            string docsLocation,
            string title,
            string apiVersion,
            string termsOfService = null,
            Server host = null,
            Contact contact = null,
            License license = null,
            string apiBaseUrl = API_BASE_URL) : this(
                    routeCacheProvider,
                    docsLocation,
                    title,
                    apiVersion,
                    termsOfService,
                    new Server[] { host },
                    contact,
                    license,
                    apiBaseUrl)
        {
        }

        protected OpenApiDocsModuleBase(IRouteCacheProvider routeCacheProvider,
            string docsLocation,
            string title,
            string apiVersion,
            string termsOfService = null,
            Server[] hosts = null,
            Contact contact = null,
            License license = null,
            string apiBaseUrl = API_BASE_URL)
        {
            this.routeCacheProvider = routeCacheProvider;
            this.title = title;
            this.apiVersion = apiVersion;
            this.termsOfService = termsOfService;
            this.hosts = hosts;
            this.contact = contact;
            this.license = license;
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
                    TermsOfService = termsOfService,
                    Contact = contact,
                    License = license
                },
                Servers = hosts
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

                //OpenApi doesnt handle these special characters on the url path construction, but Nancy allows it.
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
