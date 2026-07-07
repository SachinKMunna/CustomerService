using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CustomerService.WebApi.Swagger
{
    /// <summary>
    /// Configures Swagger/OpenAPI, including the JWT bearer scheme so the
    /// "Authorize" button is ready for the auth PR.
    /// </summary>
    public static class SwaggerConfigurator
    {
        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Customer Service API",
                    Version = "v1",
                    Description = "Customer microservice (hexagonal architecture). PR-0 foundation."
                });

                var scheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter the JWT token issued by the auth server.",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                options.AddSecurityDefinition("Bearer", scheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { [scheme] = Array.Empty<string>() });
            });

            return services;
        }
    }
}
