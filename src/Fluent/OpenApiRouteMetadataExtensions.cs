using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Model;
using System;

namespace Nancy.Metadata.OpenApi.Fluent
{
    public static class OpenApiRouteMetadataExtensions
    {
        /// <summary>
        /// Generate a new endpoint reoute which will add request / response objects.
        /// </summary>
        /// <param name="routeMetadata"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static OpenApiRouteMetadata With(this OpenApiRouteMetadata routeMetadata,
            Func<Endpoint, Endpoint> info)
        {
            routeMetadata.Info = info(routeMetadata.Info ?? new Endpoint(routeMetadata.Name));

            return routeMetadata;
        }
    }
}
