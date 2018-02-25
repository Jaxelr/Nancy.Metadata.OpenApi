[![Build status](https://ci.appveyor.com/api/projects/status/bk8fiqknunkegnv7?svg=true)](https://ci.appveyor.com/project/Jaxelr/nancy-metadata-openapi) [![NuGet](https://img.shields.io/nuget/v/Nancy.Metadata.OpenApi.svg)](https://www.nuget.org/packages/Nancy.Metadata.OpenApi)

# Nancy.Metadata.OpenApi

 ** Currently on prerelease status following the Nancy 2.0.0 version [release](https://github.com/NancyFx/Nancy/milestone/45).

Designed for usage with the OpenApi spec 3.0.X.

---
This library depends on the following libraries:

* [NancyFx](https://github.com/NancyFx/Nancy)
* [Nancy.Metadata.Modules](https://github.com/NancyFx/Nancy/tree/master/src/Nancy.Metadata.Modules)
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
* [NJsonSchema](https://github.com/NJsonSchema/NJsonSchema)


You can find the [OpenApi the latest specification here](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md) 

Install via nuget: 

``` 
    PM> Install-Package Nancy.Metadata.OpenApi -Version 0.3.1-pre 
```

## Usage:

First, we must define a docs module were we will retrieve the Open Api Json (currently only json is supported) document:

```c#
using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Modules;

public class DocsModule : OpenApiDocsModuleBase //We must inherit from the OpenApiDocsModuleBase
{
    public DocsModule(IRouteCacheProvider routeCacheProvider) : 
        base(routeCacheProvider, 
        "/api/docs",                    //Document location path
        "My API ",                      //Api Title 
        "v1.0",                         //Version of the Api            
        new Server                      
        { 
            Url = "http://localhost:5000", 
            Description = "Sample Api Docs." 
        },                              //could be an array of Servers
        "/api")                         //Base url
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

Finally, you must define the metadata of the operations. To do so, simply declare the metadata module (using Nancy.Metadata.Modules) on the same namespace as the endpoint operations were defined, using the inherited MetadataModule class and the OpenApiRouteMetadata class defined on Nancy.Metadata.OpenApi.Core.

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

Thats pretty much it, that would generate some valid OpenApi json. You can validate the Open Api endpoint using [swagger-ui](https://github.com/swagger-api/swagger-ui). (For those unaware, OpenApi used to be called Swagger, so any reference to Swagger usually means version <= 2.0) Check the [Compatibility table](https://github.com/swagger-api/swagger-ui#compatibility) of UI for usage.

For a working example,  clone this repo and see the demo application that uses the Swagger-UI site as a validator.

## Missing so far:

Open Api specifies certain optional objects that havent been implemented at the library level. Check the [Current Development Project](https://github.com/Jaxelr/Nancy.Metadata.OpenApi/projects) for more details.
