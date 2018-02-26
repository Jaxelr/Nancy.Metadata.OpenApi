using Nancy.Metadata.OpenApi.Model;
using System;
using System.Collections.Generic;
using NJsonSchema;
using Nancy.Metadata.OpenApi.Core;

namespace Nancy.Metadata.OpenApi.Fluent
{
    public static class EndpointInfoExtensions
    {
        public static Endpoint WithResponseModel(this Endpoint endpointInfo, string statusCode, Type modelType, string description = null)
        {
            if (endpointInfo.ResponseInfos == null)
            {
                endpointInfo.ResponseInfos = new Dictionary<string, Model.Response>();
            }

            endpointInfo.ResponseInfos[statusCode] = GenerateResponseInfo(description, modelType);

            return endpointInfo;
        }

        public static Endpoint WithDefaultResponse(this Endpoint endpointInfo, Type responseType, string description = "Default response")
            => endpointInfo.WithResponseModel("200", responseType, description);

        public static Endpoint WithResponse(this Endpoint endpointInfo, string statusCode, string description)
        {
            if (endpointInfo.ResponseInfos == null)
            {
                endpointInfo.ResponseInfos = new Dictionary<string, Model.Response>();
            }

            endpointInfo.ResponseInfos[statusCode] = GenerateResponseInfo(description);

            return endpointInfo;
        }

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

        public static Endpoint WithSummary(this Endpoint endpointInfo, string summary)
        {
            endpointInfo.Summary = summary;
            return endpointInfo;
        }

        public static Endpoint WithExternalDocumentation(this Endpoint endpointInfo, string url, string description)
        {
            endpointInfo.ExternalDocs = new ExternalDocumentation()
            {
                Url = url,
                Description = description
            };

            return endpointInfo;
        }

        public static Endpoint WithDeprecatedFlag(this Endpoint endpointInfo)
        {
            endpointInfo.IsDeprecated = true;

            return endpointInfo;
        }

        private static Model.Response GenerateResponseInfo(string description, Type responseType)
           => new Model.Response
           {
               Schema = new SchemaRef
               {
                   Ref = $"#/components/schemas/{GetOrSaveSchemaReference(responseType)}"
               },
               Description = description
           };

        private static Model.Response GenerateResponseInfo(string description)
            => new Model.Response
            {
                Description = description
            };

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
