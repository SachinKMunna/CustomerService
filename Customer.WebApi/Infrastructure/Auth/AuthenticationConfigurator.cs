using System.Text;
using Customer.WebApi.Domain.Security;
using Customer.WebApi.Infrastructure.Bootstrap;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            // IConfigureOptions resolves ISettingsProvider lazily from DI so test overrides
            // via ConfigureWebHost/ConfigureAppConfiguration are fully applied first.
            services.AddSingleton<IConfigureOptions<JwtBearerOptions>>(sp =>
            {
                var settings = sp.GetRequiredService<ISettingsProvider>();

                if (string.IsNullOrWhiteSpace(settings.Jwt.Key) ||
                    Encoding.UTF8.GetByteCount(settings.Jwt.Key) < 32)
                {
                    throw new InvalidOperationException(
                        "Jwt:Key must be at least 32 bytes (256 bits) for HS256. " +
                        "Set the key in appsettings.Development.json for local dev, " +
                        "or via an environment variable / secret store in production.");
                }

                return new ConfigureNamedOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
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
            });

            services.AddAuthorization();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentCustomer, CurrentCustomer>();
            services.AddSingleton<DevTokenIssuer>();

            return services;
        }
    }
}
