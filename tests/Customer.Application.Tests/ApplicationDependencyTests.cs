using System.Reflection;

namespace Customer.Application.Tests;

/// <summary>
/// Guards the hexagon: the Application core may depend on Domain only — never on
/// Infrastructure (driven adapters) or the API (driving adapters).
/// </summary>
public class ApplicationDependencyTests
{
    [Fact]
    public void Application_references_domain_only()
    {
        var referenced = Assembly.Load("Customer.Application")
            .GetReferencedAssemblies()
            .Select(a => a.Name)
            .ToArray();

        Assert.DoesNotContain("Customer.Infrastructure", referenced);
        Assert.DoesNotContain("Customer.Api", referenced);
    }
}
