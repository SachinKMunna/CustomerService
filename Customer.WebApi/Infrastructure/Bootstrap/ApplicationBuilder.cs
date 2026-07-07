using Customer.Model.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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

            // Single health route following SLE convention.
            app.MapHealthChecks(ApiRoutes.Health);

            return app;
        }
    }
}
