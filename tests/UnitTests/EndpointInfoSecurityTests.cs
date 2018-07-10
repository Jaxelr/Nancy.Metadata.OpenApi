using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Fluent;
using Nancy.Metadata.OpenApi.Tests.Fakes;
using Nancy.Metadata.OpenApi.Core;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.UnitTests
{
    public class EndpointInfoSecurityTests
    {
        [Fact]
        public void Endpoint_with_custom_authentication()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            
            //This is defined by the standard
            string name = "my_custom_api_key";
            string type = "apiKey";
            string location = "cookie";
            string description = "Basic Custom Authentication Sample";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName).WithApiKeyAuthentication(name, location, description);
            bool success = SchemaCache.SecurityCache.TryGetValue(name, out SecurityScheme securityScheme);

            //Assert
            Assert.True(success);
            Assert.Equal(securityScheme.Name, name);
            Assert.Equal(securityScheme.In, location);
            Assert.Equal(securityScheme.Type, type);
            Assert.Equal(securityScheme.Description, description);
        }

        [Fact]
        public void Endpoint_with_basic_authentication()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            string scheme = "basic";
            string type = "http";
            string description = "Basic Authentication Sample";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName).WithBasicAuthentication(description);
            bool success = SchemaCache.SecurityCache.TryGetValue(scheme, out SecurityScheme securityScheme);

            //Assert
            Assert.True(success);
            Assert.Equal(securityScheme.Name, scheme);
            Assert.Equal(securityScheme.Scheme, scheme);
            Assert.Equal(securityScheme.Type, type);
            Assert.Equal(securityScheme.Description, description);
        }


        [Fact]
        public void Endpoint_with_bearer_authentication()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            string bearerFormat = "jwt";
            string type = "http";
            string scheme = "bearer";
            string description = "Bearer Auth Sample";

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName).WithBearerAuthentication(bearerFormat, description);
            bool success = SchemaCache.SecurityCache.TryGetValue(scheme, out SecurityScheme securityScheme);

            //Assert
            Assert.True(success);
            Assert.Equal(securityScheme.Name, scheme);
            Assert.Equal(securityScheme.Scheme, scheme);
            Assert.Equal(securityScheme.BearerFormat, bearerFormat);
            Assert.Equal(securityScheme.Type, type);
            Assert.Equal(securityScheme.Description, description);
        }


        [Fact]
        public void Endpoint_with_openId_authentication()
        {
            //Arrange
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            string type = "openIdConnect";
            string name = "openIdConnect";
            string authUrl = "http://www.fakymcfake.com/authorization";
            string tokenUrl = "http://www.fakymcfake.com/tokenAuth";
            string refreshUrl = "http://www.fakymcfake.com/refresh";
            string openIdurl = "http://www.fakeaddress.com/myopenidconnectpoint";
            string flow = "implicit";
            string description = "OAuth2 Authentication Sample";
            string[] scopes = new string[] { "read", "write" };

            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName).WithOpenIdConnectAuthentication(authUrl, flow, tokenUrl, openIdurl, description, refreshUrl, scopes);
            bool success = SchemaCache.SecurityCache.TryGetValue(name, out SecurityScheme securityScheme);

            //Assert
            Assert.True(success);
            Assert.Equal(securityScheme.Name, name);
            Assert.Equal(securityScheme.Type, type);
            Assert.Equal(securityScheme.OpenIdConnectUrl, openIdurl);
            Assert.Equal(securityScheme.Flows.Implicit.AuthorizationUrl, authUrl);
            Assert.Equal(securityScheme.Flows.Implicit.TokenUrl, tokenUrl);
            Assert.Equal(securityScheme.Flows.Implicit.RefreshUrl, refreshUrl);
            Assert.Equal(securityScheme.Flows.Implicit.Scopes, scopes);
            Assert.Equal(securityScheme.Description, description);
        }

        [Fact]
        public void Endpoint_with_oauth2_authentication()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            string type = "oauth2";
            string name = "oauth2";
            string authUrl = "http://www.fakymcfake.com/authorization";
            string tokenUrl = "http://www.fakymcfake.com/tokenAuth";
            string refreshUrl = "http://www.fakymcfake.com/refresh";
            string flow = "clientCredentials";
            string description = "OAuth2 Authentication Sample";
            string[] scopes = new string[] { "read", "write" };


            //Act
            var endpoint = new Endpoint(fakeEndpoint.OperationName).WithOAuth2Authentication(authUrl, flow, tokenUrl, description, refreshUrl, scopes);
            bool success = SchemaCache.SecurityCache.TryGetValue(name, out SecurityScheme securityScheme);

            //Assert
            Assert.True(success);
            Assert.Equal(securityScheme.Name, name);
            Assert.Equal(securityScheme.Type, type);
            Assert.Equal(securityScheme.Flows.ClientCredentials.AuthorizationUrl, authUrl);
            Assert.Equal(securityScheme.Flows.ClientCredentials.TokenUrl, tokenUrl);
            Assert.Equal(securityScheme.Flows.ClientCredentials.RefreshUrl, refreshUrl);
            Assert.Equal(securityScheme.Flows.ClientCredentials.Scopes, scopes);
            Assert.Equal(securityScheme.Description, description);
        }
    }
}


