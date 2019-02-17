
namespace JHipsterNetSampleApplication {
    using System;
    using JHipsterNetSampleApplication.Service;
    using JHipsterNetSampleApplication.Startup;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class JHipsterStartup {
        public JHipsterStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddWebModule()
                .AddDatabaseModule(Configuration)
                .AddSecurityModule()
                .AddNhipsterModule(Configuration)
                .AddProblemDetailsModule()
                .AddAutoMapperModule()
                .AddSwaggerModule();

            services.AddScoped<IMailService, MailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app
                .UseApplicationSecurity(env)
                .UseApplicationWeb(env)
                .UseApplicationDatabase(serviceProvider, env)
                .UseApplicationProblemDetails()
                .UseApplicationSwagger();
        }
    }
}
