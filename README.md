# Nancy.Metadata.OpenApi [![Mit License][mit-img]][mit]

This is a forked version of [Jaxelr/Nancy.Metadata.OpenApi](https://github.com/Jaxelr/Nancy.Metadata.OpenApi)

_Now_ compatible to Nancy 1.X and Nancy 2.0!

You can find the latest specifications of [OpenApi here](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md) 

## Builds

| Appveyor  |
| :---:     |
| [![Build status][build-img]][build] |

## Packages

NuGet (Stable) | MyGet (Prerelease)
:---: | :---:
[![NuGet][nuget-img]][nuget] | [![MyGet][myget-img]][myget] |


## Installation:

Via nuget: 

``` powershell
PM> Install-Package Nancy.Metadata.OpenApi 
```

You also need the Metadata library provided by Nancyfx:

``` powershell
PM> Install-Package Nancy.Metadata.Modules 
```

## Usage:

Define a docs module that will serve our OpenApi Json (currently only json is supported) document:

```c#
using Nancy.Metadata.OpenApi.Model;
using Nancy.Metadata.OpenApi.Modules;

public class DocsModule : OpenApiDocsModuleBase //We must inherit from the OpenApiDocsModuleBase
{
    //Could be an array of Servers.
    public static Server Server 
        => new Server() { Description = "My Descripton", Url = "http://localhost:5000/" };

    public static string[] Tags => new string[] { "sample", "openapi" };
    
    public DocsModule(IRouteCacheProvider routeCacheProvider) :
        base(routeCacheProvider,
        "/api/docs/openapi.json",       //Document location path
        "My API ",                      //Api Title 
        "v1.0",                         //Version of the Api
        Server,
        "/api",                         //Base url
        Tags)                           //Document Tags
    {
    }
}
```

We could optionally, if the information is needed, add Contact, License and External Docs information:

``` c#
public class DocsModule : OpenApiDocsModuleBase //We must inherit from the OpenApiDocsModuleBase
{
    public static Server Server 
        => new Server() { Description = "My Descripton", Url = "http://localhost:5001/" };
        
    public static string[] Tags => new string[] { "sample", "openapi" };

    public DocsModule(IRouteCacheProvider routeCacheProvider) :
        base(routeCacheProvider, "/api/docs/openapi.json", "My API 2", "v1.1", Server, "/api", Tags)
    {
        //Optional information.
        WithContact("Contact Information", "jaxelrojas@email.com", "https://jaxelr.github.io");

        //Optional information.
        WithLicense("MIT", "https://opensource.org/licenses/MIT");

        //Optional Information.
        WithExternalDocument("This is an external doc, maybe a tutorial or a spec doc.", "https://jaxelr.github.io")
    }
}
```

Then, define the Nancy modules as you would usually do:

```c#

//Example using Nancy v2 

public class MyModule : NancyModule
{
    public MyModule() : base("/api")
    {
        Get("/hello", r => HelloWorld(), name: "SimpleRequest");
        Get("/hello/{name}", r => Hello(r.name), name: "SimpleRequestWithParameter");
    }
}

//Skipped method implementations for brevity sake...
```

Finally, you must define the metadata of the operations. To do so, simply declare the metadata module (using Nancy.Metadata.Modules) on the same namespace as the endpoint operations were defined, using the inherited MetadataModule class and the OpenApiRouteMetadata class defined on Nancy.Metadata.OpenApi.Core.

```c#
using Nancy.Metadata.Modules;
using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Fluent;

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

Thats pretty much it, the docs endpoint defined above would generate some valid OpenApi Json. You can validate the Open Api endpoint using [swagger-ui](https://github.com/swagger-api/swagger-ui). (For those unaware, OpenApi used to be called Swagger, so any reference to Swagger usually means version <= 2.0) Check the [Compatibility table](https://github.com/swagger-api/swagger-ui#compatibility) of UI for usage.

For a working example, clone this repo and see the sample app that uses the Swagger-UI site as a validator.

## Work to be done

Open Api specifies optional objects that havent been implemented at the library level. Check the [Current Development Project](https://github.com/Jaxelr/Nancy.Metadata.OpenApi/projects) for more details.

## Contributing

Check the [guidelines](https://github.com/Jaxelr/Nancy.Metadata.OpenApi/blob/master/.github/CONTRIBUTING.md) for a simple explanation on how you could help out.

[mit-img]: http://img.shields.io/badge/License-MIT-blue.svg
[mit]: https://github.com/Jaxelr/Nancy.Metadata.OpenApi/blob/master/LICENSE
[build]: https://ci.appveyor.com/project/Jaxelr/nancy-metadata-openapi
[build-img]: https://ci.appveyor.com/api/projects/status/bk8fiqknunkegnv7?svg=true
[nuget]: https://www.nuget.org/packages/Nancy.Metadata.OpenApi
[nuget-img]: https://img.shields.io/nuget/v/Nancy.Metadata.OpenApi.svg
[myget]: https://www.myget.org/feed/nancy-metadata-openapi/package/nuget/Nancy.Metadata.OpenApi
[myget-img]: https://img.shields.io/myget/nancy-metadata-openapi/v/Nancy.Metadata.OpenApi.svg
