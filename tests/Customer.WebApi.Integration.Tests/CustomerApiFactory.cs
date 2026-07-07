using Customer.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Customer.WebApi.Integration.Tests
{
    /// <summary>
    /// Boots the real WebApi host in-memory for component and integration tests.
    /// </summary>
    public sealed class CustomerApiFactory : WebApplicationFactory<Program>
    {
    }
}
