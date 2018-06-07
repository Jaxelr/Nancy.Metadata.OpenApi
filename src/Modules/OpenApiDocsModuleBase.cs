﻿using Nancy.Metadata.OpenApi.Core;
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

        private OpenApiSpecification openApiSpecification;
        private readonly IRouteCacheProvider routeCacheProvider;
        private readonly string title;
        private readonly string apiVersion;
        private readonly Server[] hosts;
        private readonly string termsOfService;
        private readonly Tag[] tags;
        private Contact contact;
        private License license;
        private ExternalDocumentation externalDocs;

        /// <summary>
        /// Constructor with minimal amount of values required values.
        /// </summary>
        /// <param name="routeCacheProvider"></param>
        /// <param name="docsLocation"></param>
        /// <param name="title"></param>
        /// <param name="apiVersion"></param>
        public OpenApiDocsModuleBase(IRouteCacheProvider routeCacheProvider,
            string docsLocation,
            string title,
            string apiVersion) : this(routeCacheProvider,
                docsLocation,
                title,
                apiVersion,
                null,
                new Server { Url = HOST, Description = HOST_DESCRIPTION },
                null)

        {
        }

        /// <summary>
        /// Constructor established for use with Open Api version as a basic version
        /// </summary>
        /// <param name="routeCacheProvider"></param>
        /// <param name="docsLocation"></param>
        /// <param name="title"></param>
        /// <param name="apiVersion"></param>
        /// <param name="termsOfService"></param>
        /// <param name="host"></param>
        /// <param name="contact"></param>
        /// <param name="license"></param>
        /// <param name="tags"></param>
        protected OpenApiDocsModuleBase(IRouteCacheProvider routeCacheProvider,
            string docsLocation,
            string title,
            string apiVersion,
            string termsOfService = null,
            Server host = null,
            Tag[] tags = null) : this(
                    routeCacheProvider,
                    docsLocation,
                    title,
                    apiVersion,
                    termsOfService,
                    new Server[] { host },
                    tags)
        {
        }

        /// <summary>
        /// Constructor that contains relevant doc information to generate the root endpoint
        /// it also invokes the GetDocumentation on the specified path
        /// </summary>
        /// <param name="routeCacheProvider"></param>
        /// <param name="docsLocation"></param>
        /// <param name="title"></param>
        /// <param name="apiVersion"></param>
        /// <param name="termsOfService"></param>
        /// <param name="hosts"></param>
        /// <param name="contact"></param>
        /// <param name="license"></param>
        /// <param name="tags"></param>
        protected OpenApiDocsModuleBase(IRouteCacheProvider routeCacheProvider,
            string docsLocation,
            string title,
            string apiVersion,
            string termsOfService = null,
            Server[] hosts = null,
            Tag[] tags = null)
        {
            this.routeCacheProvider = routeCacheProvider;
            this.title = title;
            this.apiVersion = apiVersion;
            this.termsOfService = termsOfService;
            this.hosts = hosts;
            this.tags = tags;

#if NETSTANDARD1_6
            Get(docsLocation, r => GetDocumentation());
#else
            Get[docsLocation] = r => GetDocumentation();
#endif
        }

        /// <summary>
        /// Add (optional) Contract information
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="url"></param>
        protected void WithContact(string name, string email, string url) =>
            contact = new Contact()
            {
                Name = name,
                Email = email,
                Url = url
            };

        /// <summary>
        /// Add (optional) License Information
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        protected void WithLicense(string name, string url) =>
            license = new License()
            {
                Name = name,
                Url = url
            };

        /// <summary>
        /// Add (optional) External Document
        /// </summary>
        /// <param name="description"></param>
        /// <param name="url"></param>
        protected void WithExternalDocument(string description, string url) =>
            externalDocs = new ExternalDocumentation()
            {
                Description = description,
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
        /// This operation generates the specification upon the openApiSpecification variable.
        /// </summary>
        private void GenerateSpecification()
        {
            openApiSpecification = new OpenApiSpecification
            {
                Info = new Info
                {
                    Title = title,
                    Version = apiVersion,
                    TermsOfService = termsOfService,
                    Contact = contact,
                    License = license,
                },
                Servers = hosts,
                Tags = tags,
                ExternalDocs = externalDocs
            };

            // Generate documentation
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
