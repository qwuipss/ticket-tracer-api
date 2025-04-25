using Microsoft.EntityFrameworkCore;
using TicketTracer.Data;

namespace TicketTracer.Api.Configuration;

internal static class DbContextConfigurator
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TicketTracerDbContext>(
            options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Postgres"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        );
    }
}