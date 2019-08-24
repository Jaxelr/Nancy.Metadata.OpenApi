﻿using System;
using System.Collections;
using Nancy.Metadata.OpenApi.Model;
using NJsonSchema;

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

            var Schema = JsonSchema.FromType(type, new NJsonSchema.Generation.JsonSchemaGeneratorSettings
            {
                SchemaType = SchemaType.OpenApi3,
                TypeNameGenerator = new TypeNameGenerator()
            });

            SchemaCache.ComponentCache[key] = Schema;

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
            bool isCollection = false;

            if (type.IsArray)
            {
                type = type.GetElementType();
                isCollection = true;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type) && (type != typeof(string)))
            {
                type = type.GetGenericArguments()[0];
                isCollection = true;
            }

            Schema schema;
            string schemaType, format;

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
                    Type = "array"
                };
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
    }
}
