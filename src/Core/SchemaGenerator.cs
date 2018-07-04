using Nancy.Metadata.OpenApi.Model;
using NJsonSchema;
using System;
using System.Collections;

namespace Nancy.Metadata.OpenApi.Core
{
    internal static class SchemaGenerator
    {
        /// <summary>
        /// Look up schema on schema cache, if not present add a new key.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string GetOrSaveSchemaReference(Type type)
        {
            string key = type.FullName;

            if (SchemaCache.Cache.ContainsKey(key))
            {
                return key;
            }

            var taskSchema = JsonSchema4.FromTypeAsync(type, new NJsonSchema.Generation.JsonSchemaGeneratorSettings
            {
                SchemaType = SchemaType.OpenApi3,
                TypeNameGenerator = new TypeNameGenerator(),
                SchemaNameGenerator = new TypeNameGenerator()
            });

            SchemaCache.Cache[key] = taskSchema.Result;

            return key;
        }

        /// <summary>
        /// Matches the type, format and item (if array) to the schema specified on the parameter.
        /// </summary>
        /// <param name="type">The type defined for the parameter, check: https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.0.md#dataTypeFormat for guidelines</param>
        /// <returns></returns>
        internal static SchemaRef GetSchemaByType(Type type)
        {
            bool isCollection = false;

            if (type.IsArray)
            {
                type = type.GetElementType();
                isCollection = true;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type) && (type != typeof(string)))
            {
                type = type.GetType().GetGenericArguments()[0];
                isCollection = true;
            }

            SchemaRef schema;
            string schemaType = null, format = null;

            switch (Type.GetTypeCode(type)) //formats as defined by OAS:
            {
                case TypeCode.String:
                    schemaType = "string";
                    format = null;
                    break;

                case TypeCode.Int16:
                case TypeCode.Int32:  //single, integer
                    schemaType = "integer";
                    format = "int32";
                    break;

                case TypeCode.Int64:
                    schemaType = "integer";
                    format = "int64";
                    break;

                case TypeCode.Decimal:
                    schemaType = "number";
                    format = "float";
                    break;

                case TypeCode.Double:
                    schemaType = "number";
                    format = "double";
                    break;

                case TypeCode.Byte:
                    schemaType = "string";
                    format = "byte";
                    break;

                case TypeCode.Boolean:
                    schemaType = "boolean";
                    format = null;
                    break;

                case TypeCode.DateTime:
                    schemaType = "string";
                    format = "date-time";
                    break;

                default:
                    schemaType = "string";
                    format = null;
                    break;
            }

            if (isCollection)
            {
                schema = new SchemaRef() { Item = new Item() { Type = schemaType, Format = format }, Type = "array" };
            }
            else
            {
                schema = new SchemaRef() { Type = schemaType, Format = format };
            }

            return schema;
        }
    }
}
