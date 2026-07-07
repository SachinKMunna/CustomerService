using System.Security.Claims;
using Customer.WebApi.Domain.Security;
using Microsoft.AspNetCore.Http;

namespace Customer.WebApi.Infrastructure.Auth
{
    /// <summary>
    /// Resolves the current customer from the validated JWT on the HTTP context.
    /// Inbound claims are not remapped (MapInboundClaims = false), so raw JWT claim
    /// names ("sub", "email") are read directly.
    /// </summary>
    public sealed class CurrentCustomer : ICurrentCustomer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentCustomer(IHttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor;

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

        public string? ExternalAuthId => User?.FindFirst("sub")?.Value
            ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string? Email => User?.FindFirst("email")?.Value
            ?? User?.FindFirst(ClaimTypes.Email)?.Value;
    }
}
