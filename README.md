[![Build status](https://ci.appveyor.com/api/projects/status/bk8fiqknunkegnv7?svg=true)](https://ci.appveyor.com/project/Jaxelr/nancy-metadata-openapi)

# Nancy.Metadata.OpenApi

This repository is a fork of the https://github.com/HackandCraft/Nancy.Metadata.Swagger repository, due to inactivity. 

---
Currently works on the OpenApi specification 3.0.0 & 3.0.1, this was implemented for usage with NancyFx (using Nancy 2.0.0-clinteastwood for .Net Core). This repo depends on the following libraries:

* [NancyFx](https://github.com/NancyFx/Nancy)
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
* [NJsonSchema](https://github.com/NJsonSchema/NJsonSchema)


You can find the [OpenApi specification here](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.0.md) 

## Usage:

First we must define a docs module were we will retrieve the Open Api Json document:

```c#
using Nancy.Metadata.OpenApi.Modules;

    public class DocsModule : OpenApiDocsModuleBase //We must inherit from the OpenApiDocsModuleBase
    {
        public DocsModule(IRouteCacheProvider routeCacheProvider) : base(routeCacheProvider, "/api/docs", "Sample API documentation", "v1.0", "localhost:5000", "/api", "http")
        {
        }
    }
```

Then you define the Nancy modules as you would usually do:

```c#
    public class MyModule : NancyModule
    {
        public MyModule() : base("/api")
        {
            Get("/hello", r => HelloWorld(), name: "SimpleRequest");
            Get("/hello/{name}", r => Hello(r.name), name: "SimpleRequestWithParameter");
        }
    }
```

Finally, to define the metadata of the operations you must simply declare the metadata module (using Nancy.Metadata.Modules) on the same namespace as the endpoint operations were defined, using the inherited MetadataModule class and the OpenApiRouteMetadata class defined on Nancy.Metadata.OpenApi.Core.

```c#
 public class MyMetadataModule : MetadataModule<OpenApiRouteMetadata>
    {
        public MyMetadataModule()
        {
            Describe["SimpleRequest"] = desc => new OpenApiRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                            .WithSummary("Simple GET example"));

            Describe["SimpleRequestWithParameter"] = desc => new OpenApiRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                            .WithRequestParameter("name")
                            .WithSummary("Simple GET with parameters"));
        }
    }
```

Thats it, that would generate some valid OpenApi json. You can validate the Open Api endpoint using [swagger-ui](https://github.com/swagger-api/swagger-ui). (For those unaware, OpenApi used to be called Swagger, so any reference to Swagger usually means version <= 2.0) Check the [Compatibility table](https://github.com/swagger-api/swagger-ui#compatibility) of UI for usage.

For a working example,  clone this repo and see the demo application that uses the Swagger-UI site as a validator.

## Missing so far:

Open Api specifies certain optional objects that havent been implemented at the library level.

* A Security object at the root level.
* A Security scheme at the component level.
* A Callback scheme at the component level.
