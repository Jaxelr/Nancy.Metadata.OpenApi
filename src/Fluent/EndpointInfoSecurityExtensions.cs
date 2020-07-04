using System;
using System.Collections.Generic;
using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Model;

namespace Nancy.Metadata.OpenApi.Fluent
{
    public static class EndpointInfoSecurityExtensions
    {
        private const string InvalidLocationMessage = "The location of the ApiKey cannot be documented on the path";

        /// <summary>
        /// Add documentation pertinent to basic authentication on the endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="description">A description for the basic authentication endpoint</param>
        /// <returns></returns>
        public static Endpoint WithBasicAuthentication(this Endpoint endpointInfo, string description = null)
        {
            const string type = "http", scheme = "basic";
            const string securityKey = "basic";

            var security = new SecurityScheme()
            {
                Type = type,
                Scheme = scheme,
                Name = securityKey,
                Description = description
            };

            return SaveAuthentication(endpointInfo, securityKey, security);
        }

        /// <summary>
        /// Add documentation pertinent to a custom Api Key authentication on the endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="name">The name of the ApiKey</param>
        /// <param name="location">Plausible values are cookie, header and query</param>
        /// <param name="description">A description for the ApiKey authentication endpoint</param>
        /// <returns></returns>
        [Obsolete("This operation will be removed on future versions, favoring the enum with location")]
        public static Endpoint WithApiKeyAuthentication(this Endpoint endpointInfo, string name, string location, string description = null)
        {
            const string type = "apiKey";

            var security = new SecurityScheme()
            {
                Type = type,
                Name = name,
                In = location,
                Description = description
            };

            return SaveAuthentication(endpointInfo, name, security);
        }

        /// <summary>
        /// Add documentation pertinent to a custom Api Key authentication on the endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="name">The name of the ApiKey</param>
        /// <param name="location">An enumerable with plausible valid locations as cookie, header and query</param>
        /// <param name="description">A description for the ApiKey authentication endpoint</param>
        /// <returns></returns>
        public static Endpoint WithApiKeyAuthentication(this Endpoint endpointInfo, string name, Loc location, string description = null)
        {
            const string type = "apiKey";

            if (location == Loc.Path)
            {
                throw new ArgumentOutOfRangeException(InvalidLocationMessage);
            }

            var security = new SecurityScheme()
            {
                Type = type,
                Name = name,
                In = LocGenerator.GetLocByEnum(location),
                Description = description
            };

            return SaveAuthentication(endpointInfo, name, security);
        }

        /// <summary>
        /// Add documentation pertinent to bearer (jwt) authentication on the endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="bearerFormat">The format of the bearer</param>
        /// <param name="description">A description for the bearer authentication endpoint</param>
        /// <returns></returns>
        public static Endpoint WithBearerAuthentication(this Endpoint endpointInfo, string bearerFormat, string description = null)
        {
            const string type = "http", scheme = "bearer";
            const string securityKey = "bearer";

            var security = new SecurityScheme()
            {
                Type = type,
                Scheme = scheme,
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
        /// <param name="description">A description for the OpenId connect authentication endpoint</param>
        /// <returns></returns>
        public static Endpoint WithOpenIdConnectAuthentication(this Endpoint endpointInfo, string authorizationUrl, string flow,
            string tokenUrl, string openIdConnectUrl, string description = null, string refreshUrl = null, params string[] scopes)
        {
            const string type = "openIdConnect";
            const string securityKey = "openIdConnect";

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
                Type = type,
                OpenIdConnectUrl = openIdConnectUrl,
                Flows = oauth2,
                Name = securityKey,
                Description = description
            };

            return SaveAuthentication(endpointInfo, securityKey, security);
        }

        /// <summary>
        /// Add documentation pertinent to OAuth2 authentication on the endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="authorizationUrl">The url to obtain authorization</param>
        /// <param name="flow">The flow defined based on the OAuth2 standards</param>
        /// <param name="tokenUrl">The initial url to obtain tokens</param>
        /// <param name="description">A description for the OAuth2 authentication endpoint</param>
        /// <param name="refreshUrl">The url to refresh tokens obtained</param>
        /// <param name="scopes">The scopes that are accesable</param>
        /// <returns></returns>
        public static Endpoint WithOAuth2Authentication(this Endpoint endpointInfo, string authorizationUrl, string flow,
            string tokenUrl, string description = null, string refreshUrl = null, params string[] scopes)
        {
            const string type = "oauth2";

            string securityKey = string.Concat(type, flow);

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
                Type = type,
                Flows = oauth2,
                Name = securityKey,
                Description = description
            };

            return SaveAuthentication(endpointInfo, securityKey, security);
        }

        /// <summary>
        /// Match the flow to the flow name
        /// </summary>
        /// <param name="flowspec"></param>
        /// <param name="flow"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Save authentication on the authentication cache
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="key"></param>
        /// <param name="security"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        private static Endpoint SaveAuthentication(this Endpoint endpointInfo, string key, SecurityScheme security, params string[] scopes)
        {
            (endpointInfo.Security ??= new List<Model.Security>()).Add(new Model.Security() { Key = key, Scopes = scopes });

            SchemaGenerator.GetOrSaveSecurity(security);

            return endpointInfo;
        }
    }
}
