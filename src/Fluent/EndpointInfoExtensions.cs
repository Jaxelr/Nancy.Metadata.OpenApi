using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Model;
using System;
using System.Collections.Generic;

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
            if (endpointInfo.ResponseInfos is null)
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
            if (endpointInfo.ResponseInfos is null)
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
            if (endpointInfo.RequestParameters is null)
            {
                endpointInfo.RequestParameters = new List<RequestParameter>();
            }

            if (type is null)
            {
                type = typeof(string);
            }

            var schema = SchemaGenerator.GetSchemaByType(type);

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
        /// <param name="contentType"></param>
        /// <param name="description"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        public static Endpoint WithRequestModel(this Endpoint endpointInfo, Type requestType, string contentType = null, string description = null, bool required = true)
        {
            if (contentType is null)
            {
                contentType = @"application/json";
            }

            string Ref = $"#/components/schemas/{SchemaGenerator.GetOrSaveSchemaReference(requestType)}";

            endpointInfo.RequestBody = new RequestBody
            {
                Required = required,
                Description = description,
                Content = new Dictionary<string, MediaTypeObject> { { contentType, new MediaTypeObject() { Schema = new SchemaRef() { Ref = Ref } } }

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
            if (endpointInfo.Tags is null)
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
                        Ref = $"#/components/schemas/{SchemaGenerator.GetOrSaveSchemaReference(responseType)}"
                    },
                    Description = description
                };
            }
            else
            {
                return new Model.Response { Description = description };
            }
        }
    }
}
