using Customer.WebApi.Infrastructure.Bootstrap;
using Microsoft.Extensions.Configuration;

namespace Customer.Unit.Tests
{
    /// <summary>
    /// Verifies that <see cref="ApplicationSettingsProvider"/> returns safe defaults
    /// when configuration sections are absent, rather than throwing.
    /// </summary>
    public class ApplicationSettingsProviderTests
    {
        [Fact]
        public void Returns_empty_mongo_defaults_when_section_is_missing()
        {
            var configuration = new ConfigurationBuilder().Build();

            ISettingsProvider settings = new ApplicationSettingsProvider(configuration);

            Assert.Equal(string.Empty, settings.Mongo.ConnectionString);
            Assert.Equal(string.Empty, settings.Mongo.Database);
        }

        [Fact]
        public void Returns_empty_jwt_defaults_when_section_is_missing()
        {
            var configuration = new ConfigurationBuilder().Build();

            ISettingsProvider settings = new ApplicationSettingsProvider(configuration);

            Assert.Equal(string.Empty, settings.Jwt.Issuer);
            Assert.Equal(string.Empty, settings.Jwt.Audience);
            Assert.Equal(string.Empty, settings.Jwt.Key);
        }
    }
}
