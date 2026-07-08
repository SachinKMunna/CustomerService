using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Customer.Tests.Utilities
{
    /// <summary>
    /// Mints signed JWTs for tests using the shared <see cref="TestAuth"/> values.
    /// </summary>
    public static class JwtTokenFactory
    {
        public static string CreateToken(string subject, string? email = null)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestAuth.Key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> { new("sub", subject) };
            if (email is not null)
            {
                claims.Add(new Claim("email", email));
            }

            var token = new JwtSecurityToken(
                issuer: TestAuth.Issuer,
                audience: TestAuth.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
