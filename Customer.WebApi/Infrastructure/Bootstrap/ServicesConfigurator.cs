using Customer.WebApi.DataStore;
using Customer.WebApi.Domain.Services;
using Customer.WebApi.Infrastructure.DataStore.Mongo;
using Customer.WebApi.Infrastructure.Mappers;
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

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<ISettingsProvider>();
                var clientSettings = MongoClientSettings.FromConnectionString(settings.Mongo.ConnectionString);
                // Fail fast instead of the 30s default when the server is unreachable.
                clientSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
                return new MongoClient(clientSettings);
            });

            services.AddSingleton<MongoDbContext>();

            services.AddSingleton<IMappingCoordinator, MappingCoordinator>();

            services.AddScoped<ICustomerDataProvider, MongoCustomerDataProvider>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<ICartDataProvider, MongoCartDataProvider>();
            services.AddScoped<ICartService, CartService>();

            services.AddHealthChecks()
                .AddMongoDb(
                    clientFactory: sp => sp.GetRequiredService<IMongoClient>(),
                    databaseNameFactory: sp => sp.GetRequiredService<ISettingsProvider>().Mongo.Database,
                    name: MongoHealthCheckName,
                    tags: ["ready"]);

            return services;
        }
    }
}
