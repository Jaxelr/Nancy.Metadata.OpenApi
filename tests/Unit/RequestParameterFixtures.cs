using System;
using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Fluent;
using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Tests.Fakes;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.Unit
{
    public class RequestParameterFixtures
    {
        [Fact]
        public void Request_parameter_string()
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
        public void Request_parameter_short()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string INT = "integer";
            const string BYTES = "int32";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                typeof(short),
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(INT, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(BYTES, endpoint.RequestParameters[0].Schema.Format);
        }

        [Fact]
        public void Request_parameter_integer()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string INT = "integer";
            const string BYTES = "int32";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                typeof(int),
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(INT, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(BYTES, endpoint.RequestParameters[0].Schema.Format);
        }

        [Fact]
        public void Request_parameter_long()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string INT = "integer";
            const string BYTES = "int64";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                typeof(long),
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(INT, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(BYTES, endpoint.RequestParameters[0].Schema.Format);
        }

        [Fact]
        public void Request_parameter_float()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string NUMBER = "number";
            const string FLOAT = "float";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                typeof(decimal),
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(NUMBER, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(FLOAT, endpoint.RequestParameters[0].Schema.Format);
        }

        [Fact]
        public void Request_parameter_double()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string NUMBER = "number";
            const string DOUBLE = "double";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                typeof(double),
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(NUMBER, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(DOUBLE, endpoint.RequestParameters[0].Schema.Format);
        }

        [Fact]
        public void Request_parameter_byte()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string BYTE = "byte";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                typeof(byte),
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(fakeRequest.Type.Name.ToLowerInvariant(), endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(BYTE, endpoint.RequestParameters[0].Schema.Format);
        }

        [Fact]
        public void Request_parameter_boolean()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string BOOLEAN = "boolean";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                typeof(bool),
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(BOOLEAN, endpoint.RequestParameters[0].Schema.Type);
            Assert.Null(endpoint.RequestParameters[0].Schema.Format);
        }

        [Fact]
        public void Request_parameter_datetime()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string DATE_TIME = "date-time";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                typeof(DateTime),
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(LocGenerator.GetLocByEnum(fakeRequest.Loc), endpoint.RequestParameters[0].In);
            Assert.Equal(fakeRequest.Type.Name.ToLowerInvariant(), endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(DATE_TIME, endpoint.RequestParameters[0].Schema.Format);
            Assert.Equal(fakeRequest.Type.Name.ToLowerInvariant(), endpoint.RequestParameters[0].Schema.Type);
        }

        [Fact]
        public void Request_parameter_object()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string OBJECT = "object";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.Operation)
                .WithRequestParameter(
                fakeRequest.Name,
                typeof(object),
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
            Assert.Equal(OBJECT, endpoint.RequestParameters[0].Schema.Type);
        }
    }
}
