using Nancy.Metadata.OpenApi.Model;
using NJsonSchema;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Core
{
    internal static class SchemaCache
    {
        internal static IDictionary<string, JsonSchema4> ComponentCache = new Dictionary<string, JsonSchema4>();

        internal static IDictionary<string, SecurityScheme> SecurityCache = new Dictionary<string, SecurityScheme>();
    }
}
