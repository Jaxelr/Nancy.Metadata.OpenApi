using Nancy.Metadata.OpenApi.Model;

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
            if (endpointInfo.Security is null)
            {
                endpointInfo.Security = new Model.Security()
                {
                    Type = "http",
                    Scheme = "basic",
                    Description = description
                };
            }

            return endpointInfo;
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
            if (endpointInfo.Security is null)
            {
                endpointInfo.Security = new Model.Security()
                {
                    Type = "apiKey",
                    Name = name,
                    In = location,
                    Description = description
                };
            }

            return endpointInfo;
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
            if (endpointInfo.Security is null)
            {
                endpointInfo.Security = new Model.Security()
                {
                    Type = "http",
                    Scheme = "bearer",
                    Description = description
                };
            }

            return endpointInfo;
        }

        /// <summary>
        /// Add documentation pertinent to OpenId authentication on the endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="url">A valid url to refer the client</param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Endpoint WithOpenIdConnectAuthentication(this Endpoint endpointInfo, string url, string description = null)
        {
            if (endpointInfo.Security is null)
            {
                endpointInfo.Security = new Model.Security()
                {
                    Type = "openIdConnect",
                    OpenIdConnectUrl = url,
                    Description = description
                };
            }

            return endpointInfo;
        }
    }
}
