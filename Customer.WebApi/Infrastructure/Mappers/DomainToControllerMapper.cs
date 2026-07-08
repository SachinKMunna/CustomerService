using AutoMapper;
using Customer.Model.Api.V1.Response;
using DomainModel = Customer.WebApi.Domain.Model;

namespace Customer.WebApi.Infrastructure.Mappers;

/// <summary>
/// Provides mapping configuration from domain models to controller response models.
/// </summary>
public static class DomainToControllerMapper
{
    /// <summary>
    /// Configures AutoMapper mappings from Customer domain model to CustomerResponse.
    /// </summary>
    /// <param name="cfg">The AutoMapper configuration expression.</param>
    public static void MapDomainToCustomerResponse(this IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<DomainModel.Customer, CustomerResponse>();
    }
}
