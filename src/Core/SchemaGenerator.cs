using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nancy.Metadata.OpenApi.Model;

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
            string key = type.Name;

            if (SchemaCache.ComponentCache.ContainsKey(key))
            {
                return key;
            }

            var schema = GetSchemaByType(type);

            schema.Properties = GetPropertiesByType(type);

            SchemaCache.ComponentCache[key] = schema;

            return key;
        }

        /// <summary>
        /// Saves the Security Schemes on the Cache Dictionary also returns the key used.
        /// </summary>
        /// <param name="securityScheme"></param>
        /// <returns></returns>
        internal static string GetOrSaveSecurity(SecurityScheme securityScheme)
        {
            string key = securityScheme.Name;

            if (SchemaCache.SecurityCache.ContainsKey(key))
            {
                return key;
            }

            SchemaCache.SecurityCache[key] = securityScheme;

            return key;
        }

        /// <summary>
        /// Matches the type, format and item (if array) to the schema specified on the parameter.
        /// </summary>
        /// <param name="type">The type defined for the parameter, check: https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#data-types for guidelines</param>
        /// <returns></returns>
        internal static Schema GetSchemaByType(Type type)
        {
            Schema schema;
            string schemaType, format;

            bool isCollection;
            (isCollection, type) = GetElementCollection(type);

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
                    schemaType = "object";
                    format = null;
                    break;
            }

            if (isCollection)
            {
                schema = new Schema()
                {
                    Items = new Schema()
                    {
                        Type = schemaType,
                        Format = format
                    },
                    Type = "array",
                };

                if (schemaType == "object")
                {
                    schema.Items.Ref = $"#/components/schemas/{GetOrSaveSchemaReference(type)}";
                }
            }
            else
            {
                schema = new Schema()
                {
                    Type = schemaType,
                    Format = format
                };
            }

            return schema;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static Dictionary<string, Schema> GetPropertiesByType(Type type)
        {
            var result = new Dictionary<string, Schema>();

            var props = type.GetProperties().Select(x => new
            {
                Name = x.Name.ToLower(),
                Type = x.PropertyType
            });

            foreach (var prop in props)
            {
                result.Add(prop.Name, GetSchemaByType(prop.Type));
            }

            return result;
        }

        internal static (bool, Type) GetElementCollection(Type type)
        {
            bool collection = false;

            if (type.IsArray)
            {
                type = type.GetElementType();
                collection = true;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type) && (type != typeof(string)))
            {
                type = type.GetGenericArguments()[0];
                collection = true;
            }

            return (collection, type);
        }
    }
}
