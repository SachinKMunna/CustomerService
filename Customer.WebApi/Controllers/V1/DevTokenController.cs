using Customer.WebApi.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Customer.WebApi.Controllers.V1
{
    /// <summary>
    /// Development-only helper that mints a JWT so the API can be tested in Swagger/Postman
    /// before a real identity provider is configured. Returns 404 outside the Development
    /// environment.
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/dev-token")]
    public sealed class DevTokenController : ControllerBase
    {
        private readonly DevTokenIssuer _issuer;
        private readonly IWebHostEnvironment _environment;

        public DevTokenController(DevTokenIssuer issuer, IWebHostEnvironment environment)
        {
            _issuer = issuer;
            _environment = environment;
        }

        [HttpPost]
        public IActionResult Create([FromBody] DevTokenRequest? request)
        {
            if (!_environment.IsDevelopment())
            {
                return NotFound();
            }

            var subject = string.IsNullOrWhiteSpace(request?.Subject) ? "dev-customer" : request!.Subject!;
            var email = string.IsNullOrWhiteSpace(request?.Email) ? "dev@example.com" : request!.Email!;

            var (token, expiresIn) = _issuer.Issue(subject, email);

            return Ok(new
            {
                access_token = token,
                token_type = "Bearer",
                expires_in = expiresIn
            });
        }
    }

    /// <summary>Optional overrides for the generated development token.</summary>
    public sealed record DevTokenRequest(string? Subject, string? Email);
}
