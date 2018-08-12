using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Tests.Fakes;
using Nancy.Metadata.OpenApi.Fluent;
using Nancy.Routing;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.UnitTests
{

    public class MetadataTests
    {
        [Fact]
        public void Generate_openapi_metadata()
        {
            //Arrange
            var endpoint = new FakeEndpoint();
            var fakeRouteDescription = new RouteDescription(endpoint.Operation, endpoint.Method, endpoint.Path, null);

            //Act
            var metadata = new OpenApiRouteMetadata(fakeRouteDescription);

            //Assert
            Assert.Equal(metadata.Path, fakeRouteDescription.Path);
            Assert.Equal(metadata.Method, fakeRouteDescription.Method.ToLowerInvariant());
            Assert.Equal(metadata.Name, fakeRouteDescription.Name);
        }


        //[Fact]
        //public void Generate_method_description()
        //{
        //    //Arrange
        //    var endpoint = new FakeEndpoint();

        //    var fakeRouteDescription = new RouteDescription(endpoint.Operation, endpoint.Method, endpoint.Path, null);

        //    //Act
        //    var metadata = new OpenApiRouteMetadata(fakeRouteDescription)
        //        .With(i => i.WithResponseModel("200", typeof(FakeResponseModel), "Sample response")
        //                    .WithSummary(endpoint.Summary));

        //    //Assert
        //    Assert.Equal(metadata.Path, fakeRouteDescription.Path);
        //    Assert.Equal(metadata.Method, fakeRouteDescription.Method.ToLowerInvariant());
        //    Assert.Equal(metadata.Name, fakeRouteDescription.Name);
        //    Assert.Equal(metadata.Info.OperationId, endpoint.Operation);
        //    Assert.Equal(metadata.Info.Summary, endpoint.Summary);
        //}
    }
}

