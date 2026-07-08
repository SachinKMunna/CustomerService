using AutoMapper;
using Customer.Model.Api.V1.Cart;
using DomainModel = Customer.WebApi.Domain.Model;

namespace Customer.WebApi.Infrastructure.Mappers;

/// <summary>
/// Provides mapping configuration from controller cart request models to domain models.
/// </summary>
public static class CartControllerToDomainMapper
{
    /// <summary>
    /// Configures AutoMapper mappings from AddCartItemRequest to CartItem domain model.
    /// </summary>
    /// <param name="cfg">The AutoMapper configuration expression.</param>
    public static void MapAddCartItemRequestToDomain(this IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<AddCartItemRequest, DomainModel.CartItem>();
    }
}
