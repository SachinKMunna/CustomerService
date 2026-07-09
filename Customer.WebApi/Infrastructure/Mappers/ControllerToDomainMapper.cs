using AutoMapper;
using Customer.Model.Api.V1.Request;
using DomainModel = Customer.WebApi.Domain.Model;

namespace Customer.WebApi.Infrastructure.Mappers;

/// <summary>
/// Provides mapping configuration from controller request models to domain models.
/// </summary>
public static class ControllerToDomainMapper
{
    /// <summary>
    /// Configures AutoMapper mappings from RegisterCustomerRequest to Customer domain model.
    /// </summary>
    /// <param name="cfg">The AutoMapper configuration expression.</param>
    public static void MapRegisterRequestToDomain(this IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<RegisterCustomerRequest, DomainModel.Customer>()
            .ForMember(dest => dest.Email, options => options.MapFrom(src => src.Email != null ? src.Email.ToLower() : null))
            .ForMember(customer => customer.Id, options => options.Ignore())
            .ForMember(customer => customer.ExternalAuthId, options => options.Ignore())
            .ForMember(customer => customer.CreatedAt, options => options.Ignore())
            .ForMember(customer => customer.UpdatedAt, options => options.Ignore())
            .ForMember(customer => customer.Email, options => options.MapFrom(src => src.Email != null ? src.Email.ToLowerInvariant() : null));
    }
}
