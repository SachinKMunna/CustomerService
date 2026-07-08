using Customer.WebApi.Infrastructure.Auth;
using Customer.WebApi.Infrastructure.Bootstrap;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Customer.Unit.Tests
{
    /// <summary>
    /// Verifies that <see cref="AuthenticationConfigurator"/> fails fast when
    /// the JWT signing key is missing or too short.
    /// </summary>
    public class AuthenticationConfiguratorTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("too-short")]
        public void Throws_when_jwt_key_is_missing_or_too_short(string key)
        {
            var services = new ServiceCollection();
            services.AddSingleton<ISettingsProvider>(new StubSettings(key));
            services.AddJwtAuthentication();
            var sp = services.BuildServiceProvider();

            var ex = Assert.Throws<InvalidOperationException>(
                () => sp.GetRequiredService<IConfigureOptions<JwtBearerOptions>>());

            Assert.Contains("Jwt:Key", ex.Message);
            Assert.Contains("32 bytes", ex.Message);
        }

        [Fact]
        public void Does_not_throw_when_jwt_key_is_valid()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ISettingsProvider>(
                new StubSettings("a-valid-key-that-is-at-least-32-bytes-long!"));
            services.AddJwtAuthentication();
            var sp = services.BuildServiceProvider();

            var ex = Record.Exception(
                () => sp.GetRequiredService<IConfigureOptions<JwtBearerOptions>>());

            Assert.Null(ex);
        }

        private sealed class StubSettings : ISettingsProvider
        {
            public StubSettings(string key)
                => Jwt = new JwtSettings { Issuer = "https://test", Audience = "api", Key = key };

            public MongoSettings Mongo { get; } = new MongoSettings();
            public JwtSettings Jwt { get; }
        }
    }
}
