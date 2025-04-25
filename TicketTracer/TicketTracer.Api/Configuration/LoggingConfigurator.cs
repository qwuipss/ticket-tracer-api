using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace TicketTracer.Api.Configuration;

internal static class LoggingConfigurator
{
    public static void AddLogging()
    {
        const string logMessageTemplate = "[{Timestamp:HH:mm:ss.fff}] {TraceId} {Level:u3} [{SourceContextShortened}] {Message:lj}{NewLine}{Exception}";
        const string logFileTemplate = "ticket-tracer-api-.log";

        var logDir = Path.Combine(Environment.CurrentDirectory, "logs");
        var logFilePath = Path.Combine(logDir, logFileTemplate);

        if (!Directory.Exists(logDir))
        {
            Directory.CreateDirectory(logDir);
        }

        var loggerConfig = new LoggerConfiguration()
                           .MinimumLevel.Information()
                           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                           .Enrich.FromLogContext()
                           .Enrich.With<SourceContextShortenedEnricher>()
                           .WriteTo.Console(outputTemplate: logMessageTemplate)
                           .WriteTo.File(
                               logFilePath,
                               outputTemplate: logMessageTemplate,
                               rollingInterval: RollingInterval.Day,
                               flushToDiskInterval: TimeSpan.FromSeconds(5),
                               fileSizeLimitBytes: 16 * 1024 * 1024,
                               retainedFileCountLimit: 2,
                               rollOnFileSizeLimit: true,
                               shared: true
                           );

        Log.Logger = loggerConfig.CreateLogger();
    }

    private class SourceContextShortenedEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var sourceContext = logEvent.Properties.TryGetValue("SourceContext", out var contextValue)
                ? contextValue.ToString().AsSpan().Trim('"')
                : null;

            if (sourceContext.IsEmpty)
            {
                return;
            }

            var dotLastIndex = sourceContext.LastIndexOf('.');
            var sourceContextShortened = sourceContext[(dotLastIndex + 1)..].ToString();
            var logEventProperty = propertyFactory.CreateProperty("SourceContextShortened", sourceContextShortened);
            logEvent.AddPropertyIfAbsent(logEventProperty);
        }
    }
}