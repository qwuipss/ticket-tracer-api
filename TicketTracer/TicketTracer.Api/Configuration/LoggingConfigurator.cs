using Serilog;
using Serilog.Events;

namespace TicketTracer.Api.Configuration;

public static class LoggingConfigurator
{
    public static void AddLogging(this IServiceCollection services)
    {
        const string logTemplate = "[{Timestamp:HH:mm:ss.fff}] {TraceId} {Level:u3} {Message:lj} {NewLine}{Exception}";

        var logPath = Path.Combine(Environment.CurrentDirectory, "logs", "ticket-tracer-api-.log");
        var loggerConfig = new LoggerConfiguration()
                           .MinimumLevel.Information()
                           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                           .Enrich.FromLogContext()
                           .WriteTo.Console(outputTemplate: logTemplate)
                           .WriteTo.File(
                               path: logPath,
                               outputTemplate: logTemplate,
                               rollingInterval: RollingInterval.Day,
                               flushToDiskInterval: TimeSpan.FromSeconds(5),
                               fileSizeLimitBytes: 16 * 1024 * 1024,
                               retainedFileCountLimit: 2,
                               rollOnFileSizeLimit: true,
                               shared: true
                           );

        Log.Logger = loggerConfig.CreateLogger();
    }
}