using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Customer.Api.IntegrationTests;

/// <summary>
/// PR-0 smoke test: the host boots and the liveness endpoint responds without external dependencies.
/// </summary>
public class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthEndpointTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Liveness_endpoint_returns_ok()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health/live");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
