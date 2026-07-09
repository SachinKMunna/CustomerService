using AutoMapper;

namespace Customer.WebApi.Infrastructure.Mappers;

/// <summary>
/// Specific implementation of <see cref="IMappingCoordinator"/> for the Customer service.
/// Encapsulates AutoMapper configuration and provides a clean abstraction for mapping operations.
/// </summary>
public class MappingCoordinator : IMappingCoordinator
{
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="MappingCoordinator"/> class.
    /// </summary>
    public MappingCoordinator()
    {
        var configuration = InitializeMapping();
        _mapper = configuration.CreateMapper();
    }

    private MapperConfiguration InitializeMapping()
        => new MapperConfiguration(cfg => ConfigureMapping(cfg));

    /// <summary>
    /// Configure all mappings for the service.
    /// </summary>
    protected virtual IMapperConfigurationExpression ConfigureMapping(IMapperConfigurationExpression cfg)
    {
        // Customer: Controller to Domain mappings
        cfg.MapRegisterRequestToDomain();

        // Customer: Domain to Controller mappings
        cfg.MapDomainToCustomerResponse();

        // Cart: Controller to Domain mappings
        cfg.MapAddCartItemRequestToDomain();

        // Cart: Domain to Controller mappings
        cfg.MapCartDomainToResponse();

        // Controller to Domain mappings
        cfg.MapRegisterRequestToDomain();

        // Domain to Controller mappings
        cfg.MapDomainToCustomerResponse();

        return cfg;
    }

    /// <summary>
    /// Maps an object of type <see cref="TSource"/> to an object of type <see cref="TDestination"/>.
    /// </summary>
    public TDestination Map<TSource, TDestination>(TSource source)
        => _mapper.Map<TDestination>(source);

    /// <summary>
    /// Maps a collection of objects of type <see cref="TSource"/> to a collection of objects of type <see cref="TDestination"/>.
    /// </summary>
    public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        => _mapper.Map<IEnumerable<TDestination>>(source);
}
