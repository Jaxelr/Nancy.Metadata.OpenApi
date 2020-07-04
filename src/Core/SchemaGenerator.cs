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
        /// Look up schema class on the schema cache, if its missing,
        /// process the type and add it to cache.
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

            SchemaCache.ComponentCache[key] = schema;

            schema.Properties = GetPropertiesByType(type);

            return key;
        }

        /// <summary>
        /// Saves the securitysSchemes on the cache dictionary also returns the key used.
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
        /// This method returns the public properties by the poco type
        /// </summary>
        /// <param name="type"></param>
        /// <returns><A Dictionary with the poco name as key and the Schema Open Api object as the value/returns>
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
                var schema = GetSchemaByType(prop.Type);

                //If the prop its a poco and it doesnt exist on the dictionary, we must add it.
                if (schema.Type == "object")
                {
                    schema.Ref = $"#/components/schemas/{GetOrSaveSchemaReference(prop.Type)}";
                }
                result.Add(prop.Name, schema);
            }

            return result;
        }

        /// <summary>
        /// This method validates if the type is a collection and
        /// returns the type of the element inside the collection
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
