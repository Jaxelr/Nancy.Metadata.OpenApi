using Nancy.Metadata.Modules;
using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.DemoApplication.Model;
using Nancy.Metadata.OpenApi.Fluent;
using Nancy.ModelBinding;

namespace Nancy.Metadata.OpenApi.DemoApplication.Modules
{
    public class RootModule : NancyModule
    {
        public RootModule() : base("/api")
        {
            Get("/hello", r => HelloWorld(), name: "SimpleRequest");
            Post("/hello", r => HelloPost(), name: "SimplePostRequst");
            Get("/hello/{name}", r => Hello(r.name), name: "SimpleRequestWithParameter");
            Post("/hello", r => r.HelloPost(), name: "SimplePostRequest");
            Post("hello/model/", r => HelloModel(), name: "PostRequestWithModel");
            Post("/hello/nestedmodel", r => r.HelloNestedModel(), name: "PostRequestWithNestedModel");
        }

        private Response HelloNestedModel()
        {
            NestedRequestModel model = this.Bind<NestedRequestModel>();

            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = $"Hello, {model.SimpleModel.Name}. We got your name from nested object"
            };

            return Response.AsJson(response);
        }

        private Response HelloModel()
        {
            SimpleRequestModel model = this.Bind<SimpleRequestModel>();

            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = $"Hello, {model.Name}"
            };

            return Response.AsJson(response);
        }

        private Response HelloPost()
        {
            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = "Hello Post!"
            };

            return Response.AsJson(response);
        }

        private Response Hello(string name)
        {
            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = $"Hello, {name}"
            };

            return Response.AsJson(response);
        }

        private Response HelloWorld()
        {
            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = "Hello World!"
            };

            return Response.AsJson(response);
        }
    }

    public class RootMetadataModule : MetadataModule<OpenApiRouteMetadata>
    {
        public RootMetadataModule()
        {
            Describe["SimpleRequest"] = desc => new OpenApiRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                            .WithSummary("Simple GET example"));

            Describe["SimpleRequestWithParameter"] = desc => new OpenApiRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                            .WithRequestParameter("name")
                            .WithSummary("Simple GET with parameters"));

            Describe["SimplePostRequst"] = desc => new OpenApiRouteMetadata(desc)
                .With(info => info.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                    .WithSummary("Simple POST example"));

            Describe["PostRequestWithModel"] = desc => new OpenApiRouteMetadata(desc)
                .With(info => info.WithResponseModel("200", typeof(SimpleResponseModel))
                    .WithResponse("400", "Bad request")
                    .WithSummary("Simple POST example with request model")
                    .WithRequestModel(typeof(SimpleRequestModel)));

            Describe["PostRequestWithNestedModel"] = desc => new OpenApiRouteMetadata(desc)
                .With(info => info.WithResponseModel("200", typeof(SimpleResponseModel))
                    .WithResponse("400", "Bad request")
                    .WithSummary("Simple POST example with nested request model")
                    .WithRequestModel(typeof(NestedRequestModel)));
        }
    }
}