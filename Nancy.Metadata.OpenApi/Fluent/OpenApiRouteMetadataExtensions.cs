using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Model;
using System;

namespace Nancy.Metadata.OpenApi.Fluent
{
    public static class OpenApiRouteMetadataExtensions
    {
        public static OpenApiRouteMetadata With(this OpenApiRouteMetadata routeMetadata,
            Func<Endpoint, Endpoint> info)
        {
            routeMetadata.Info = info(routeMetadata.Info ?? new Endpoint(routeMetadata.Name));

            return routeMetadata;
        }
    }
}