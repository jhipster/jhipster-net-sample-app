namespace JHipsterNetSampleApplication.Test.Web.Identity {
    using FluentAssertions;
    using FluentAssertions.Json;
    using JHipsterNetSampleApplication.Domain.Identity;
    using JHipsterNetSampleApplication.Test.Setup;
    using JHipsterNetSampleApplication.Test.Web.Rest;
    using Microsoft.AspNetCore.Identity;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xunit;

    public class AuthorizationControllerIntTest {
        public AuthorizationControllerIntTest()
        {
            _factory = new NhipsterWebApplicationFactory<JHipsterStartup>();
            _client = _factory.CreateClient();

            _userManager = _factory.GetRequiredService<UserManager<User>>();
            _passwordHasher = _factory.GetRequiredService<IPasswordHasher<User>>();
        }

        private readonly NhipsterWebApplicationFactory<JHipsterStartup> _factory;
        private readonly HttpClient _client;

        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        [Fact]
        public async Task TestAuthorize()
        {
            var user = new User {
                UserName = "user-jwt-controller",
                Email = "user-jwt-controller@example.com",
                PasswordHash = _passwordHasher.HashPassword(null, "test"),
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(user);

            var login = new Dictionary<string, string> {
                { "grant_type", "password" },
                { "username", "user-jwt-controller" },
                { "password", "test" }
            };

            var response = await _client.PostAsync("/connect/token", TestUtil.ToFormUrlEncodedContent(login));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.Should().HaveElement("access_token");
            json.SelectToken("$.access_token").Value<string>().Should().NotBeEmpty();
        }

        [Fact]
        public async Task TestAuthorizeFails()
        {
            var login = new Dictionary<string, string> {
                { "grant_type", "password" },
                { "username", "wrong-user" },
                { "password", "wrong-password" }
            };

            var response = await _client.PostAsync("/connect/token", TestUtil.ToFormUrlEncodedContent(login));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
