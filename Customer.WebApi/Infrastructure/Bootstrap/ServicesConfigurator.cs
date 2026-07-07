using Customer.WebApi.Infrastructure.DataStore.Mongo;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Customer.WebApi.Infrastructure.Bootstrap
{
    /// <summary>
    /// Registers the service's dependencies (driven adapters) on the DI container.
    /// </summary>
    public static class ServicesConfigurator
    {
        public const string MongoHealthCheckName = "mongodb";

        public static IServiceCollection AddServices(this IServiceCollection services, ISettingsProvider settings)
        {
            services.AddSingleton(settings);

            services.AddSingleton<IMongoClient>(_ =>
            {
                var clientSettings = MongoClientSettings.FromConnectionString(settings.Mongo.ConnectionString);
                // Fail fast instead of the 30s default when the server is unreachable.
                clientSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
                return new MongoClient(clientSettings);
            });

            services.AddSingleton<MongoDbContext>();

            services.AddHealthChecks()
                .AddMongoDb(
                    clientFactory: sp => sp.GetRequiredService<IMongoClient>(),
                    databaseNameFactory: _ => settings.Mongo.Database,
                    name: MongoHealthCheckName,
                    tags: ["ready"]);

            return services;
        }
    }
}
