namespace Customer.Infrastructure.Persistence.Mongo;

/// <summary>
/// Configuration for the MongoDB connection, bound from the "MongoDb" section of configuration.
/// This is the only place the core touches persistence details — the driven (secondary) adapter side of the hexagon.
/// </summary>
public sealed class MongoDbOptions
{
    public const string SectionName = "MongoDb";

    public string ConnectionString { get; init; } = string.Empty;

    public string Database { get; init; } = string.Empty;
}
