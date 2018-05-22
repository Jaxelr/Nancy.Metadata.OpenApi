using Nancy.Metadata.OpenApi.Fluent;
using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Tests.Fakes;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.UnitTests
{
    public class RequestParameterTests
    {
        [Fact]
        public void Request_parameter_string()
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
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Null(endpoint.RequestParameters[0].Schema.Format);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
            Assert.Equal(fakeRequest.Type, endpoint.RequestParameters[0].Schema.Type);
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
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestParameter(
                fakeRequest.Name,
                INT,
                fakeRequest.Format,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
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
            const string LONG = "long";
            const string BYTES = "int64";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestParameter(
                fakeRequest.Name,
                LONG,
                fakeRequest.Format,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
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
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestParameter(
                fakeRequest.Name,
                FLOAT,
                fakeRequest.Format,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
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
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestParameter(
                fakeRequest.Name,
                DOUBLE,
                fakeRequest.Format,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
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
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestParameter(
                fakeRequest.Name,
                BYTE,
                fakeRequest.Format,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
            Assert.Equal(fakeRequest.Type, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(BYTE, endpoint.RequestParameters[0].Schema.Format);
        }

        [Fact]
        public void Request_parameter_binary()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string BINARY = "binary";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestParameter(
                fakeRequest.Name,
                BINARY,
                fakeRequest.Format,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
            Assert.Equal(fakeRequest.Type, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(BINARY, endpoint.RequestParameters[0].Schema.Format);
        }

        [Fact]
        public void Request_parameter_boolean()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string BOOLEAN = "boolean";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestParameter(
                fakeRequest.Name,
                BOOLEAN,
                fakeRequest.Format,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
            Assert.Equal(BOOLEAN, endpoint.RequestParameters[0].Schema.Type);
            Assert.Null(endpoint.RequestParameters[0].Schema.Format);
        }

        [Fact]
        public void Request_parameter_date()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string DATE = "date";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestParameter(
                fakeRequest.Name,
                DATE,
                fakeRequest.Format,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
            Assert.Equal(fakeRequest.Type, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(DATE, endpoint.RequestParameters[0].Schema.Format);
            Assert.Equal(fakeRequest.Type, endpoint.RequestParameters[0].Schema.Type);
        }

        [Fact]
        public void Request_parameter_datetime()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string DATETIME = "datetime";
            const string DATE_TIME = "date-time";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestParameter(
                fakeRequest.Name,
                DATETIME,
                fakeRequest.Format,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
            Assert.Equal(fakeRequest.Type, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(DATE_TIME, endpoint.RequestParameters[0].Schema.Format);
            Assert.Equal(fakeRequest.Type, endpoint.RequestParameters[0].Schema.Type);
        }

        [Fact]
        public void Request_parameter_password()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            var fakeRequest = new FakeRequest();
            const string PASSWORD = "password";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName)
                .WithRequestParameter(
                fakeRequest.Name,
                PASSWORD,
                fakeRequest.Format,
                fakeRequest.Required,
                fakeRequest.Description,
                fakeRequest.Loc,
                fakeRequest.Deprecated);

            //Assert
            Assert.Equal(fakeRequest.Description, endpoint.RequestParameters[0].Description);
            Assert.Equal(fakeRequest.Required, endpoint.RequestParameters[0].Required);
            Assert.Equal(fakeRequest.Name, endpoint.RequestParameters[0].Name);
            Assert.Equal(fakeRequest.Loc, endpoint.RequestParameters[0].In);
            Assert.Equal(fakeRequest.Type, endpoint.RequestParameters[0].Schema.Type);
            Assert.Equal(PASSWORD, endpoint.RequestParameters[0].Schema.Format);
            Assert.Equal(fakeRequest.Type, endpoint.RequestParameters[0].Schema.Type);
        }
    }
}
