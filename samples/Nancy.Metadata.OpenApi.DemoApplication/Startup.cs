using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;

namespace Nancy.Metadata.OpenApi.DemoApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseOwin(x => x.UseNancy());

            app.UseCors("CustomPolicy");
        }

        public void ConfigureServices(IServiceCollection services)
            => services.AddCors(o => o.AddPolicy("CustomPolicy",
                builder =>
            {
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            }));
    }
}
