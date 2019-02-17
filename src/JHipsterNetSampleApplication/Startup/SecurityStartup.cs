
namespace JHipsterNetSampleApplication.Startup {
    using AspNet.Security.OpenIdConnect.Primitives;
    using JHipsterNetSampleApplication.Data.EntityFramework;
    using JHipsterNetSampleApplication.Domain.Identity;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using OpenIddict.Validation;
    using System;

    public static class SecurityConfiguration {
        public static IServiceCollection AddSecurityModule(this IServiceCollection @this)
        {
            @this.AddIdentity<User, Role>(options => { options.SignIn.RequireConfirmedEmail = true; })
                    .AddEntityFrameworkStores<JHipsterDataContext>()
                    .AddDefaultTokenProviders();

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            @this.Configure<IdentityOptions>(options => {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            @this.AddAuthentication(options => {
                options.DefaultScheme = OpenIddictValidationDefaults.AuthenticationScheme;
            });

            @this.AddOpenIddict()

                 // Register the OpenIddict core services.
                 .AddCore(options => {
                     // Register the Entity Framework stores and models.
                     options.UseEntityFrameworkCore()
                             .UseDbContext<JHipsterDataContext>();
                 })

                 // Register the OpenIddict server handler.
                 .AddServer(options => {
                     // Register the ASP.NET Core MVC binder used by OpenIddict.
                     // Note: if you don't call this method, you won't be able to
                     // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                     options.UseMvc();

                     // Enable the token endpoint.
                     options.EnableTokenEndpoint("/connect/token");

                     // Enable the password flow.
                     options.AllowPasswordFlow();

                     // Accept anonymous clients (i.e clients that don't send a client_id).
                     options.AcceptAnonymousClients();

                     // During development, you can disable the HTTPS requirement.
                     options.DisableHttpsRequirement();

                     options.AddEphemeralSigningKey();
                 })

                 // Register the OpenIddict validation handler.
                 // Note: the OpenIddict validation handler is only compatible with the
                 // default token format or with reference tokens and cannot be used with
                 // JWT tokens. For JWT tokens, use the Microsoft JWT bearer handler.
                 .AddValidation();

            return @this;
        }

        public static IApplicationBuilder UseApplicationSecurity(this IApplicationBuilder @this, IHostingEnvironment env)
        {
            @this.UseAuthentication();

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            @this.UseHsts();

            if (!env.IsDevelopment()) {
                @this.UseHttpsRedirection();
            }

            return @this;
        }
    }
}
