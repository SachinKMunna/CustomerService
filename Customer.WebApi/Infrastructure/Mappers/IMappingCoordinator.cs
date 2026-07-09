namespace Customer.WebApi.Infrastructure.Mappers;

/// <summary>
/// Interface providing contracts for mapping between models.
/// Abstraction layer over AutoMapper for consistent mapping operations.
/// </summary>
public interface IMappingCoordinator
{
    /// <summary>
    /// Maps an object of type <see cref="TSource"/> to an object of type <see cref="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the input object.</typeparam>
    /// <typeparam name="TDestination">The type of the output object.</typeparam>
    /// <param name="source">The input object of type <see cref="TSource"/>.</param>
    /// <returns>The output object of type <see cref="TDestination"/>.</returns>
    TDestination Map<TSource, TDestination>(TSource source);

    /// <summary>
    /// Maps a collection of objects of type <see cref="TSource"/> to a collection of objects of type <see cref="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the input objects.</typeparam>
    /// <typeparam name="TDestination">The type of the output objects.</typeparam>
    /// <param name="source">The input collection of objects of type <see cref="TSource"/>.</param>
    /// <returns>The output collection of objects of type <see cref="TDestination"/>.</returns>
    IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);
}
