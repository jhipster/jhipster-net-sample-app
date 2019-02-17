
namespace JHipsterNetSampleApplication.Startup {
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;

    public static class SwaggerConfiguration {
        public static IServiceCollection AddSwaggerModule(this IServiceCollection @this)
        {
            @this.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info {Title = "JHipsterNetSampleApplication API v1", Version = "1.0"});
            });

            return @this;
        }

        public static IApplicationBuilder UseApplicationSwagger(this IApplicationBuilder @this)
        {
            @this.UseSwagger();
            @this.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "nhipsterSampleApplication API v1");
            });
            return @this;
        }
    }
}
