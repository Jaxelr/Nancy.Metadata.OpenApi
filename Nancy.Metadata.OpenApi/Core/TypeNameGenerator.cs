using System;
using System.Collections.Generic;
using NJsonSchema;

namespace Nancy.Metadata.OpenApi.Core
{
    public class TypeNameGenerator : ITypeNameGenerator, ISchemaNameGenerator
    {
        public string Generate(Type type)
        {
            return type.FullName;
        }

        public string Generate(JsonSchema4 schema, string typeNameHint, IEnumerable<string> reservedTypeNames)
        {
            return typeNameHint;
        }
    }
}