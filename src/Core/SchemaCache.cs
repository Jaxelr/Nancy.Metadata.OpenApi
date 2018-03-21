using System.Collections.Generic;
using NJsonSchema;

namespace Nancy.Metadata.OpenApi.Core
{
    public static class SchemaCache
    {
        public static IDictionary<string, JsonSchema4> Cache = new Dictionary<string, JsonSchema4>();
    }
}
