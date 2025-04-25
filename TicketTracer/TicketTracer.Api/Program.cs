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

        builder.Services.AddOpenApi("v1");
        builder.Services.AddAuthorization();
        builder.Services.AddSentry(builder.Configuration);
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddRepositories();
        builder.Services.AddUtilities();
        builder.Services.AddServices();
        builder.Services.AddDbContext(builder.Configuration);
        builder.Services.AddAuth();
        ControllersConfigurator.AddControllers(builder.Services);
        MetricsConfigurator.AddMetrics(builder.Services);
        LoggingConfigurator.AddLogging();
        CorsConfigurator.AddCors(builder.Services);

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        var app = builder.Build();

        EnsureCreatedDatabase(app);
        
        app.MapOpenApi();
        app.MapControllers();

        // app.UseCors("FrontendClient");
        app.UseExceptionLoggingMiddleware();
        app.UseTraceContextPropagatingMiddleware();
        app.UseRequestLogging();
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        app.UseAuthorization();

        app.Run();
    }

    private static void EnsureCreatedDatabase(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TicketTracerDbContext>();
        dbContext.Database.EnsureCreated();
    }
}