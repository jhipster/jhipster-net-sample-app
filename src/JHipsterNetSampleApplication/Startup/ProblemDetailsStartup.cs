

namespace JHipsterNetSampleApplication.Startup {
    using Hellang.Middleware.ProblemDetails;
    using JHipsterNet.Mvc.Problems;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class ProblemDetailsStartup {
        //TODO Understand difference between UI and non-ui Exceptions
        //https://github.com/christianacca/ProblemDetailsDemo/blob/master/src/ProblemDetailsDemo.Api/Startup.cs

        public static IServiceCollection AddProblemDetailsModule(this IServiceCollection @this)
        {
            @this.ConfigureOptions<ProblemDetailsConfiguration>();
            @this.AddProblemDetails();

            return @this;
        }

        public static IApplicationBuilder UseApplicationProblemDetails(this IApplicationBuilder @this)
        {
            @this.UseProblemDetails();
            return @this;
        }
    }
}
