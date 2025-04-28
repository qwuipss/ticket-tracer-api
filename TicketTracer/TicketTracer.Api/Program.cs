using Microsoft.EntityFrameworkCore;
using Serilog;
using TicketTracer.Api.Configuration;
using TicketTracer.Api.Middlewares.Extensions;
using TicketTracer.Data;

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
        builder.Services.AddOpenApi("document");
        builder.Services.AddCors(builder.Configuration);
        ControllersConfigurator.AddControllers(builder.Services);
        MetricsConfigurator.AddMetrics(builder.Services);
        LoggingConfigurator.AddLogging();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        var app = builder.Build();

        MigrateDatabase(app);

        app.MapOpenApi();
        app.MapControllers();

        app.UsePathBase(builder.Configuration.GetValue<string>("PathBase") ?? throw new ArgumentException("'PathBase' setting is missing"));
        app.UseExceptionLoggingMiddleware();
        app.UseTraceContextPropagatingMiddleware();
        app.UseRequestLogging();
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        app.UseCors(CorsPolicies.FrontendClient);
        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();
    }

    private static void MigrateDatabase(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TicketTracerDbContext>();
        dbContext.Database.Migrate();
    }
}