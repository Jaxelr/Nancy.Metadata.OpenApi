using System;
using System.Collections.Generic;
using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Model;

namespace Nancy.Metadata.OpenApi.Fluent
{
    /// <summary>
    /// Endpoint info extensions for use with the metadata nancy modules
    /// </summary>
    public static class EndpointInfoExtensions
    {
        /// <summary>
        /// Adds a response model to the endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="statusCode">Http status code of the response</param>
        /// <param name="responseType">Poco that describes the response model</param>
        /// <param name="description">A description of the response model</param>
        /// <returns></returns>
        [Obsolete("This operation will be removed on future versions, favoring the HttpStatusCode enumeration")]
        public static Endpoint WithResponseModel(this Endpoint endpointInfo, string statusCode, Type responseType, string description = null)
        {
            if (endpointInfo.ResponseInfos is null)
            {
                endpointInfo.ResponseInfos = new Dictionary<string, Model.Response>();
            }

            endpointInfo.ResponseInfos[statusCode] = GenerateResponseInfo(description, responseType);

            return endpointInfo;
        }

        /// <summary>
        /// Adds a response model to the endpoint
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="statusCode">Http status code of the response</param>
        /// <param name="responseType">Poco that describes the response model</param>
        /// <param name="description">A description of the response model</param>
        /// <returns></returns>
        public static Endpoint WithResponseModel(this Endpoint endpointInfo, HttpStatusCode statusCode, Type responseType, string description = null)
        {
            if (endpointInfo.ResponseInfos is null)
            {
                endpointInfo.ResponseInfos = new Dictionary<string, Model.Response>();
            }

            string code = statusCode.GetHashCode()
                                    .ToString();

            endpointInfo.ResponseInfos[code] = GenerateResponseInfo(description, responseType);

            return endpointInfo;
        }

        /// <summary>
        /// Adds a default response model with status code 200
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="responseType">Poco that describes the response model</param>
        /// <param name="description">A description of the default response model</param>
        /// <returns></returns>
        public static Endpoint WithDefaultResponse(this Endpoint endpointInfo, Type responseType, string description = "Default response")
            => endpointInfo.WithResponseModel(HttpStatusCode.OK, responseType, description);

        /// <summary>
        /// Adds a response without a model, for usage with status code and description
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="statusCode">Http status code of the response</param>
        /// <param name="description">A description of the default response</param>
        /// <returns></returns>
        [Obsolete("This operation will be removed on future versions, favoring the HttpStatusCode enumeration")]
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
        /// Adds a response without a model, for usage with status code and description
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="statusCode">Http status code of the response</param>
        /// <param name="description">A description of the response</param>
        /// <returns></returns>
        public static Endpoint WithResponse(this Endpoint endpointInfo, HttpStatusCode statusCode, string description)
        {
            if (endpointInfo.ResponseInfos is null)
            {
                endpointInfo.ResponseInfos = new Dictionary<string, Model.Response>();
            }

            string code = statusCode.GetHashCode()
                        .ToString();

            endpointInfo.ResponseInfos[code] = GenerateResponseInfo(description);

            return endpointInfo;
        }

        /// <summary>
        /// Ads a request parameter for usage with querystring, header, path or cookie parameters
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="name">The name of the parameter</param>
        /// <param name="type">Poco that describes the request parameter</param>
        /// <param name="required">A flag to indicate if the parameter is required</param>
        /// <param name="description">A description of the request parameter</param>
        /// <param name="loc">Plausible values are query, header, path or cookie</param>
        /// <param name="deprecated">A flag to indicate if this parameter is deprecated</param>
        /// <returns></returns>
        public static Endpoint WithRequestParameter(this Endpoint endpointInfo, string name, Type type = null,
        bool required = true, string description = null,Loc loc = Loc.Path, bool deprecated = false)
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
            string locText = LocGenerator.GetLocByEnum(loc);

            endpointInfo.RequestParameters.Add(new RequestParameter
            {
                Required = required,
                Description = description,
                In = locText,
                Name = name,
                Deprecated = deprecated,
                Schema = schema
            });

            return endpointInfo;
        }


        /// <summary>
        /// Ads a request model to the endpoint operation
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="requestType">Poco that describes the request model</param>
        /// <param name="contentType">The http content type of the response</param>
        /// <param name="description">A description of the request model</param>
        /// <param name="required">A flag to indicate if the parameter is required</param>
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
                Content = new Dictionary<string, MediaTypeObject>
                {
                    {
                        contentType, new MediaTypeObject() { Schema = new SchemaRef() { Ref = Ref } }
                    }
                }
            };

            return endpointInfo;
        }

        /// <summary>
        /// Add a description to the endpoint operation
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="description">A description of the attached endpoint</param>
        /// <param name="tags">Tags defined to the endpoint</param>
        /// <returns></returns>
        public static Endpoint WithDescription(this Endpoint endpointInfo, string description, params string[] tags)
        {
            if (endpointInfo.Tags is null)
            {
                if (tags.Length > 0)
                {
                    endpointInfo.Tags = tags;
                }
                else
                {
                    endpointInfo.Tags = null;
                }
            }

            endpointInfo.Description = description;

            return endpointInfo;
        }

        /// <summary>
        /// Add a summary description to the endpoint operation
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="summary">A summary definition of the attached endpoint</param>
        /// <returns></returns>
        public static Endpoint WithSummary(this Endpoint endpointInfo, string summary)
        {
            endpointInfo.Summary = summary;
            return endpointInfo;
        }

        /// <summary>
        /// Add an external documentation object to the endpoint operation
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <param name="url">A url of the external documentation</param>
        /// <param name="description">A description of the external documentation</param>
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
        /// Create an optional deprecation flag for the endpoints
        /// </summary>
        /// <param name="endpointInfo"></param>
        /// <returns></returns>
        public static Endpoint IsDeprecated(this Endpoint endpointInfo)
        {
            endpointInfo.IsDeprecated = true;

            return endpointInfo;
        }

        /// <summary>
        /// Generate a response as based on the poco and description included
        /// </summary>
        /// <param name="description">A description of the response model</param>
        /// <param name="responseType">Poco that describes the response model</param>
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
