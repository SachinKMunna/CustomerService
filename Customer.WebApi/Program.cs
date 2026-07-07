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

            ISettingsProvider settings = new ApplicationSettingsProvider(builder.Configuration);

            builder.Host.UseSerilog((context, config) =>
                config.ReadFrom.Configuration(context.Configuration).WriteTo.Console());

            builder.Services
                .AddServices(settings)
                .AddJwtAuthentication(settings)
                .AddApiDocumentation();
            builder.Services.AddControllers();

            WebApplication app = builder.Build();

            app.Configure();
            app.Run();
        }
    }
}
