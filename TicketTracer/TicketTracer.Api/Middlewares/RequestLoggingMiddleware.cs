using System.Diagnostics;
using System.Diagnostics.Metrics;
using TicketTracer.Api.Helpers;

namespace TicketTracer.Api.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    private static readonly Histogram<double> RequestDurationMsHistogram = new Meter(AppOptions.ResourceName).CreateHistogram<double>("request_duration_ms");
    private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var controller = context.GetRouteValue("controller")?.ToString();
        var action = context.GetRouteValue("action")?.ToString();

        var request = context.Request;
        var isApiTarget = controller is not null && action is not null;
        var path = isApiTarget ? $"{controller}/{action}" : request.Path.Value;

        _logger.LogInformation(
            "Start executing request: {method} {protocol} {path}",
            request.Method,
            request.Protocol,
            path
        );

        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        var elapsedMs = stopwatch.Elapsed.TotalMilliseconds;

        _logger.LogInformation(
            "Request finished: {method} {protocol} {path}. {elapsedMs}ms",
            request.Method,
            request.Protocol,
            path,
            elapsedMs
        );

        if (isApiTarget)
        {
            ReportRequestDuration(elapsedMs, controller!, action!);
        }
    }

    private static void ReportRequestDuration(double elapsedMs, string controller, string action)
    {
        RequestDurationMsHistogram.Record(
            elapsedMs,
            MetricsHelper.CreateTag("controller", controller),
            MetricsHelper.CreateTag("action", action)
        );
    }
}