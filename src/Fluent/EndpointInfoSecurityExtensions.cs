using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nancy.Metadata.OpenApi.Fluent
{
    public static class EndpointInfoSecurityExtensions
    {
        /// <summary>
        /// Add documentation pertinent to basic authentication on the endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Endpoint WithBasicAuthentication(this Endpoint endpointInfo, string description = null)
        {
            string securityKey = "basic";

            var security = new SecurityScheme()
            {
                Type = "http",
                Scheme = "basic",
                Name = securityKey,
                Description = description
            };

            return SaveAuthentication(endpointInfo, securityKey, security);
        }

        /// <summary>
        /// Add documentation pertinent to custom key authentication on the endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="name"></param>
        /// <param name="location">Plausible values are cookie, header and query</param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Endpoint WithApiKeyAuthentication(this Endpoint endpointInfo, string name, string location, string description = null)
        {
            var security = new SecurityScheme()
            {
                Type = "apiKey",
                Name = name,
                In = location,
                Description = description
            };

            return SaveAuthentication(endpointInfo, name, security);
        }

        /// <summary>
        /// Add documentation pertinent to bearer (jwt) authentication on the endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="bearerFormat">The format of the bearer</param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Endpoint WithBearerAuthentication(this Endpoint endpointInfo, string bearerFormat, string description = null)
        {
            string securityKey = "bearer";

            var security = new SecurityScheme()
            {
                Type = "http",
                Scheme = "bearer",
                Name = securityKey,
                BearerFormat = bearerFormat,
                Description = description
            };

            return SaveAuthentication(endpointInfo, securityKey, security);
        }

        /// <summary>
        /// Add documentation pertinent to OpenId authentication on the endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="url">A valid url to refer the client</param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Endpoint WithOpenIdConnectAuthentication(this Endpoint endpointInfo, string authorizationUrl, string flow,
            string tokenUrl, string openIdConnectUrl, string description = null, string refreshUrl = null, params string[] scopes)
        {
            string securityKey = "openIdConnect";

            var flowspec = new Flow()
            {
                AuthorizationUrl = authorizationUrl,
                TokenUrl = tokenUrl,
                RefreshUrl = refreshUrl,
                Scopes = scopes
            };

            var oauth2 = MatchFlow(flowspec, flow);


            var security = new SecurityScheme()
            {
                Type = "openIdConnect",
                OpenIdConnectUrl = openIdConnectUrl,
                Flows = oauth2,
                Name = securityKey,
                Description = description
            };

            return SaveAuthentication(endpointInfo, securityKey, security);
        }

        public static Endpoint WithOAuth2Authentication(this Endpoint endpointInfo, string authorizationUrl, string flow,
            string tokenUrl, string description = null, string refreshUrl = null, params string[] scopes)
        {
            string securityKey = "oauth2";

            var flowspec = new Flow()
            {
                AuthorizationUrl = authorizationUrl,
                TokenUrl = tokenUrl,
                RefreshUrl = refreshUrl,
                Scopes = scopes
            };

            var oauth2 = MatchFlow(flowspec, flow);

            var security = new SecurityScheme()
            {
                Type = "oauth2",
                Flows = oauth2,
                Name = securityKey,
                Description = description
            };

            return SaveAuthentication(endpointInfo, securityKey, security);
        }

        private static OAuth2 MatchFlow(Flow flowspec, string flow)
        {
            var oauth2 = new OAuth2();
            switch (flow.ToLowerInvariant())
            {
                case "implicit":
                    oauth2.Implicit = flowspec;
                    break;
                case "authorizationcode":
                    oauth2.AuthorizationCode = flowspec;
                    break;
                case "clientcredentials":
                    oauth2.ClientCredentials = flowspec;
                    break;
                case "password":
                    oauth2.Password = flowspec;
                    break;
            }

            return oauth2;
        }

        private static Endpoint SaveAuthentication(this Endpoint endpointInfo, string key, SecurityScheme security, params string[] scopes)
        {
            if (endpointInfo.Security is null)
            {
                endpointInfo.Security = new List<Model.Security>();
            }

            endpointInfo.Security.Add(new Model.Security() { Key = key, Scopes = scopes });

            SchemaGenerator.GetOrSaveSecurity(security);

            return endpointInfo;
        }
    }
}
