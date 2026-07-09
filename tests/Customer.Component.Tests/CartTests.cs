using System.Net;
using System.Text;
using System.Text.Json;
using Customer.Model.Api.V1.Cart;
using Customer.Tests.Utilities;
using Xunit;

namespace Customer.Component.Tests;

/// <summary>
/// Verifies shopping cart operations end to end against the /api/v1/cart/{email} endpoints.
/// </summary>
public class CartTests : IClassFixture<CustomerApiFactory>
{
    private readonly CustomerApiFactory _factory;
    private const string CustomerEmail = "cart-test@example.com";

    public CartTests(CustomerApiFactory factory)
    {
        _factory = factory;
    }

    private string GenerateToken(string email = CustomerEmail)
        => JwtTokenFactory.CreateToken(email, email);

    private HttpRequestMessage CreateAuthorizedRequest(HttpMethod method, string uri, string token)
    {
        var request = new HttpRequestMessage(method, uri);
        request.Headers.Add("Authorization", $"Bearer {token}");
        return request;
    }

    [Fact]
    public async Task AddItem_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var token = GenerateToken();
        var request = new AddCartItemRequest
        {
            ProductId = "prod-001",
            ProductName = "Widget",
            Quantity = 2,
            Price = 19.99m
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var httpRequest = CreateAuthorizedRequest(HttpMethod.Post, $"/api/v1/cart/{CustomerEmail}/items", token);
        httpRequest.Content = content;

        // Act
        var response = await client.SendAsync(httpRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("prod-001", body);
        Assert.Contains("Widget", body);
        Assert.Contains("19.99", body);
    }

    [Fact]
    public async Task AddItem_WithMissingProductId_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var token = GenerateToken();
        var request = new AddCartItemRequest
        {
            ProductId = "",
            ProductName = "Widget",
            Quantity = 2,
            Price = 19.99m
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var httpRequest = CreateAuthorizedRequest(HttpMethod.Post, $"/api/v1/cart/{CustomerEmail}/items", token);
        httpRequest.Content = content;

        // Act
        var response = await client.SendAsync(httpRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddItem_WithInvalidQuantity_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var token = GenerateToken();
        var request = new AddCartItemRequest
        {
            ProductId = "prod-001",
            ProductName = "Widget",
            Quantity = 0,
            Price = 19.99m
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var httpRequest = CreateAuthorizedRequest(HttpMethod.Post, $"/api/v1/cart/{CustomerEmail}/items", token);
        httpRequest.Content = content;

        // Act
        var response = await client.SendAsync(httpRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddItem_WithoutAuthorizationHeader_ReturnsUnauthorized()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new AddCartItemRequest
        {
            ProductId = "prod-001",
            ProductName = "Widget",
            Quantity = 2,
            Price = 19.99m
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync($"/api/v1/cart/{CustomerEmail}/items", content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AddItem_WithWrongEmail_ReturnsForbidden()
    {
        // Arrange
        var client = _factory.CreateClient();
        var token = GenerateToken();
        var request = new AddCartItemRequest
        {
            ProductId = "prod-001",
            ProductName = "Widget",
            Quantity = 2,
            Price = 19.99m
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var httpRequest = CreateAuthorizedRequest(HttpMethod.Post, "/api/v1/cart/different@example.com/items", token);
        httpRequest.Content = content;

        // Act
        var response = await client.SendAsync(httpRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task RemoveItem_WithValidProductId_ReturnsOk()
    {
        // Arrange - Add an item first
        var client = _factory.CreateClient();
        var token = GenerateToken();
        var addRequest = new AddCartItemRequest
        {
            ProductId = "prod-001",
            ProductName = "Widget",
            Quantity = 2,
            Price = 19.99m
        };

        var addContent = new StringContent(
            JsonSerializer.Serialize(addRequest),
            Encoding.UTF8,
            "application/json");

        var addHttpRequest = CreateAuthorizedRequest(HttpMethod.Post, $"/api/v1/cart/{CustomerEmail}/items", token);
        addHttpRequest.Content = addContent;

        await client.SendAsync(addHttpRequest);

        // Act - Remove the item
        var removeRequest = CreateAuthorizedRequest(HttpMethod.Delete, $"/api/v1/cart/{CustomerEmail}/items/prod-001", token);
        var response = await client.SendAsync(removeRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("items", body);
    }

    [Fact]
    public async Task RemoveItem_WithoutAuthorizationHeader_ReturnsUnauthorized()
    {
        // Act
        var client = _factory.CreateClient();
        var response = await client.DeleteAsync($"/api/v1/cart/{CustomerEmail}/items/prod-001");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RemoveItem_WithWrongEmail_ReturnsForbidden()
    {
        // Arrange
        var client = _factory.CreateClient();
        var token = GenerateToken();

        // Act
        var request = CreateAuthorizedRequest(HttpMethod.Delete, "/api/v1/cart/different@example.com/items/prod-001", token);
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task AddMultipleItems_ReturnsCartWithAllItems()
    {
        // Arrange
        var client = _factory.CreateClient();
        var token = GenerateToken();

        var item1 = new AddCartItemRequest
        {
            ProductId = "prod-001",
            ProductName = "Widget",
            Quantity = 2,
            Price = 19.99m
        };

        var item2 = new AddCartItemRequest
        {
            ProductId = "prod-002",
            ProductName = "Gadget",
            Quantity = 1,
            Price = 29.99m
        };

        // Act - Add first item
        var content1 = new StringContent(
            JsonSerializer.Serialize(item1),
            Encoding.UTF8,
            "application/json");

        var request1 = CreateAuthorizedRequest(HttpMethod.Post, $"/api/v1/cart/{CustomerEmail}/items", token);
        request1.Content = content1;

        await client.SendAsync(request1);

        // Act - Add second item
        var content2 = new StringContent(
            JsonSerializer.Serialize(item2),
            Encoding.UTF8,
            "application/json");

        var request2 = CreateAuthorizedRequest(HttpMethod.Post, $"/api/v1/cart/{CustomerEmail}/items", token);
        request2.Content = content2;

        var response = await client.SendAsync(request2);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("prod-001", body);
        Assert.Contains("prod-002", body);
    }
}

