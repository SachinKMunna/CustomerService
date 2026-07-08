using Customer.WebApi;
using Customer.WebApi.Infrastructure.Bootstrap;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Customer.Tests.Utilities
{
    /// <summary>
    /// Boots the real WebApi host in-memory for component and integration tests.
    /// Supplies deterministic JWT settings by replacing <see cref="ISettingsProvider"/>
    /// via <c>ConfigureTestServices</c>, which is scoped to this factory instance and
    /// does not leak into other tests or the process environment.
    /// </summary>
    public sealed class CustomerApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(ISettingsProvider));
                services.AddSingleton<ISettingsProvider>(new TestSettingsProvider());
            });
        }

        private sealed class TestSettingsProvider : ISettingsProvider
        {
            public MongoSettings Mongo { get; } = new MongoSettings
            {
                ConnectionString = "mongodb://127.0.0.1:27017",
                Database = "customerdb"
            };

            public JwtSettings Jwt { get; } = new JwtSettings
            {
                Issuer = TestAuth.Issuer,
                Audience = TestAuth.Audience,
                Key = TestAuth.Key
            };
        }
    }
}
