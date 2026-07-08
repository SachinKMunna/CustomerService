using Customer.WebApi.DataStore;
using DomainModel = Customer.WebApi.Domain.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Customer.WebApi.Infrastructure.DataStore.Mongo;

public class MongoCustomerDataProvider : ICustomerDataProvider
{
    private readonly IMongoCollection<DomainModel.Customer> _collection;
    private const string CollectionName = "customers";

    public MongoCustomerDataProvider(MongoDbContext context)
    {
        _collection = context.Collection<DomainModel.Customer>(CollectionName);
    }

    public async Task<DomainModel.Customer> CreateAsync(DomainModel.Customer customer, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(customer);
        await _collection.InsertOneAsync(customer, cancellationToken: cancellationToken);
        return customer;
    }

    public async Task<DomainModel.Customer?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<DomainModel.Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        return await _collection.Find(c => c.Email == email).FirstOrDefaultAsync(cancellationToken);
    }
}
