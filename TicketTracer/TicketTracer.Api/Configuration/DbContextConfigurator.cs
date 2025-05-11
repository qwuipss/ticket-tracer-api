using Microsoft.EntityFrameworkCore;
using TicketTracer.Data;

namespace TicketTracer.Api.Configuration;

internal static class DbContextConfigurator
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TicketTracerDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(
                    configuration.GetConnectionString("Postgres")
                    ?? throw new ArgumentException(
                        "Database connection string is missing"
                    ),
                    builder => builder.MigrationsAssembly(typeof(TicketTracerDbContext).Assembly.FullName)
                );
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        );
    }
}