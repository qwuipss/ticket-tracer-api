using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TicketTracer.Api.Configuration;
using TicketTracer.Api.Middlewares.Extensions;
using TicketTracer.Api.Services;

namespace TicketTracer.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi("v1.0");
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        
        builder.Services.AddSingleton<ISentryService, SentryService>();
        
        MetricsConfigurator.AddMetrics(builder.Services);
        
        LoggingConfigurator.AddLogging(builder.Services);
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.MapControllers();
        
        app.UseExceptionLoggingMiddleware();
        app.UseTraceContextPropagatingMiddleware();
        // app.UseSerilogRequestLogging();
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        app.UseAuthorization();
        
        app.Run();
    }
}