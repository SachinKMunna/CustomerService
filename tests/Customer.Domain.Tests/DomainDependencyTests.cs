using System.Reflection;

namespace Customer.Domain.Tests;

/// <summary>
/// Guards the hexagon: the Domain core must not depend on outer layers.
/// </summary>
public class DomainDependencyTests
{
    [Fact]
    public void Domain_does_not_reference_outer_layers()
    {
        var referenced = Assembly.Load("Customer.Domain")
            .GetReferencedAssemblies()
            .Select(a => a.Name)
            .ToArray();

        Assert.DoesNotContain("Customer.Application", referenced);
        Assert.DoesNotContain("Customer.Infrastructure", referenced);
        Assert.DoesNotContain("Customer.Api", referenced);
    }
}
