using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Tests.Fakes;
using Nancy.Routing;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
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
            string body;

            using (var memoryStream = new MemoryStream())
            {
                response.Contents.Invoke(memoryStream);
                body = Encoding.UTF8.GetString(memoryStream.GetBuffer());
            }

            var spec = JsonConvert.DeserializeObject<OpenApiSpecification>(body);

            //Assert
            Assert.All(spec.Servers, item => Assert.Contains(item.Description, FakeDocsModule.Server.Description));
            Assert.All(spec.Servers, item => Assert.Contains(item.Url, FakeDocsModule.Server.Url));
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
            string body;

            using (var memoryStream = new MemoryStream())
            {
                response.Contents.Invoke(memoryStream);
                body = Encoding.UTF8.GetString(memoryStream.GetBuffer());
            }

            var spec = JsonConvert.DeserializeObject<OpenApiSpecification>(body);

            //Assert
            Assert.All(spec.Servers, item => Assert.Contains(item.Description, FakeDocsModule.Server.Description));
            Assert.All(spec.Servers, item => Assert.Contains(item.Url, FakeDocsModule.Server.Url));
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
            string body;

            using (var memoryStream = new MemoryStream())
            {
                response.Contents.Invoke(memoryStream);
                body = Encoding.UTF8.GetString(memoryStream.GetBuffer());
            }

            var spec = JsonConvert.DeserializeObject<OpenApiSpecification>(body);

            //Assert
            Assert.All(spec.Servers, item => Assert.Contains(item.Description, FakeDocsModule.Server.Description));
            Assert.All(spec.Servers, item => Assert.Contains(item.Url, FakeDocsModule.Server.Url));
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

            string body;

            using (var memoryStream = new MemoryStream())
            {
                response.Contents.Invoke(memoryStream);
                body = Encoding.UTF8.GetString(memoryStream.GetBuffer());
            }

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

            string body;

            using (var memoryStream = new MemoryStream())
            {
                response.Contents.Invoke(memoryStream);
                body = Encoding.UTF8.GetString(memoryStream.GetBuffer());
            }

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

            string body;

            using (var memoryStream = new MemoryStream())
            {
                response.Contents.Invoke(memoryStream);
                body = Encoding.UTF8.GetString(memoryStream.GetBuffer());
            }

            var spec = JsonConvert.DeserializeObject<OpenApiSpecification>(body);

            //Assert
            Assert.Equal(lic.Name, spec.Info.License.Name);
            Assert.Equal(lic.Url, spec.Info.License.Url);
        }
    }
}
