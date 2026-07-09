using Customer.WebApi.DataStore;
using MongoDB.Driver;
using DomainModel = Customer.WebApi.Domain.Model;

namespace Customer.WebApi.Infrastructure.DataStore.Mongo;

public class MongoCartDataProvider : ICartDataProvider
{
    private readonly IMongoCollection<DomainModel.Cart> _collection;
    private const string CollectionName = "carts";

    public MongoCartDataProvider(MongoDbContext context)
    {
        _collection = context.Collection<DomainModel.Cart>(CollectionName);
        EnsureIndexes();
    }

    private void EnsureIndexes()
    {
        try
        {
            var indexModel = new CreateIndexModel<DomainModel.Cart>(
                Builders<DomainModel.Cart>.IndexKeys.Ascending(c => c.Email),
                new CreateIndexOptions { Unique = true });

            _collection.Indexes.CreateOne(indexModel);
        }
        catch
        {
            // Index may already exist, ignore creation errors
        }
    }

    public async Task<DomainModel.Cart> GetOrCreateCartAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        var existingCart = await _collection.Find(c => c.Email == email).FirstOrDefaultAsync(cancellationToken);
        
        if (existingCart != null)
            return existingCart;

        var newCart = new DomainModel.Cart { Email = email };
        await _collection.InsertOneAsync(newCart, cancellationToken: cancellationToken);
        return newCart;
    }

    public async Task<DomainModel.Cart> AddItemAsync(string email, DomainModel.CartItem item, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(item);

        var update = Builders<DomainModel.Cart>.Update
            .Push(c => c.Items, item)
            .Set(c => c.UpdatedAt, DateTime.UtcNow);

        var options = new FindOneAndUpdateOptions<DomainModel.Cart> { ReturnDocument = ReturnDocument.After };
        
        var updatedCart = await _collection.FindOneAndUpdateAsync(
            c => c.Email == email,
            update,
            options,
            cancellationToken);

        return updatedCart ?? throw new InvalidOperationException($"Cart not found for email {email}");
    }

    public async Task<DomainModel.Cart> RemoveItemAsync(string email, string productId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(productId);

        var update = Builders<DomainModel.Cart>.Update
            .PullFilter(c => c.Items, item => item.ProductId == productId)
            .Set(c => c.UpdatedAt, DateTime.UtcNow);

        var options = new FindOneAndUpdateOptions<DomainModel.Cart> { ReturnDocument = ReturnDocument.After };
        
        var updatedCart = await _collection.FindOneAndUpdateAsync(
            c => c.Email == email,
            update,
            options,
            cancellationToken);

        return updatedCart ?? throw new InvalidOperationException($"Cart not found for email {email}");
    }


}
