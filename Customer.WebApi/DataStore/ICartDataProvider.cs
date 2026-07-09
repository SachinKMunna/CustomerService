using DomainModel = Customer.WebApi.Domain.Model;

namespace Customer.WebApi.DataStore;

public interface ICartDataProvider
{
    Task<DomainModel.Cart> GetOrCreateCartAsync(string email, CancellationToken cancellationToken = default);
    Task<DomainModel.Cart> AddItemAsync(string email, DomainModel.CartItem item, CancellationToken cancellationToken = default);
    Task<DomainModel.Cart> RemoveItemAsync(string email, string productId, CancellationToken cancellationToken = default);
}
