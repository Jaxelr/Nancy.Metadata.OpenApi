using Nancy.Metadata.OpenApi.Model;
using Nancy.Routing;

namespace Nancy.Metadata.OpenApi.Core
{
    public class OpenApiRouteMetadata
    {
        public OpenApiRouteMetadata(string path, string method, string name)
        {
            Path = path;
            Method = method.ToLower();
            Name = name;
        }

        public OpenApiRouteMetadata(RouteDescription desc) : this(desc.Path, desc.Method, desc.Name)
        {
        }

        public string Path { get; set; }

        public string Method { get; set; }

        public string Name { get; set; }

        public Endpoint Info { get; set; }
    }
}