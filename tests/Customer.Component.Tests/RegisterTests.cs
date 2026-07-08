using System.Net;
using System.Text;
using System.Text.Json;
using Customer.Model.Api.V1.Request;
using Customer.Tests.Utilities;

namespace Customer.Component.Tests
{
    /// <summary>
    /// Verifies customer registration end to end against the /api/v1/register endpoint.
    /// </summary>
    public class RegisterTests : IClassFixture<CustomerApiFactory>
    {
        private readonly CustomerApiFactory _factory;

        public RegisterTests(CustomerApiFactory factory) => _factory = factory;

        [Fact]
        public async Task Register_with_valid_data_returns_201()
        {
            var client = _factory.CreateClient();
            var request = new RegisterCustomerRequest
            {
                Email = "newcustomer@example.com",
                Name = "John Doe",
                Phone = "+1234567890"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/register", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("newcustomer@example.com", body);
            Assert.Contains("John Doe", body);
            Assert.Contains("+1234567890", body);
        }

        [Fact]
        public async Task Register_missing_name_returns_400()
        {
            var client = _factory.CreateClient();
            var request = new RegisterCustomerRequest
            {
                Email = "newcustomer@example.com",
                Name = "", // required
                Phone = "+1234567890"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/register", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Register_missing_phone_returns_400()
        {
            var client = _factory.CreateClient();
            var request = new RegisterCustomerRequest
            {
                Email = "newcustomer@example.com",
                Name = "John Doe",
                Phone = "" // required
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/register", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Register_with_optional_email_succeeds()
        {
            var client = _factory.CreateClient();
            var request = new RegisterCustomerRequest
            {
                Email = null, // optional
                Name = "Jane Doe",
                Phone = "+0987654321"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/register", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Jane Doe", body);
            Assert.Contains("+0987654321", body);
        }
    }
}
