using System.Text.Json;
using Customer.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Structured logging.
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration)
          .WriteTo.Console());

// Driven (secondary) adapters: MongoDB + health checks.
builder.Services.AddInfrastructure(builder.Configuration);

// Driving (primary) adapter surface.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Customer Service API",
        Version = "v1",
        Description = "Customer microservice (hexagonal architecture). PR-0 foundation."
    });

    // JWT bearer definition so the Swagger "Authorize" button is ready for PR-Auth.
    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "need to Enter the JWT token issued by the auth server.",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    options.AddSecurityDefinition("Bearer", scheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement { [scheme] = Array.Empty<string>() });
});

var app = builder.Build();

// Global exception handling — one place, returns problem+json.
app.UseExceptionHandler(handler =>
{
    handler.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(new
        {
            type = "https://httpstatuses.io/500",
            title = "An unexpected error occurred.",
            status = StatusCodes.Status500InternalServerError
        });
    });
});

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Service API v1"));
}

app.UseHttpsRedirection();

// Auth middleware (JWT) is fully configured in PR-Auth. Ordering is fixed here intentionally.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Liveness: no dependencies — is the process up?
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });

// Readiness: dependencies (MongoDB ping) — can we serve traffic?
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = WriteHealthResponse
});

app.Run();

static Task WriteHealthResponse(HttpContext context, HealthReport report)
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

// Exposed for the integration test host (WebApplicationFactory<Program>).
public partial class Program { }
