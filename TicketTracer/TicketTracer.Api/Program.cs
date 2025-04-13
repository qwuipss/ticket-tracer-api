using System.Diagnostics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Context;

namespace TicketTracer.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        Log.Logger = new LoggerConfiguration()
                     .MinimumLevel.Information()
                     .Enrich.FromLogContext()
                     .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff}] {TraceId} {Level:u3} {Message:lj} {NewLine}{Exception}")
                     .WriteTo.File(
                         path: "/home/ivan/projects/cs/ticket-tracer-api/TicketTracer/TicketTracer.Api/ticket-tracer-api-.log",
                         outputTemplate: "[{Timestamp:HH:mm:ss.fff}] {TraceId} {Level:u3} {Message:lj} {NewLine}{Exception}",
                         rollingInterval: RollingInterval.Day,
                         flushToDiskInterval: TimeSpan.FromSeconds(3),
                         fileSizeLimitBytes: 10 * 1024 * 1024,
                         retainedFileCountLimit: 2,
                         rollOnFileSizeLimit: true,
                         shared: true
                     )
                     .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Host.UseSerilog();

        builder
            .Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("ticket-tracer-api"))
            .WithTracing(
                tracing => tracing
                           .AddAspNetCoreInstrumentation()
            )
            .WithMetrics(
                metrics => metrics
                           .AddMeter("ticket-tracer-api")
                           .AddAspNetCoreInstrumentation()
                           .AddPrometheusExporter()
            );

        builder.Services.AddOpenApi("v1.0");

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.MapControllers();
        app.UseSerilogRequestLogging();
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        app.Use(
            async (context, next) =>
            {
                var traceId = Activity.Current?.TraceId.ToString() ?? "<no-trace-id>";
                using (LogContext.PushProperty("TraceId", traceId))
                {
                    await next();
                }
            }
        );

        app.UseAuthorization();

        app.Run();
    }
}