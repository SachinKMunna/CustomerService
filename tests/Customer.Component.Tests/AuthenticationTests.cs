using System.Net;
using System.Net.Http.Headers;
using CustomerService.Tests.Utilities;

namespace CustomerService.Component.Tests
{
    /// <summary>
    /// Verifies JWT bearer authentication end to end against the /api/v1/me endpoint.
    /// </summary>
    public class AuthenticationTests : IClassFixture<CustomerApiFactory>
    {
        private readonly CustomerApiFactory _factory;

        public AuthenticationTests(CustomerApiFactory factory) => _factory = factory;

        [Fact]
        public async Task Me_without_token_returns_401()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/v1/me");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Me_with_valid_token_returns_identity()
        {
            var client = _factory.CreateClient();
            var token = JwtTokenFactory.CreateToken("customer-123", "customer@example.com");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/v1/me");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("customer-123", body);
        }
    }
}
