namespace Customer.WebApi.Infrastructure.Mappers;

/// <summary>
/// Abstracts AutoMapper configuration and usage to provide a clean mapping interface for the service.
/// </summary>
public interface IMappingCoordinator
{
    /// <summary>
    /// Maps an object of type <see cref="TSource"/> to an object of type <see cref="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The source object type to map from.</typeparam>
    /// <typeparam name="TDestination">The destination object type to map to.</typeparam>
    /// <param name="source">The source object instance.</param>
    /// <returns>A new instance of type <see cref="TDestination"/> mapped from <paramref name="source"/>.</returns>
    TDestination Map<TSource, TDestination>(TSource source);

    /// <summary>
    /// Maps a collection of objects of type <see cref="TSource"/> to a collection of objects of type <see cref="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The source collection element type to map from.</typeparam>
    /// <typeparam name="TDestination">The destination collection element type to map to.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <returns>A collection of new instances of type <see cref="TDestination"/> mapped from <paramref name="source"/>.</returns>
    IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);
}
