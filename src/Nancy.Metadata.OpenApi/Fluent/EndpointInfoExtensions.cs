using Nancy.Metadata.OpenApi.Model;
using System;
using System.Collections.Generic;
using NJsonSchema;
using Nancy.Metadata.OpenApi.Core;

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
        /// <returns></returns>
        public static Endpoint WithRequestParameter(this Endpoint endpointInfo, string name,
            string type = "string", string format = null, bool required = true, string description = null,
            string loc = "path")
        {
            if (endpointInfo.RequestParameters == null)
            {
                endpointInfo.RequestParameters = new List<RequestParameter>();
            }

            endpointInfo.RequestParameters.Add(new RequestParameter
            {
                Required = required,
                Description = description,
                Format = format,
                In = loc,
                Name = name,
                Type = type
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
        public static Endpoint WithRequestModel(this Endpoint endpointInfo, Type requestType, string name = "body", string description = null, bool required = true, string loc = "body")
        {
            if (endpointInfo.RequestParameters == null)
            {
                endpointInfo.RequestParameters = new List<RequestParameter>();
            }

            endpointInfo.RequestParameters.Add(new RequestParameter
            {
                Required = required,
                Description = description,
                In = loc,
                Name = name,
                Schema = new SchemaRef
                {
                    Ref = $"#/components/schemas/{GetOrSaveSchemaReference(requestType)}"
                }
            });

            return endpointInfo;
        }

        /// <summary>
        /// Add a description to the endpoint operation.
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="description"></param>
        /// <param name="contentType"></param>
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
        /// Create new Response model with Schema Ref property and Description.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="responseType"></param>
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
        public static Endpoint WithDeprecatedFlag(this Endpoint endpointInfo)
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
        private static Model.Response GenerateResponseInfo(string description, Type responseType)
           => new Model.Response
           {
               Schema = new SchemaRef
               {
                   Ref = $"#/components/schemas/{GetOrSaveSchemaReference(responseType)}"
               },
               Description = description
           };

        /// <summary>
        /// Generate new Response model with Description only.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private static Model.Response GenerateResponseInfo(string description)
            => new Model.Response
            {
                Description = description
            };

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
                NullHandling = NullHandling.Swagger,
                TypeNameGenerator = new TypeNameGenerator(),
                SchemaNameGenerator = new TypeNameGenerator()
            });

            SchemaCache.Cache[key] = taskSchema.Result;

            return key;
        }
    }
}
