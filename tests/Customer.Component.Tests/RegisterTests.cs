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
                Name = "jerry",
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
            Assert.Contains("jerry", body);
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
                Name = "SachinMunna",
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
                Name = "Rakesh",
                Phone = "+0987654321"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/register", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Rakesh", body);
            Assert.Contains("+0987654321", body);
        }

        [Fact]
        public async Task RegisterBulk_with_multiple_customers_returns_201()
        {
            var client = _factory.CreateClient();
            var request = new RegisterCustomersRequest
            {
                Customers = new List<RegisterCustomerRequest>
                {
                    new() { Email = "customer1@example.com", Name = "Customer One", Phone = "+1111111111" },
                    new() { Email = "customer2@example.com", Name = "Customer Two", Phone = "+2222222222" },
                    new() { Email = "customer3@example.com", Name = "Customer Three", Phone = "+3333333333" }
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/register/bulk", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Customer One", body);
            Assert.Contains("Customer Two", body);
            Assert.Contains("Customer Three", body);
            Assert.Contains("\"totalRegistered\":3", body);
        }

        [Fact]
        public async Task RegisterBulk_with_empty_list_returns_400()
        {
            var client = _factory.CreateClient();
            var request = new RegisterCustomersRequest
            {
                Customers = new List<RegisterCustomerRequest>() // empty list
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/register/bulk", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RegisterBulk_with_null_customers_returns_400()
        {
            var client = _factory.CreateClient();
            var request = new RegisterCustomersRequest
            {
                Customers = null
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/register/bulk", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RegisterBulk_with_single_customer_succeeds()
        {
            var client = _factory.CreateClient();
            var request = new RegisterCustomersRequest
            {
                Customers = new List<RegisterCustomerRequest>
                {
                    new() { Email = "single@example.com", Name = "Single Customer", Phone = "+9999999999" }
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/register/bulk", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Single Customer", body);
            Assert.Contains("\"totalRegistered\":1", body);
        }

        [Fact]
        public async Task RegisterBulk_with_invalid_customer_in_list_returns_400()
        {
            var client = _factory.CreateClient();
            var request = new RegisterCustomersRequest
            {
                Customers = new List<RegisterCustomerRequest>
                {
                    new() { Email = "valid@example.com", Name = "Valid Customer", Phone = "+1111111111" },
                    new() { Email = "invalid@example.com", Name = "", Phone = "+2222222222" } // invalid: empty name
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/v1/register/bulk", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
