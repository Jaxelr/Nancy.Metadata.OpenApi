using Nancy.Metadata.OpenApi.Fluent;
using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Tests.Fakes;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.UnitTests
{
    public class EndpointInfoTests
    {
        [Fact]
        public void Endpoint_with_summary()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName).WithSummary(fakeEndpoint.Summary);

            //Assert
            Assert.Equal(fakeEndpoint.Summary, endpoint.Summary);
        }

        [Fact]
        public void Endpoint_with_description()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName).WithDescription(fakeEndpoint.Description);

            //Assert
            Assert.Equal(fakeEndpoint.Description, endpoint.Description);
        }


        [Fact]
        public void Endpoint_with_external_documentation()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
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
            var endpoint = new Endpoint(fakeEndpoint.OperationName).IsDeprecated();

            //Assert
            Assert.True(endpoint.IsDeprecated);
        }

        [Fact]
        public void Endpoint_with_description_and_tags()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName).WithDescription(fakeEndpoint.Description, fakeEndpoint.Tags);

            //Assert
            Assert.Equal(fakeEndpoint.Description, endpoint.Description);
            Assert.Equal(fakeEndpoint.Tags, endpoint.Tags);
        }

        [Fact]
        public void Endpoint_with_request_parameter()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestParameter(
                fakeRequest.Name,
                fakeRequest.Type,
                fakeRequest.Format,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated,
                fakeRequest.IsArray);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Null(endpoint.RequestParameters[0].Schema.Format);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
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
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestParameter(
                fakeRequest.Name,
                fakeRequest.Type,
                fakeRequest.Format,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated,
                fakeRequest.IsArray); //Consider this request an array of strings

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Null(endpoint.RequestParameters[0].Schema.Item.Format);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
            Assert.Equal(ARRAY, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(fakeRequest.Type.Name.ToLowerInvariant(), endpoint.RequestParameters[0].Schema.Item.Type);
        }

        [Fact]
        public void Endpoint_with_request_model()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequestModel();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestModel(
                    typeof(FakeRequestModel),
                    fakeRequest.ContentType,
                    fakeRequest.Description,
                    fakeRequest.Required);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestBody.Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestBody.Required);
            Assert.True(endpoint.RequestBody.Content.ContainsKey(fakeRequest.ContentType));
            Assert.Contains(nameof(FakeRequestModel), endpoint.RequestBody.Content[fakeRequest.ContentType].Ref);
        }

        [Fact]
        public void Endpoint_with_response_model()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeResponseModel = new FakeResponseModel();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithResponseModel(fakeResponseModel.StatusCode, typeof(FakeResponseModel), fakeResponseModel.Description);

            //Assert
            Assert.Equal(fakeEndpoint.OperationName, endpoint.OperationId);
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
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithDefaultResponse(typeof(FakeResponseModel), fakeResponseModel.Description);

            //Assert
            Assert.Equal(fakeEndpoint.OperationName, endpoint.OperationId);
            Assert.NotNull(endpoint.ResponseInfos[fakeResponseModel.StatusCode]);
            Assert.Equal(fakeResponseModel.Description, endpoint.ResponseInfos[fakeResponseModel.StatusCode].Description);
            Assert.Contains(nameof(FakeResponseModel), endpoint.ResponseInfos[fakeResponseModel.StatusCode].Schema.Ref);
        }

        [Fact]
        public void Endpoint_with_response()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeResponse = new FakeResponse();

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithResponse(fakeResponse.StatusCode, fakeResponse.Description);

            //Assert
            Assert.Equal(fakeEndpoint.OperationName, endpoint.OperationId);
            Assert.NotNull(endpoint.ResponseInfos[fakeResponse.StatusCode]);
            Assert.Equal(fakeResponse.Description, endpoint.ResponseInfos[fakeResponse.StatusCode].Description);
        }
    }
}
