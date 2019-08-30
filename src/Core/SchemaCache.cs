using System.Collections.Generic;
using Nancy.Metadata.OpenApi.Model;

namespace Nancy.Metadata.OpenApi.Core
{
    public static class SchemaCache
    {
        public static IDictionary<string, Schema> ComponentCache = new Dictionary<string, Schema>();

        public static IDictionary<string, SecurityScheme> SecurityCache = new Dictionary<string, SecurityScheme>();
    }
}
