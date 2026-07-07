namespace Customer.WebApi.Infrastructure.Bootstrap
{
    /// <summary>
    /// Abstraction over application configuration so the rest of the service does not
    /// depend on <c>IConfiguration</c> directly 
    /// </summary>
    public interface ISettingsProvider
    {
        MongoSettings Mongo { get; }

        JwtSettings Jwt { get; }
    }

    /// <summary>MongoDB connection settings, bound from the "MongoDb" configuration section.</summary>
    public sealed class MongoSettings
    {
        public const string SectionName = "MongoDb";

        public string ConnectionString { get; init; } = string.Empty;

        public string Database { get; init; } = string.Empty;
    }

    /// <summary>JWT settings, bound from the "Jwt" configuration section (used in PR-Auth).</summary>
    public sealed class JwtSettings
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; init; } = string.Empty;

        public string Audience { get; init; } = string.Empty;

        public string Key { get; init; } = string.Empty;
    }
}
