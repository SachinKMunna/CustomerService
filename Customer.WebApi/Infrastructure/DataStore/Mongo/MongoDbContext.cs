using Customer.WebApi.Infrastructure.Bootstrap;
using MongoDB.Driver;

namespace Customer.WebApi.Infrastructure.DataStore.Mongo
{
    /// <summary>
    /// Thin wrapper over the Mongo database handle. Data providers
    /// depend on this to obtain their collections.
    /// </summary>
    public sealed class MongoDbContext
    {
        public MongoDbContext(IMongoClient client, ISettingsProvider settings)
        {
            Database = client.GetDatabase(settings.Mongo.Database);
        }

        public IMongoDatabase Database { get; }

        public IMongoCollection<TDocument> Collection<TDocument>(string name)
            => Database.GetCollection<TDocument>(name);
    }
}
