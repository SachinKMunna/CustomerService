using System.Security.Claims;
using Customer.WebApi.Infrastructure.Auth;
using Microsoft.AspNetCore.Http;

namespace Customer.Unit.Tests
{
    /// <summary>
    /// Verifies the current-customer accessor reads identity from the HTTP context claims.
    /// </summary>
    public class CurrentCustomerTests
    {
        [Fact]
        public void Reads_identity_from_authenticated_principal()
        {
            var claims = new List<Claim>
            {
                new("sub", "customer-123"),
                new("email", "customer@example.com")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "Test"));
            var accessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            var currentCustomer = new CurrentCustomer(accessor);

            Assert.True(currentCustomer.IsAuthenticated);
            Assert.Equal("customer-123", currentCustomer.ExternalAuthId);
            Assert.Equal("customer@example.com", currentCustomer.Email);
        }

        [Fact]
        public void Reports_not_authenticated_when_no_identity()
        {
            var accessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };

            var currentCustomer = new CurrentCustomer(accessor);

            Assert.False(currentCustomer.IsAuthenticated);
            Assert.Null(currentCustomer.ExternalAuthId);
        }
    }
}
