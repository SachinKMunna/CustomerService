using Customer.Model.Api.V1.Cart;
using Customer.WebApi.DataStore;
using Customer.WebApi.Infrastructure.Mappers;
using DomainModel = Customer.WebApi.Domain.Model;

namespace Customer.WebApi.Domain.Services;

public interface ICartService
{
    Task<CartResponse> AddItemAsync(string email, AddCartItemRequest request, CancellationToken cancellationToken = default);
    Task<CartResponse> RemoveItemAsync(string email, string productId, CancellationToken cancellationToken = default);
}

public class CartService : ICartService
{
    private readonly ICartDataProvider _cartDataProvider;
    private readonly IMappingCoordinator _mappingCoordinator;

    public CartService(ICartDataProvider cartDataProvider, IMappingCoordinator mappingCoordinator)
    {
        _cartDataProvider = cartDataProvider;
        _mappingCoordinator = mappingCoordinator;
    }

    public async Task<CartResponse> AddItemAsync(
        string email,
        AddCartItemRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(request);

        // Ensure cart exists before adding item
        await _cartDataProvider.GetOrCreateCartAsync(email, cancellationToken);

        var cartItem = _mappingCoordinator.Map<AddCartItemRequest, DomainModel.CartItem>(request);

        var cart = await _cartDataProvider.AddItemAsync(email, cartItem, cancellationToken);

        return _mappingCoordinator.Map<DomainModel.Cart, CartResponse>(cart);
    }

    public async Task<CartResponse> RemoveItemAsync(
        string email,
        string productId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(productId);

        var cart = await _cartDataProvider.RemoveItemAsync(email, productId, cancellationToken);

        return _mappingCoordinator.Map<DomainModel.Cart, CartResponse>(cart);
    }

}

