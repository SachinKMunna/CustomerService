using Customer.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Customer.Tests.Utilities
{
    /// <summary>
    /// Boots the real WebApi host in-memory for component and integration tests.
    /// Supplies deterministic JWT settings (via environment variables, read by the host
    /// at build time) so minted test tokens validate.
    /// </summary>
    public sealed class CustomerApiFactory : WebApplicationFactory<Program>
    {
        public CustomerApiFactory()
        {
            Environment.SetEnvironmentVariable("Jwt__Issuer", TestAuth.Issuer);
            Environment.SetEnvironmentVariable("Jwt__Audience", TestAuth.Audience);
            Environment.SetEnvironmentVariable("Jwt__Key", TestAuth.Key);
        }
    }
}
