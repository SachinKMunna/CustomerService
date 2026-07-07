using System.Net;
using CustomerService.Tests.Utilities;

namespace CustomerService.Component.Tests
{
    /// <summary>
    /// PR-0 smoke test: the host boots and liveness responds without external dependencies.
    /// </summary>
    public class HealthEndpointTests : IClassFixture<CustomerApiFactory>
    {
        private readonly CustomerApiFactory _factory;

        public HealthEndpointTests(CustomerApiFactory factory) => _factory = factory;

        [Fact]
        public async Task Liveness_endpoint_returns_ok()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/health/live");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
