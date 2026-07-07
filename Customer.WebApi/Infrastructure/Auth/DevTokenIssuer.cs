using System.Text;
using CustomerService.WebApi.Infrastructure.Bootstrap;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CustomerService.WebApi.Infrastructure.Auth
{
    /// <summary>
    /// Issues signed JWTs using the service's own symmetric key. Intended for local
    /// development only (exposed via a Development-only endpoint) so the API can be
    /// exercised in Swagger/Postman before a real identity provider is wired in.
    /// </summary>
    public sealed class DevTokenIssuer
    {
        private readonly ISettingsProvider _settings;

        public DevTokenIssuer(ISettingsProvider settings) => _settings = settings;

        public (string Token, int ExpiresInSeconds) Issue(string subject, string? email)
        {
            const int lifetimeSeconds = 3600;

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Jwt.Key));
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _settings.Jwt.Issuer,
                Audience = _settings.Jwt.Audience,
                Expires = DateTime.UtcNow.AddSeconds(lifetimeSeconds),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                Claims = new Dictionary<string, object>
                {
                    ["sub"] = subject,
                    ["email"] = email ?? string.Empty
                }
            };

            var token = new JsonWebTokenHandler().CreateToken(descriptor);
            return (token, lifetimeSeconds);
        }
    }
}
