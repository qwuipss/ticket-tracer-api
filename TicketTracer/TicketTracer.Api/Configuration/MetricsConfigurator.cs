using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace TicketTracer.Api.Configuration;

public static class MetricsConfigurator
{
    public static void AddMetrics(this IServiceCollection services)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(AppOptions.ResourceName))
            .WithMetrics(
                metrics => metrics
                           .AddMeter(AppOptions.ResourceName)
                           .AddPrometheusExporter()
            );
    }
}