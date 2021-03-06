﻿using System;
using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Fluent;
using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Tests.Fakes;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.Unit
{
    public class EndpointInfoSecurityFixtures
    {
        [Obsolete]
        [Theory]
        [InlineData("my_custom_api_key", "Basic Custom Authentication Sample")]
        public void Endpoint_with_custom_authentication(string name, string description)
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();

            //This is defined by the standard
            const string type = "apiKey";
            const string location = "cookie";

            //Act
            _ = new Endpoint(fakeEndpoint.Operation).WithApiKeyAuthentication(name, location, description);
            bool success = SchemaCache.SecurityCache.TryGetValue(name, out SecurityScheme securityScheme);

            //Assert
            Assert.True(success);
            Assert.Equal(securityScheme.Name, name);
            Assert.Equal(securityScheme.In, location);
            Assert.Equal(securityScheme.Type, type);
            Assert.Equal(securityScheme.Description, description);
        }

        [Theory]
        [InlineData("my_custom_api_key_2", "Basic Custom Authentication Sample")]
        public void Endpoint_with_custom_authentication_using_enumeration(string name, string description)
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();

            //This is defined by the standard
            const string type = "apiKey";
            const Loc location = Loc.Cookie;

            //Act
            _ = new Endpoint(fakeEndpoint.Operation).WithApiKeyAuthentication(name, location, description);
            bool success = SchemaCache.SecurityCache.TryGetValue(name, out SecurityScheme securityScheme);

            //Assert
            Assert.True(success);
            Assert.Equal(securityScheme.Name, name);
            Assert.Equal(securityScheme.In, LocGenerator.GetLocByEnum(location));
            Assert.Equal(securityScheme.Type, type);
            Assert.Equal(securityScheme.Description, description);
        }

        [Theory]
        [InlineData("my_custom_api_key_3", "Basic Custom Authentication Sample")]

        public void Endpoint_with_custom_authentication_with_invalid_location(string name, string description)
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();

            //This is defined by the standard
            const Loc location = Loc.Path;

            //Act
            Assert.Throws<ArgumentOutOfRangeException>(() => new Endpoint(fakeEndpoint.Operation).WithApiKeyAuthentication(name, location, description));
            bool success = SchemaCache.SecurityCache.TryGetValue(name, out SecurityScheme securityScheme);

            //Assert
            Assert.False(success);
        }

        [Fact]
        public void Endpoint_with_basic_authentication()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            const string scheme = "basic";
            const string type = "http";
            const string description = "Basic Authentication Sample";

            //Act
            _ = new Endpoint(fakeEndpoint.Operation).WithBasicAuthentication(description);
            bool success = SchemaCache.SecurityCache.TryGetValue(scheme, out SecurityScheme securityScheme);

            //Assert
            Assert.True(success);
            Assert.Equal(securityScheme.Name, scheme);
            Assert.Equal(securityScheme.Scheme, scheme);
            Assert.Equal(securityScheme.Type, type);
            Assert.Equal(securityScheme.Description, description);
        }

        [Fact]
        public void Endpoint_with_basic_authentication_twice()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            const string scheme = "basic";
            const string type = "http";
            const string description = "Basic Authentication Sample";

            //Act
            SchemaCache.SecurityCache.Clear(); //This test requires that the size of the Schema Cache is 0 at the start.

            _ = new Endpoint(fakeEndpoint.Operation)
                .WithBasicAuthentication(description)
                .WithBasicAuthentication(description);
            bool success = SchemaCache.SecurityCache.TryGetValue(scheme, out SecurityScheme securityScheme);

            //Assert
            Assert.True(success);
            Assert.Single(SchemaCache.SecurityCache);
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
            const string bearerFormat = "jwt";
            const string type = "http";
            const string scheme = "bearer";
            const string description = "Bearer Auth Sample";

            //Act
            _ = new Endpoint(fakeEndpoint.Operation).WithBearerAuthentication(bearerFormat, description);
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
            var fakeEndpoint = new FakeEndpoint();
            const string type = "openIdConnect";
            const string name = "openIdConnect";
            const string authUrl = "http://www.fakymcfake.com/authorization";
            const string tokenUrl = "http://www.fakymcfake.com/tokenAuth";
            const string refreshUrl = "http://www.fakymcfake.com/refresh";
            const string openIdurl = "http://www.fakeaddress.com/myopenidconnectpoint";
            const string flow = "implicit";
            const string description = "OAuth2 Authentication Sample";
            string[] scopes = new string[] { "read", "write" };

            //Act
            _ = new Endpoint(fakeEndpoint.Operation).WithOpenIdConnectAuthentication(authUrl, flow, tokenUrl, openIdurl, description, refreshUrl, scopes);
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
        public void Endpoint_with_oauth2_authentication_client_credentials()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            const string type = "oauth2";
            const string authUrl = "http://www.fakymcfake.com/authorization";
            const string tokenUrl = "http://www.fakymcfake.com/tokenAuth";
            const string refreshUrl = "http://www.fakymcfake.com/refresh";
            const string flow = "clientCredentials";
            string name = string.Concat(type, flow);
            const string description = "OAuth2 Authentication with Client Credentials Sample";
            string[] scopes = new string[] { "read", "write" };

            //Act
            _ = new Endpoint(fakeEndpoint.Operation).WithOAuth2Authentication(authUrl, flow, tokenUrl, description, refreshUrl, scopes);
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

        [Fact]
        public void Endpoint_with_oauth2_authentication_authorization_code()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            const string type = "oauth2";
            const string authUrl = "http://www.fakymcfake.com/authorization";
            const string tokenUrl = "http://www.fakymcfake.com/tokenAuth";
            const string refreshUrl = "http://www.fakymcfake.com/refresh";
            const string flow = "authorizationcode";
            string name = string.Concat(type, flow);
            const string description = "OAuth2 Authentication with Authorization Code Sample";
            string[] scopes = new string[] { "read", "write" };

            //Act
            _ = new Endpoint(fakeEndpoint.Operation).WithOAuth2Authentication(authUrl, flow, tokenUrl, description, refreshUrl, scopes);
            bool success = SchemaCache.SecurityCache.TryGetValue(name, out SecurityScheme securityScheme);

            //Assert
            Assert.True(success);
            Assert.Equal(securityScheme.Name, name);
            Assert.Equal(securityScheme.Type, type);
            Assert.Equal(securityScheme.Flows.AuthorizationCode.AuthorizationUrl, authUrl);
            Assert.Equal(securityScheme.Flows.AuthorizationCode.TokenUrl, tokenUrl);
            Assert.Equal(securityScheme.Flows.AuthorizationCode.RefreshUrl, refreshUrl);
            Assert.Equal(securityScheme.Flows.AuthorizationCode.Scopes, scopes);
            Assert.Equal(securityScheme.Description, description);
        }

        [Fact]
        public void Endpoint_with_oauth2_authentication_password()
        {
            //Arrange
            var fakeEndpoint = new FakeEndpoint();
            const string type = "oauth2";
            const string authUrl = "http://www.fakymcfake.com/authorization";
            const string tokenUrl = "http://www.fakymcfake.com/tokenAuth";
            const string refreshUrl = "http://www.fakymcfake.com/refresh";
            const string flow = "password";
            string name = string.Concat(type, flow);
            const string description = "OAuth2 Authentication with Password Sample";
            string[] scopes = new string[] { "read", "write" };

            //Act
            _ = new Endpoint(fakeEndpoint.Operation).WithOAuth2Authentication(authUrl, flow, tokenUrl, description, refreshUrl, scopes);
            bool success = SchemaCache.SecurityCache.TryGetValue(name, out SecurityScheme securityScheme);

            //Assert
            Assert.True(success);
            Assert.Equal(securityScheme.Name, name);
            Assert.Equal(securityScheme.Type, type);
            Assert.Equal(securityScheme.Flows.Password.AuthorizationUrl, authUrl);
            Assert.Equal(securityScheme.Flows.Password.TokenUrl, tokenUrl);
            Assert.Equal(securityScheme.Flows.Password.RefreshUrl, refreshUrl);
            Assert.Equal(securityScheme.Flows.Password.Scopes, scopes);
            Assert.Equal(securityScheme.Description, description);
        }
    }
}
