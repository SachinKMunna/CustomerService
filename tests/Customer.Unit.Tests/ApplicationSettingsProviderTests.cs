using CustomerService.WebApi.Infrastructure.Bootstrap;
using Microsoft.Extensions.Configuration;

namespace CustomerService.Unit.Tests
{
    /// <summary>
    /// Verifies configuration binding through <see cref="ApplicationSettingsProvider"/>.
    /// </summary>
    public class ApplicationSettingsProviderTests
    {
        [Fact]
        public void Reads_mongo_settings_from_configuration()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["MongoDb:ConnectionString"] = "mongodb://127.0.0.1:27017",
                    ["MongoDb:Database"] = "customerdb"
                })
                .Build();

            ISettingsProvider settings = new ApplicationSettingsProvider(configuration);

            Assert.Equal("mongodb://127.0.0.1:27017", settings.Mongo.ConnectionString);
            Assert.Equal("customerdb", settings.Mongo.Database);
        }
    }
}
