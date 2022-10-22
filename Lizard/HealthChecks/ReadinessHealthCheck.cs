
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Lizard.HealthChecks
{
    public class ReadinessHealthCheck : IHealthCheck
    {
        IConfiguration configuration;
        public ReadinessHealthCheck(IConfiguration _config)
        {
            configuration = _config;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(configuration["database"]);
            var db = new LizardDbContext(optionsBuilder.Options);
            var pendingMigrations = await db.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                return HealthCheckResult.Unhealthy("Migrations pending");
            return HealthCheckResult.Healthy("Ready");
        }
    }
}
