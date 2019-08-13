using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Tests.Fakes;
using Nancy.Routing;
using Newtonsoft.Json;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.Unit
{
    public class ModuleFixtures
    {
        [Fact]
        public void Generate_docs()
        {
            //Arrange
            var module = new FakeDocsModule(new DefaultRouteCacheProvider(() => new FakeRouteCache()));

            //Act
            var response = module.GetDocumentation();

            string body = InvokeStreamToString(response.Contents);

            var spec = JsonConvert.DeserializeObject<OpenApiSpecification>(body);

            //Assert
            Assert.Equal(FakeDocsModule.Title, spec.Info.Title);
            Assert.Equal(FakeDocsModule.ApiVersion, spec.Info.Version);
        }

        [Fact]
        public void Generate_docs_twice()
        {
            //Arrange
            var module = new FakeDocsModule(new DefaultRouteCacheProvider(() => new FakeRouteCache()));

            //Act
            var response = module.GetDocumentation();
            response = module.GetDocumentation(); //We do this to make sure the cache object is not generated

            string body = InvokeStreamToString(response.Contents);

            var spec = JsonConvert.DeserializeObject<OpenApiSpecification>(body);

            //Assert
            Assert.Equal(FakeDocsModule.Title, spec.Info.Title);
            Assert.Equal(FakeDocsModule.ApiVersion, spec.Info.Version);
        }

        [Fact]
        public void Generate_docs_with_tags()
        {
            //Arrange
            Tag[] Tags = { new Tag() { Name = "Default" } };
            var module = new FakeDocsModule(new DefaultRouteCacheProvider(() => new FakeRouteCache()), Tags);

            //Act
            var response = module.GetDocumentation();

            string body = InvokeStreamToString(response.Contents);

            var spec = JsonConvert.DeserializeObject<OpenApiSpecification>(body);

            //Assert
            Assert.All(spec.Servers, item => Assert.Equal(item.Description, FakeDocsModule.Server.Description));
            Assert.All(spec.Servers, item => Assert.Equal(item.Url, FakeDocsModule.Server.Url));
            Assert.Equal(FakeDocsModule.Title, spec.Info.Title);
            Assert.Equal(FakeDocsModule.ApiVersion, spec.Info.Version);
            Assert.True(Tags.All(t => spec.Tags.Any(
                               s => t.Name == s.Name &&
                               t.Description == s.Description &&
                               t.ExternalDocumentation == s.ExternalDocumentation
                           )));
        }

        [Fact]
        public void Generate_docs_with_terms_of_services()
        {
            //Arrange
            string TermsOfService = "blah blah blah";
            var module = new FakeDocsModule(new DefaultRouteCacheProvider(() => new FakeRouteCache()), TermsOfService);

            //Act
            var response = module.GetDocumentation();

            string body = InvokeStreamToString(response.Contents);

            var spec = JsonConvert.DeserializeObject<OpenApiSpecification>(body);

            //Assert
            Assert.All(spec.Servers, item => Assert.Equal(item.Description, FakeDocsModule.Server.Description));
            Assert.All(spec.Servers, item => Assert.Equal(item.Url, FakeDocsModule.Server.Url));
            Assert.Equal(FakeDocsModule.Title, spec.Info.Title);
            Assert.Equal(FakeDocsModule.ApiVersion, spec.Info.Version);
            Assert.Equal(TermsOfService, spec.Info.TermsOfService);
        }

        [Fact]
        public void Generate_docs_with_contact_info()
        {
            //Arrange
            var module = new FakeDocsModule(new DefaultRouteCacheProvider(() => new FakeRouteCache()));
            var contact = new FakeContact();

            //Act
            module.FillContact(contact.Name, contact.Email, contact.Url);

            var response = module.GetDocumentation();

            string body = InvokeStreamToString(response.Contents);

            var spec = JsonConvert.DeserializeObject<OpenApiSpecification>(body);

            //Assert
            Assert.Equal(contact.Name, spec.Info.Contact.Name);
            Assert.Equal(contact.Email, spec.Info.Contact.Email);
            Assert.Equal(contact.Url, spec.Info.Contact.Url);
        }

        [Fact]
        public void Generate_docs_with_external_doc()
        {
            //Arrange
            var module = new FakeDocsModule(new DefaultRouteCacheProvider(() => new FakeRouteCache()));
            var doc = new FakeExternalDoc();

            //Act
            module.FillExternalDoc(doc.Description, doc.Url);

            var response = module.GetDocumentation();

            string body = InvokeStreamToString(response.Contents);

            var spec = JsonConvert.DeserializeObject<OpenApiSpecification>(body);

            //Assert
            Assert.Equal(doc.Description, spec.ExternalDocs.Description);
            Assert.Equal(doc.Url, spec.ExternalDocs.Url);
        }

        [Fact]
        public void Generate_docs_with_License()
        {
            //Arrange
            var module = new FakeDocsModule(new DefaultRouteCacheProvider(() => new FakeRouteCache()));
            var lic = new FakeLicense();

            //Act
            module.FillLicense(lic.Name, lic.Url);

            var response = module.GetDocumentation();

            string body = InvokeStreamToString(response.Contents);

            var spec = JsonConvert.DeserializeObject<OpenApiSpecification>(body);

            //Assert
            Assert.Equal(lic.Name, spec.Info.License.Name);
            Assert.Equal(lic.Url, spec.Info.License.Url);
        }

        [Fact]
        public void Generate_docs_with_expanded_server()
        {
            //Arrange
            var server = FakeServer.Server;

            var module = new FakeDocsModule(new DefaultRouteCacheProvider(() => new FakeRouteCache()), server);

            //Act
            var response = module.GetDocumentation();

            string body = InvokeStreamToString(response.Contents);

            var spec = JsonConvert.DeserializeObject<OpenApiSpecification>(body);

            //Assert
            Assert.All(spec.Servers, item => Assert.Equal(item.Description, server.Description));
            Assert.All(spec.Servers, item => Assert.Equal(item.Url, server.Url));
            Assert.All(spec.Servers.FirstOrDefault().Variables, item => Assert.Equal(item.Key, FakeServer.FakeKey));
            Assert.All(spec.Servers.FirstOrDefault().Variables, item => Assert.Equal(item.Value.Description, FakeServer.ServerVariable.Description));
            Assert.All(spec.Servers.FirstOrDefault().Variables, item => Assert.Equal(item.Value.Default, FakeServer.ServerVariable.Default));
            Assert.All(spec.Servers.FirstOrDefault().Variables, item => Assert.Equal(item.Value.Enum, FakeServer.ServerVariable.Enum));
        }

        [Theory]
        [InlineData("basic")]
        public void Get_security_requirements_on_metadata(string key)
        {
            //Arrange
            var module = new FakeDocsModule(new DefaultRouteCacheProvider(() => new FakeRouteCache()));
            var endpoint = new FakeEndpoint();
            var metadata = new Core.OpenApiRouteMetadata(endpoint.Path, endpoint.Method, endpoint.Operation);
            var securities = new List<Model.Security>
            {
                new Model.Security() { Key = key }
            };

            metadata.Info = new Endpoint(endpoint.Operation)
            {
                Security = securities
            };

            //Act
            var result = module.GetSecurityRequirements(metadata);

            //Assert
            Assert.True(result.All(r => r.Key == key));
            Assert.Single(result);
        }


        [Theory]
        [InlineData("basic")]
        public void Get_security_requirements_on_metadata_twice(string key)
        {
            //Arrange
            var module = new FakeDocsModule(new DefaultRouteCacheProvider(() => new FakeRouteCache()));
            var endpoint = new FakeEndpoint();
            var metadata = new Core.OpenApiRouteMetadata(endpoint.Path, endpoint.Method, endpoint.Operation);
            var securities = new List<Model.Security>
            {
                new Model.Security() { Key = key }
            };

            metadata.Info = new Endpoint(endpoint.Operation)
            {
                Security = securities
            };

            //Act
            _ = module.GetSecurityRequirements(metadata);
            var result = module.GetSecurityRequirements(metadata);

            //Assert
            Assert.True(result.All(r => r.Key == key));
            Assert.Single(result);
        }

        [Fact]
        public void Get_security_requirements_with_no_key()
        {
            //Arrange
            var module = new FakeDocsModule(new DefaultRouteCacheProvider(() => new FakeRouteCache()));
            var endpoint = new FakeEndpoint();
            var metadata = new Core.OpenApiRouteMetadata(endpoint.Path, endpoint.Method, endpoint.Operation);

            metadata.Info = new Endpoint(endpoint.Operation);

            //Act
            var result = module.GetSecurityRequirements(metadata);

            //Assert
            Assert.Empty(result);
        }

        private string InvokeStreamToString(Action<Stream> action)
        {
            string result = string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                action.Invoke(memoryStream);
                result = Encoding.UTF8.GetString(memoryStream.GetBuffer());
            }

            return result;
        }
    }
}
