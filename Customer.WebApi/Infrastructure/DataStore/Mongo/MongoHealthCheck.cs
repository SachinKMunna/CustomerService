using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;

namespace CustomerService.WebApi.Infrastructure.DataStore.Mongo
{
    /// <summary>
    /// Health check that pings MongoDB so /health/ready reflects real database connectivity.
    /// </summary>
    public sealed class MongoHealthCheck : IHealthCheck
    {
        private readonly MongoDbContext _context;

        public MongoHealthCheck(MongoDbContext context) => _context = context;

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Database.RunCommandAsync<BsonDocument>(
                    new BsonDocument("ping", 1),
                    cancellationToken: cancellationToken);

                return HealthCheckResult.Healthy("MongoDB is reachable.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("MongoDB is unreachable.", ex);
            }
        }
    }
}
