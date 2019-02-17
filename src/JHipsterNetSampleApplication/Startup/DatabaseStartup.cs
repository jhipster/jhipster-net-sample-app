
namespace JHipsterNetSampleApplication.Startup {

    using System;
    using JHipsterNetSampleApplication.Data.EntityFramework;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DatabaseConfiguration {
        public static IServiceCollection AddDatabaseModule(this IServiceCollection @this, IConfiguration configuration)
        {
            @this.AddDbContext<JHipsterDataContext>(ctx => {
                ctx.UseInMemoryDatabase(databaseName: "jHipsterNet_In_Memory_Database");
                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                ctx.UseOpenIddict();
            });

            return @this;
        }

        public static IApplicationBuilder UseApplicationDatabase(this IApplicationBuilder @this, IServiceProvider serviceProvider, IHostingEnvironment environment)
        {
            if (!environment.IsDevelopment()) {
                return @this;
            }

            var context = serviceProvider.GetRequiredService<JHipsterDataContext>();
            context.Database.EnsureCreated();

            return @this;
        }
    }
}
