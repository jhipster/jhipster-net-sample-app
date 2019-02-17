
namespace JHipsterNetSampleApplication.Startup {
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;

    public static class AutoMapperStartup {
        public static IServiceCollection AddAutoMapperModule(this IServiceCollection @this)
        {
            @this.AddAutoMapper();

            return @this;
        }
    }
}
