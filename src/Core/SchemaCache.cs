using Nancy.Metadata.OpenApi.Model;
using NJsonSchema;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Core
{
    public static class SchemaCache
    {
        public static IDictionary<string, JsonSchema> ComponentCache = new Dictionary<string, JsonSchema>();

        public static IDictionary<string, SecurityScheme> SecurityCache = new Dictionary<string, SecurityScheme>();
    }
}
