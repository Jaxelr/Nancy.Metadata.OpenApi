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

        /// <summary>
        /// Default constructor used, inherited from version Swagger version.
        /// </summary>
        /// <param name="routeCacheProvider"></param>
        /// <param name="docsLocation"></param>
        /// <param name="title"></param>
        /// <param name="apiVersion"></param>
        /// <param name="host"></param>
        /// <param name="hostDescription"></param>
        /// <param name="apiBaseUrl"></param>
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

        /// <summary>
        /// New Constructor established for use with Open Api version.
        /// </summary>
        /// <param name="routeCacheProvider"></param>
        /// <param name="docsLocation"></param>
        /// <param name="title"></param>
        /// <param name="apiVersion"></param>
        /// <param name="termsOfService"></param>
        /// <param name="host"></param>
        /// <param name="contact"></param>
        /// <param name="license"></param>
        /// <param name="apiBaseUrl"></param>
        protected OpenApiDocsModuleBase(IRouteCacheProvider routeCacheProvider,
            string docsLocation,
            string title,
            string apiVersion,
            string termsOfService = null,
            Server host = null,
            string apiBaseUrl = API_BASE_URL) : this(
                    routeCacheProvider,
                    docsLocation,
                    title,
                    apiVersion,
                    termsOfService,
                    new Server[] { host },
                    apiBaseUrl)
        {
        }

        /// <summary>
        /// Constructor that contains multiple objects.
        /// </summary>
        /// <param name="routeCacheProvider"></param>
        /// <param name="docsLocation"></param>
        /// <param name="title"></param>
        /// <param name="apiVersion"></param>
        /// <param name="termsOfService"></param>
        /// <param name="hosts"></param>
        /// <param name="contact"></param>
        /// <param name="license"></param>
        /// <param name="apiBaseUrl"></param>
        protected OpenApiDocsModuleBase(IRouteCacheProvider routeCacheProvider,
            string docsLocation,
            string title,
            string apiVersion,
            string termsOfService = null,
            Server[] hosts = null,
            string apiBaseUrl = API_BASE_URL)
        {
            this.routeCacheProvider = routeCacheProvider;
            this.title = title;
            this.apiVersion = apiVersion;
            this.termsOfService = termsOfService;
            this.hosts = hosts;
            this.apiBaseUrl = apiBaseUrl;

            Get(docsLocation, r => GetDocumentation());
        }

        /// <summary>
        /// Add Contract information
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="url"></param>
        protected void WithContact(string name, string email, string url)
            => contact = new Contact()
            {
                Name = name,
                Email = email,
                Url = url
            };

        /// <summary>
        /// Add License Information
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        protected void WithLicense(string name, string url)
            => license = new License()
            {
                Name = name,
                Url = url
            };

        /// <summary>
        /// Generate the json documentation file.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// This operation generates the specification into the openApiSpecification variable.
        /// </summary>
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
