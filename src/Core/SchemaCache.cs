using System.Collections.Generic;
using Nancy.Metadata.OpenApi.Model;

namespace Nancy.Metadata.OpenApi.Core
{
    public static class SchemaCache
    {
        public static Dictionary<string, Schema> ComponentCache = new Dictionary<string, Schema>();

        public static Dictionary<string, SecurityScheme> SecurityCache = new Dictionary<string, SecurityScheme>();
    }
}
