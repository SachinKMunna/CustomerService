using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Customer.WebApi.Infrastructure.Bootstrap
{
    /// <summary>
    /// Configures the HTTP request pipeline and endpoints.
    /// </summary>
    public static class ApplicationBuilder
    {
        public static WebApplication Configure(this WebApplication app)
        {
            app.UseExceptionHandler(handler => handler.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(new
                {
                    type = "https://httpstatuses.io/500",
                    title = "An unexpected error occurred.",
                    status = StatusCodes.Status500InternalServerError
                });
            }));

            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Service API v1"));
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // Liveness: no dependencies. Readiness: MongoDB ping.
            app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready"),
                ResponseWriter = WriteHealthResponse
            });

            return app;
        }

        private static Task WriteHealthResponse(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";
            var payload = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description
                })
            };
            return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
