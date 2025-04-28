using Serilog;
using TicketTracer.Api.Configuration;
using TicketTracer.Api.Middlewares.Extensions;

namespace TicketTracer.Api;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddSentry(builder.Configuration);
        builder.Services.AddMapper();
        builder.Services.AddRepositories();
        builder.Services.AddUtilities();
        builder.Services.AddServices();
        builder.Services.AddDbContext(builder.Configuration);
        builder.Services.AddAuth();
        builder.Services.AddOpenApi("v1");
        builder.Services.AddCors(builder.Configuration);
        ControllersConfigurator.AddControllers(builder.Services);
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
        app.UseCors(CorsPolicies.FrontendClient);
        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();
    }
}