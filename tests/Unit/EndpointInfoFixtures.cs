using System;
using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Fluent;
using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Tests.Fakes;
using Nancy.Routing;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.Unit
{
    public class EndpointInfoFixtures
    {
        [Fact]
        public void Generate_openapi_metadata()
        {
            //Arrange
            var endpoint = new FakeEndpoint();
            var fakeRouteDescription = new RouteDescription(endpoint.Operation, endpoint.Method, endpoint.Path, ctx => true, null);

            //Act
            var metadata = new OpenApiRouteMetadata(fakeRouteDescription);

            //Assert
            Assert.Equal(metadata.Path, fakeRouteDescription.Path);
            Assert.Equal(metadata.Method, fakeRouteDescription.Method.ToLowerInvariant());
            Assert.Equal(metadata.Name, fakeRouteDescription.Name);
        }

        [Obsolete]
        [Fact]
        public void Generate_method_description()
        {
            //Arrange
            var endpoint = new FakeEndpoint();

            var fakeRouteDescription = new RouteDescription(endpoint.Operation, endpoint.Method, endpoint.Path, ctx => true, null);

            //Act
            var metadata = new OpenApiRouteMetadata(fakeRouteDescription)
                .With(i => i.WithResponseModel("200", typeof(FakeResponseModel), "Sample response")
                            .WithSummary(endpoint.Summary));

            //Assert
            Assert.Equal(metadata.Path, fakeRouteDescription.Path);
            Assert.Equal(metadata.Method, fakeRouteDescription.Method.ToLowerInvariant());
            Assert.Equal(metadata.Name, fakeRouteDescription.Name);
            Assert.Equal(metadata.Info.OperationId, endpoint.Operation);
            Assert.Equal(metadata.Info.Summary, endpoint.Summary);
        }

        [Fact]
        public void Endpoint_with_summary()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation).WithSummary(fakeEndpoint.Summary);

            //Assert
            Assert.Equal(fakeEndpoint.Summary, endpoint.Summary);
        }

        [Fact]
        public void Endpoint_with_description()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation).WithDescription(fakeEndpoint.Description);

            //Assert
            Assert.Equal(fakeEndpoint.Description, endpoint.Description);
        }

        [Fact]
        public void Endpoint_with_description_and_tags()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation).WithDescription(fakeEndpoint.Description, fakeEndpoint.Tags);

            //Assert
            Assert.Equal(fakeEndpoint.Description, endpoint.Description);
            Assert.Equal(fakeEndpoint.Tags, endpoint.Tags);
        }

        [Fact]
        public void Endpoint_with_description_and_existing_tag_exists()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            string[] sampleTag = new string[] { "Sample 1" };

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
            {
                Tags = sampleTag
            };

            endpoint.WithDescription(fakeEndpoint.Description, fakeEndpoint.Tags);

            //Assert
            Assert.Equal(fakeEndpoint.Description, endpoint.Description);
            Assert.Equal(sampleTag, endpoint.Tags);
            Assert.NotEqual(fakeEndpoint.Tags, endpoint.Tags);
        }

        [Fact]
        public void Endpoint_with_external_documentation()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithExternalDocumentation(fakeEndpoint.ExternalDocsUrl, fakeEndpoint.ExternalDocs);

            //Assert
            Assert.Equal(fakeEndpoint.ExternalDocs, endpoint.ExternalDocs.Description);
            Assert.Equal(fakeEndpoint.ExternalDocsUrl, endpoint.ExternalDocs.Url);
        }

        [Fact]
        public void Endpoint_with_is_deprecated_flag()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation).IsDeprecated();

            //Assert
            Assert.True(endpoint.IsDeprecated);
        }

        [Fact]
        public void Endpoint_with_request_parameter()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                fakeRequest.Type,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Null(endpoint.RequestParameters[0].Schema.Format);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(fakeRequest.Type.Name.ToLowerInvariant(), endpoint.RequestParameters[0].Schema.Type);
        }

        [Fact]
        public void Endpoint_with_multiple_request_parameters()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            string Param2 = "Param2";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter
                (
                    fakeRequest.Name,
                    fakeRequest.Type,
                    fakeRequest.Required,
                    fakeRequest.Description,
                    fakeRequest.Loc,
                    fakeRequest.Deprecated
                )
                .WithRequestParameter
                (
                    Param2,
                    fakeRequest.Type,
                    fakeRequest.Required,
                    fakeRequest.Description,
                    fakeRequest.Loc,
                    fakeRequest.Deprecated
                );

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Null(endpoint.RequestParameters[0].Schema.Format);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(fakeRequest.Type.Name.ToLowerInvariant(), endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[1].Description);
            Assert.Null(endpoint.RequestParameters[1].Schema.Format);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[1].Required);
            Assert.Equal(Param2, endpoint.RequestParameters[1].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[1].In);
            Assert.Equal(fakeRequest.Type.Name.ToLowerInvariant(), endpoint.RequestParameters[1].Schema.Type);
        }

        [Fact]
        public void Endpoint_with_request_parameter_null_type()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                null,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Null(endpoint.RequestParameters[0].Schema.Format);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(fakeRequest.Type.Name.ToLowerInvariant(), endpoint.RequestParameters[0].Schema.Type);
        }

        [Fact]
        public void Endpoint_with_request_parameter_array()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequestArray();
            const string ARRAY = "array";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                fakeRequest.Type,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Null(endpoint.RequestParameters[0].Schema.Items.Format);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(ARRAY, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(fakeRequest.Type.GetElementType().Name.ToLowerInvariant(), endpoint.RequestParameters[0].Schema.Items.Type);
        }

        [Fact]
        public void Endpoint_with_request_parameter_collection()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequestCollection();
            const string ARRAY = "array";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                fakeRequest.Type,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Null(endpoint.RequestParameters[0].Schema.Items.Format);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(ARRAY, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(fakeRequest.Type.GenericTypeArguments[0].Name.ToLowerInvariant(), endpoint.RequestParameters[0].Schema.Items.Type);
        }

        [Fact]
        public void Endpoint_with_request_model()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequestModel();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestModel(
                    typeof(FakeRequestModel),
                    fakeRequest.ContentType,
                    fakeRequest.Description,
                    fakeRequest.Required);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestBody.Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestBody.Required);
            Assert.True(endpoint.RequestBody.Content.ContainsKey(fakeRequest.ContentType));
            Assert.Contains(nameof(FakeRequestModel), endpoint.RequestBody.Content[fakeRequest.ContentType].Schema.Ref);
        }

        [Fact]
        public void Endpoint_with_request_model_empty_content_type()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequestModel();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestModel(
                    typeof(FakeRequestModel),
                    null,
                    fakeRequest.Description,
                    fakeRequest.Required);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestBody.Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestBody.Required);
            Assert.True(endpoint.RequestBody.Content.ContainsKey(fakeRequest.ContentType));
            Assert.Contains(nameof(FakeRequestModel), endpoint.RequestBody.Content[fakeRequest.ContentType].Schema.Ref);
        }

        [Obsolete]
        [Fact]
        public void Endpoint_with_response_model()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeResponseModel = new FakeResponseModel();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithResponseModel(fakeResponseModel.StatusCode, typeof(FakeResponseModel), fakeResponseModel.Description);

            //Assert
            Assert.Equal(fakeEndpoint.Operation, endpoint.OperationId);
            Assert.NotNull(endpoint.ResponseInfos[fakeResponseModel.StatusCode]);
            Assert.Equal(fakeResponseModel.Description, endpoint.ResponseInfos[fakeResponseModel.StatusCode].Description);
            Assert.Contains(nameof(FakeResponseModel), endpoint.ResponseInfos[fakeResponseModel.StatusCode].Schema.Ref);
        }

        [Fact]
        public void Endpoint_with_default_response_model()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeResponseModel = new FakeResponseModel();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithDefaultResponse(typeof(FakeResponseModel), fakeResponseModel.Description);

            //Assert
            Assert.Equal(fakeEndpoint.Operation, endpoint.OperationId);
            Assert.NotNull(endpoint.ResponseInfos[fakeResponseModel.StatusCode]);
            Assert.Equal(fakeResponseModel.Description, endpoint.ResponseInfos[fakeResponseModel.StatusCode].Description);
            Assert.Contains(nameof(FakeResponseModel), endpoint.ResponseInfos[fakeResponseModel.StatusCode].Schema.Ref);
        }

        [Obsolete]
        [Fact]
        public void Endpoint_with_multiple_response_models()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeResponseModel = new FakeResponseModel();
            string NewResource = "201";
            string NewResourceDescription = "New Resource Created";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithResponseModel(fakeResponseModel.StatusCode, typeof(FakeResponseModel), fakeResponseModel.Description)
                .WithResponseModel(NewResource, typeof(FakeResponseModel), NewResourceDescription);

            //Assert
            Assert.Equal(fakeEndpoint.Operation, endpoint.OperationId);
            Assert.NotNull(endpoint.ResponseInfos[fakeResponseModel.StatusCode]);
            Assert.NotNull(endpoint.ResponseInfos[NewResource]);
            Assert.Equal(fakeResponseModel.Description, endpoint.ResponseInfos[fakeResponseModel.StatusCode].Description);
            Assert.Equal(NewResourceDescription, endpoint.ResponseInfos[NewResource].Description);
            Assert.Contains(nameof(FakeResponseModel), endpoint.ResponseInfos[fakeResponseModel.StatusCode].Schema.Ref);
        }

        [Fact]
        public void Endpoint_with_multiple_response_models_http_status_codes()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeResponseModel = new FakeResponseModel();
            HttpStatusCode NewResource = HttpStatusCode.Created;
            string NewResourceDescription = "New Resource Created";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithResponseModel(fakeResponseModel.HttpStatusCode, typeof(FakeResponseModel), fakeResponseModel.Description)
                .WithResponseModel(NewResource, typeof(FakeResponseModel), NewResourceDescription);

            //Assert
            Assert.Equal(fakeEndpoint.Operation, endpoint.OperationId);
            Assert.NotNull(endpoint.ResponseInfos[fakeResponseModel.StatusCode]);
            Assert.NotNull(endpoint.ResponseInfos[NewResource.GetHashCode().ToString()]);
            Assert.Equal(fakeResponseModel.Description, endpoint.ResponseInfos[fakeResponseModel.StatusCode].Description);
            Assert.Equal(NewResourceDescription, endpoint.ResponseInfos[NewResource.GetHashCode().ToString()].Description);
            Assert.Contains(nameof(FakeResponseModel), endpoint.ResponseInfos[fakeResponseModel.StatusCode].Schema.Ref);
        }

        [Obsolete]
        [Fact]
        public void Endpoint_with_response()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeResponse = new FakeResponse();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithResponse(fakeResponse.StatusCode, fakeResponse.Description);

            //Assert
            Assert.Equal(fakeEndpoint.Operation, endpoint.OperationId);
            Assert.NotNull(endpoint.ResponseInfos[fakeResponse.StatusCode]);
            Assert.Equal(fakeResponse.Description, endpoint.ResponseInfos[fakeResponse.StatusCode].Description);
        }

        [Obsolete]
        [Fact]
        public void Endpoint_with_multiple_response()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeResponse = new FakeResponse();
            string badRequest = "400";
            string badRequestDesc = "Bad Request Made";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithResponse(fakeResponse.StatusCode, fakeResponse.Description)
                .WithResponse(badRequest, badRequestDesc);

            //Assert
            Assert.Equal(fakeEndpoint.Operation, endpoint.OperationId);
            Assert.NotNull(endpoint.ResponseInfos[fakeResponse.StatusCode]);
            Assert.Equal(fakeResponse.Description, endpoint.ResponseInfos[fakeResponse.StatusCode].Description);
            Assert.NotNull(endpoint.ResponseInfos[badRequest]);
            Assert.Equal(badRequestDesc, endpoint.ResponseInfos[badRequest].Description);
        }

        [Fact]
        public void Endpoint_with_response_http_status_code()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeResponse = new FakeResponse();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithResponse(fakeResponse.HttpStatusCode, fakeResponse.Description);

            //Assert
            Assert.Equal(fakeEndpoint.Operation, endpoint.OperationId);
            Assert.NotNull(endpoint.ResponseInfos[fakeResponse.StatusCode]);
            Assert.Equal(fakeResponse.Description, endpoint.ResponseInfos[fakeResponse.StatusCode].Description);
        }

        [Fact]
        public void Endpoint_with_multiple_response_http_status_code()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeResponse = new FakeResponse();
            var badRequest = HttpStatusCode.BadRequest;
            string badRequestDesc = "Bad Request Made";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithResponse(fakeResponse.HttpStatusCode, fakeResponse.Description)
                .WithResponse(badRequest, badRequestDesc);

            //Assert
            Assert.Equal(fakeEndpoint.Operation, endpoint.OperationId);
            Assert.NotNull(endpoint.ResponseInfos[fakeResponse.StatusCode]);
            Assert.Equal(fakeResponse.Description, endpoint.ResponseInfos[fakeResponse.StatusCode].Description);
            Assert.NotNull(endpoint.ResponseInfos[badRequest.GetHashCode().ToString()]);
            Assert.Equal(badRequestDesc, endpoint.ResponseInfos[badRequest.GetHashCode().ToString()].Description);
        }
    }
}
