using MongoDB.Driver;

namespace Customer.Infrastructure.Persistence.Mongo;

/// <summary>
/// Thin wrapper over the Mongo database handle. Repositories (added in later PRs) depend on this
/// to obtain their collections. Registered as a singleton in the composition root.
/// </summary>
public sealed class MongoDbContext
{
    public MongoDbContext(IMongoClient client, MongoDbOptions options)
    {
        Database = client.GetDatabase(options.Database);
    }

    public IMongoDatabase Database { get; }

    public IMongoCollection<TDocument> Collection<TDocument>(string name)
        => Database.GetCollection<TDocument>(name);
}
