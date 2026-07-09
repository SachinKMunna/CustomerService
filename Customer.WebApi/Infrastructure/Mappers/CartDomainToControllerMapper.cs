using AutoMapper;
using Customer.Model.Api.V1.Cart;
using DomainModel = Customer.WebApi.Domain.Model;

namespace Customer.WebApi.Infrastructure.Mappers;

/// <summary>
/// Provides mapping configuration from domain models to controller cart response models.
/// </summary>
public static class CartDomainToControllerMapper
{
    /// <summary>
    /// Configures AutoMapper mappings from Cart domain model to CartResponse.
    /// </summary>
    /// <param name="cfg">The AutoMapper configuration expression.</param>
    public static void MapCartDomainToResponse(this IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<DomainModel.CartItem, CartItemResponse>();
        
        cfg.CreateMap<DomainModel.Cart, CartResponse>()
            .ForMember(response => response.CartId, options => options.MapFrom(src => src.Id));
    }
}
