using System.Text;
using Customer.WebApi.Domain.Security;
using Customer.WebApi.Infrastructure.Bootstrap;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Customer.WebApi.Infrastructure.Auth
{
    /// <summary>
    /// Configures JWT bearer authentication and the <see cref="ICurrentCustomer"/> accessor.
    /// Uses a symmetric signing key for local development; swappable for Azure AD later
    /// without touching the core.
    /// </summary>
    public static class AuthenticationConfigurator
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            ISettingsProvider settings)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = settings.Jwt.Issuer,
                        ValidateAudience = true,
                        ValidAudience = settings.Jwt.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Jwt.Key)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            services.AddAuthorization();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentCustomer, CurrentCustomer>();
            services.AddSingleton<DevTokenIssuer>();

            return services;
        }
    }
}
