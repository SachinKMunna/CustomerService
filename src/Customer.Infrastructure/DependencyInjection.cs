using Customer.Infrastructure.Persistence.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Customer.Infrastructure;

/// <summary>
/// Composition of the driven (secondary) adapters. Called from the API composition root.
/// As later PRs add outbound ports (repositories, gateways, event publishers), their adapters
/// are registered here — the core never references these implementations directly.
/// </summary>
public static class DependencyInjection
{
    public const string MongoHealthCheckName = "mongodb";

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration
            .GetSection(MongoDbOptions.SectionName)
            .Get<MongoDbOptions>() ?? new MongoDbOptions();

        services.AddSingleton(options);
        services.AddSingleton<IMongoClient>(_ =>
        {
            var settings = MongoClientSettings.FromConnectionString(options.ConnectionString);
            // Fail fast instead of the 30s default when the server is unreachable.
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
            return new MongoClient(settings);
        });
        services.AddSingleton<MongoDbContext>();

        services.AddHealthChecks()
            .AddCheck<MongoHealthCheck>(MongoHealthCheckName, tags: ["ready"]);

        return services;
    }
}
