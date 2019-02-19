
namespace JHipsterNet.Mvc.Problems {
    using Hellang.Middleware.ProblemDetails;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using System;
    using System.Diagnostics;
    using System.Security.Authentication;

    public class ProblemDetailsConfiguration : IConfigureOptions<ProblemDetailsOptions> {
        public ProblemDetailsConfiguration(IHostingEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _HttpContextAccessor = httpContextAccessor;
        }

        private IHostingEnvironment _environment { get; }
        private IHttpContextAccessor _HttpContextAccessor { get; }

        public void Configure(ProblemDetailsOptions options)
        {
            options.IncludeExceptionDetails = ctx => _environment.IsDevelopment();

            options.OnBeforeWriteDetails = details => {
                // keep consistent with asp.net core 2.2 conventions that adds a tracing value
                var traceId = Activity.Current?.Id ?? _HttpContextAccessor.HttpContext.TraceIdentifier;
                details.Extensions["traceId"] = traceId;
            };

            options.Map<AuthenticationException>(exception =>
                new ExceptionProblemDetails(exception, StatusCodes.Status401Unauthorized));
            options.Map<NotImplementedException>(exception =>
                new ExceptionProblemDetails(exception, StatusCodes.Status501NotImplemented));

            //TODO add Headers to HTTP responses
        }
    }
}
