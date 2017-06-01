using System;

namespace Nancy.Metadata.OpenApi.Core
{
    public class TypeNameGenerator : NJsonSchema.ITypeNameGenerator, NJsonSchema.ISchemaNameGenerator
    {
        public string Generate(Type type)
        {
            return type.FullName;
        }

        public string Generate(NJsonSchema.JsonSchema4 schema, string typeNameHint)
        {
            return typeNameHint;
        }
    }
}