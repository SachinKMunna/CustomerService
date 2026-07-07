using Microsoft.Extensions.Configuration;

namespace Customer.WebApi.Infrastructure.Bootstrap
{
    /// <summary>
    /// Reads strongly-typed settings from <see cref="IConfiguration"/>.
    /// </summary>
    public sealed class ApplicationSettingsProvider : ISettingsProvider
    {
        public ApplicationSettingsProvider(IConfiguration configuration)
        {
            Mongo = configuration.GetSection(MongoSettings.SectionName).Get<MongoSettings>() ?? new MongoSettings();
            Jwt = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>() ?? new JwtSettings();
        }

        public MongoSettings Mongo { get; }

        public JwtSettings Jwt { get; }
    }
}
