using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Model;
using NJsonSchema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Nancy.Metadata.OpenApi.Fluent
{
    /// <summary>
    /// Endpoint Info Extensions for use with the nancy modules.
    /// </summary>
    public static class EndpointInfoExtensions
    {
        /// <summary>
        /// Adds a Response Model to the endpoint.
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="statusCode"></param>
        /// <param name="modelType"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Endpoint WithResponseModel(this Endpoint endpointInfo, string statusCode, Type modelType, string description = null)
        {
            if (endpointInfo.ResponseInfos == null)
            {
                endpointInfo.ResponseInfos = new Dictionary<string, Model.Response>();
            }

            endpointInfo.ResponseInfos[statusCode] = GenerateResponseInfo(description, modelType);

            return endpointInfo;
        }

        /// <summary>
        /// Adds a Default Response Model with status code 200.
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="responseType"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Endpoint WithDefaultResponse(this Endpoint endpointInfo, Type responseType, string description = "Default response")
            => endpointInfo.WithResponseModel("200", responseType, description);

        /// <summary>
        /// Adds a Response without a model, for usage with status code and description.
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="statusCode"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Endpoint WithResponse(this Endpoint endpointInfo, string statusCode, string description)
        {
            if (endpointInfo.ResponseInfos == null)
            {
                endpointInfo.ResponseInfos = new Dictionary<string, Model.Response>();
            }

            endpointInfo.ResponseInfos[statusCode] = GenerateResponseInfo(description);

            return endpointInfo;
        }

        /// <summary>
        /// Ads a request parameters to the endpoint operation.
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="format"></param>
        /// <param name="required"></param>
        /// <param name="description"></param>
        /// <param name="loc"></param>
        /// <param name="deprecated"></param>
        /// <returns></returns>
        public static Endpoint WithRequestParameter(this Endpoint endpointInfo, string name, Type type = null,
            string format = null, bool required = true, string description = null,
            string loc = "path", bool deprecated = false)
        {
            if (endpointInfo.RequestParameters == null)
            {
                endpointInfo.RequestParameters = new List<RequestParameter>();
            }
            
            if (type is null)
            {
                type = typeof(string);
            }

            var schema = GetSchemaByType(type);

            endpointInfo.RequestParameters.Add(new RequestParameter
            {
                Required = required,
                Description = description,
                In = loc,
                Name = name,
                Deprecated = deprecated,
                Schema = schema
            });

            return endpointInfo;
        }

        /// <summary>
        /// Ads a request model to the endpoint operation.
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="requestType"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="required"></param>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static Endpoint WithRequestModel(this Endpoint endpointInfo, Type requestType, string contentType = null, string description = null, bool required = true)
        {
            if (contentType is null)
            {
                contentType = @"application/json";
            }

            endpointInfo.RequestBody = new RequestBody
            {
                Required = required,
                Description = description,
                Content = new Dictionary<string, MediaTypeObject> {
                    {contentType, new MediaTypeObject() { Schema = new SchemaRef() { Ref = $"#/components/schemas/{GetOrSaveSchemaReference(requestType)}" } } }
                }
            };

            return endpointInfo;
        }

        /// <summary>
        /// Add a description to the endpoint operation.
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="description"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static Endpoint WithDescription(this Endpoint endpointInfo, string description, params string[] tags)
        {
            if (endpointInfo.Tags == null)
            {
                if (tags.Length > 0)
                {
                    endpointInfo.Tags = tags;
                }
            }

            endpointInfo.Description = description;

            return endpointInfo;
        }

        /// <summary>
        /// Add a summary description to the endpoint operation.
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="summary"></param>
        /// <returns></returns>
        public static Endpoint WithSummary(this Endpoint endpointInfo, string summary)
        {
            endpointInfo.Summary = summary;
            return endpointInfo;
        }

        /// <summary>
        /// Add an external documentation object to the endpoint operation.
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="url"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Endpoint WithExternalDocumentation(this Endpoint endpointInfo, string url, string description)
        {
            endpointInfo.ExternalDocs = new ExternalDocumentation()
            {
                Url = url,
                Description = description
            };

            return endpointInfo;
        }

        /// <summary>
        /// Create an optional deprecation flag for the endpoints.
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <returns></returns>
        public static Endpoint IsDeprecated(this Endpoint endpointInfo)
        {
            endpointInfo.IsDeprecated = true;

            return endpointInfo;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="description"></param>
        /// <param name="responseType"></param>
        /// <returns></returns>
        private static Model.Response GenerateResponseInfo(string description, Type responseType = null)
        {
            if (responseType is Type)
            { 
                return new Model.Response
                {
                    Schema = new SchemaRef
                    {
                        Ref = $"#/components/schemas/{GetOrSaveSchemaReference(responseType)}"
                    },
                    Description = description
                };
            }
            else
            {
                return new Model.Response { Description = description };
            }
        }

        /// <summary>
        /// Look up schema on schema cache, if not present add a new key.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetOrSaveSchemaReference(Type type)
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
        private static SchemaRef GetSchemaByType(Type type)
        {
            bool isArray = false;

            if (type.IsArray)
            {
                type = type.GetElementType();
                isArray = true;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type) && (type != typeof(string)))
            {
#if NET45

                type = type.GetType().GetGenericArguments()[0];
#elif NETSTANDARD1_6
                type = type.GetTypeInfo().GetGenericArguments()[0];
#endif
                isArray = true;
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

            if (isArray)
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
