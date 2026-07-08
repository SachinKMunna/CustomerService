using Customer.WebApi.Infrastructure.Auth;
using Customer.WebApi.Infrastructure.Bootstrap;
using Customer.WebApi.Swagger;
using Serilog;

namespace Customer.WebApi
{
    /// <summary>
    /// Application entry point and composition root.
    /// </summary>
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, config) =>
                config.ReadFrom.Configuration(context.Configuration).WriteTo.Console());

            // ISettingsProvider is registered as a lazy factory so configuration sources
            // added by WebApplicationFactory (test overrides) are fully merged before
            // settings are first read.
            builder.Services.AddSingleton<ISettingsProvider>(
                sp => new ApplicationSettingsProvider(sp.GetRequiredService<IConfiguration>()));

            builder.Services
                .AddServices()
                .AddJwtAuthentication()
                .AddApiDocumentation();
            builder.Services.AddControllers();

            WebApplication app = builder.Build();

            app.Configure();
            app.Run();
        }
    }
}
