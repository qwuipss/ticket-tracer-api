using Serilog;
using TicketTracer.Api.Configuration;
using TicketTracer.Api.Middlewares.Extensions;

namespace TicketTracer.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi("v1.0");
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddSentry(builder.Configuration);
        MetricsConfigurator.AddMetrics(builder.Services);

        LoggingConfigurator.AddLogging();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        var app = builder.Build();

        app.MapOpenApi();
        app.MapControllers();

        app.UseExceptionLoggingMiddleware();
        app.UseTraceContextPropagatingMiddleware();
        app.UseRequestLogging();
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        app.UseAuthorization();

        app.Run();
    }
}