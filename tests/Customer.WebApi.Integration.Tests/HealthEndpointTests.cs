using System.Net;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Customer.WebApi.Integration.Tests
{
    /// <summary>
    /// Component tests for the health endpoints. Verifies liveness, readiness success,
    /// and MongoDB failure conditions.
    /// </summary>
    public class HealthEndpointTests : IClassFixture<CustomerApiFactory>
    {
        private readonly CustomerApiFactory _factory;

        public HealthEndpointTests(CustomerApiFactory factory) => _factory = factory;

        [Fact]
        public async Task Liveness_endpoint_returns_ok_and_healthy()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/health/live");
            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Healthy", body);
        }

        [Fact]
        public async Task Readiness_endpoint_returns_ok_when_mongo_is_reachable()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/health/ready");
            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Healthy", body);
        }

        [Fact]
        public async Task Readiness_endpoint_returns_unhealthy_when_mongo_is_unreachable()
        {
            // Override IMongoClient with a client pointing to a port that isn't listening.
            // ServerSelectionTimeout is set to 200ms so the test fails fast.
            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMongoClient));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddSingleton<IMongoClient>(_ =>
                    {
                        var settings = MongoClientSettings.FromConnectionString("mongodb://127.0.0.1:27099");
                        settings.ServerSelectionTimeout = TimeSpan.FromMilliseconds(200);
                        return new MongoClient(settings);
                    });
                });
            });

            var client = factory.CreateClient();

            var response = await client.GetAsync("/health/ready");
            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
            Assert.Equal("Unhealthy", body);
        }
    }
}
