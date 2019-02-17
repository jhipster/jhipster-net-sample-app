
namespace JHipsterNetSampleApplication.Startup {
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using JHipsterNet.Pagination.Binders;
    using Microsoft.AspNetCore.Mvc.Versioning;

    public static class WebConfiguration {
        public static IServiceCollection AddWebModule(this IServiceCollection @this)
        {
            @this.AddHttpContextAccessor();

            @this.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            @this.AddApiVersioning(o => {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ApiVersionReader = new HeaderApiVersionReader("version");
            });

            // In production, the Angular files will be served from this directory
            @this.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });

            //https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2
            @this.AddHealthChecks();

            //TODO use AddMvcCore + config
            @this.AddMvc(options => { options.ModelBinderProviders.Insert(0, new PageableBinderProvider()); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            return @this;
        }

        public static IApplicationBuilder UseApplicationWeb(this IApplicationBuilder @this, IHostingEnvironment env)
        {
            @this.UseStaticFiles();
            @this.UseSpaStaticFiles();

            @this.UseMvc();

            @this.UseHealthChecks("/health");

            @this.UseWelcomePage();

            if (env.IsDevelopment()) {
                @this.UseDeveloperExceptionPage();
            }

            return @this;
        }
    }
}
