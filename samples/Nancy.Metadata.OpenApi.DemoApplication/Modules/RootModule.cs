using Nancy.Metadata.Modules;
using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.DemoApplication.Model;
using Nancy.Metadata.OpenApi.Fluent;
using Nancy.ModelBinding;
using System.Linq;

namespace Nancy.Metadata.OpenApi.DemoApplication.Modules
{
    public class RootModule : NancyModule
    {
        public RootModule() : base("/api")
        {
            Get("/hello", r => HelloWorld(), name: "SimpleRequest");
            Get("/hello/{name}", r => Hello(r.name), name: "SimpleRequestWithParameter");
            Get("/hello/{names}", r => Hello(r.names), name: "SimpleRequestWithParameterArray");
            Get("/Count/{number}", r => HelloCount(r.number), name: "SimpleRequestWithNumericParameter");
            Post("/hello", r => HelloPost(), name: "SimplePostRequest");
            Post("hello/model", r => HelloModel(), name: "PostRequestWithModel");
            Post("/hello/nestedmodel", r => HelloNestedModel(), name: "PostRequestWithNestedModel");
        }

        private Response HelloCount(int number) => Response.AsJson(new SimpleResponseModel { Hello = $"Hello, {number + 1}" });

        private Response HelloPost() => Response.AsJson(new SimpleResponseModel { Hello = "Hello Post!" });

        private Response Hello(string name) => Response.AsJson(new SimpleResponseModel { Hello = $"Hello, {name}" });

        private Response HelloWorld() => Response.AsJson(new SimpleResponseModel { Hello = "Hello World!" });

        private Response HelloNestedModel()
        {
            try
            {
                var model = this.BindAndValidate<NestedRequestModel>();

                if (!ModelValidationResult.IsValid)
                {
                    return Response
                         .AsJson(new ValidationFailedResponseModel(ModelValidationResult))
                         .WithStatusCode(HttpStatusCode.BadRequest);
                }

                var response = new SimpleResponseModel
                {
                    Hello = $"Hello, {model?.SimpleModel.Name}. We got your name from a nested object."
                };

                return Response.AsJson(response);
            }
            catch (ModelBindingException)
            {
                return Response
                     .AsJson(new ValidationFailedResponseModel("Model Binding Failed with Exception."))
                     .WithStatusCode(HttpStatusCode.BadRequest);
            }
            catch (System.NullReferenceException)
            {
                return Response
                .AsJson(new ValidationFailedResponseModel("The body contains an invalid nested request model."))
                .WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private Response HelloModel()
        {
            try
            {
                SimpleRequestModel model = this.BindAndValidate<SimpleRequestModel>();

                if (!ModelValidationResult.IsValid)
                {
                    return Response
                         .AsJson(new ValidationFailedResponseModel(ModelValidationResult))
                         .WithStatusCode(HttpStatusCode.BadRequest);
                }

                var response = new SimpleResponseModel
                {
                    Hello = $"Hello, {model.Name}."
                };

                return Response.AsJson(response);
            }
            catch (ModelBindingException)
            {
                return Response
                     .AsJson(new ValidationFailedResponseModel("Model Binding Failed with Exception."))
                     .WithStatusCode(HttpStatusCode.BadRequest);
            }
            catch (System.NullReferenceException)
            {
                return Response
                .AsJson(new ValidationFailedResponseModel("The body contains an invalid nested request model."))
                .WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private Response Hello(string[] names)
        {
            var response = new SimpleResponseModel
            {
                Hello = names.Aggregate((curr, next) => string.Concat(curr, ", ", next))
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
                            .WithDescription("This is a simple request", "Sample")
                            .WithSummary("Simple GET example"));

            Describe["SimpleRequestWithParameter"] = desc => new OpenApiRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                            .WithDescription("This is a simple request with a parameter", "Sample")
                            .WithRequestParameter("name")
                            .WithSummary("Simple GET with parameters"));

            Describe["SimpleRequestWithNumericParameter"] = desc => new OpenApiRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                            .WithDescription("This is a simple request with numeric parameter", "Sample")
                            .WithRequestParameter("number", type: typeof(int), loc: "path")
                            .WithSummary("Simple GET with numeric parameters"));

            Describe["SimpleRequestWithParameterArray"] = desc => new OpenApiRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                            .WithRequestParameter("names", isArray: true, type: typeof(string))
                            .WithDescription("This is a simple request with a parameter array", "Sample")
                            .WithSummary("Simple GET with array parameters"));

            Describe["SimplePostRequest"] = desc => new OpenApiRouteMetadata(desc)
                .With(info => info.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                            .WithDescription("This is a post request", "Sample")
                            .WithSummary("Simple POST example"));

            Describe["PostRequestWithModel"] = desc => new OpenApiRouteMetadata(desc)
                .With(info => info.WithResponseModel("200", typeof(SimpleResponseModel), "Simple Response Model")
                            .WithDescription("This is post request with a response model", "Sample")
                            .WithResponseModel("400", typeof(ValidationFailedResponseModel), "Validation Failed Model")
                            .WithSummary("Simple POST example with request model")
                            .WithRequestModel(typeof(SimpleRequestModel), contentType: "application/json", description: "Simple Request Model"));

            Describe["PostRequestWithNestedModel"] = desc => new OpenApiRouteMetadata(desc)
                    .With(info => info.WithResponseModel("200", typeof(SimpleResponseModel), "Simple Response Model")
                            .WithDescription("This is post request with a nested model", "Sample")
                            .WithResponseModel("400", typeof(ValidationFailedResponseModel), "Validation Failed Model")
                            .WithSummary("Simple POST example with nested request model")
                            .WithRequestModel(typeof(NestedRequestModel)));
        }
    }
}
