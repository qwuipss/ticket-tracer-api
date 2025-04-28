using System.Diagnostics;
using System.Diagnostics.Metrics;
using TicketTracer.Api.Helpers;

namespace TicketTracer.Api.Middlewares;

internal class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    private static readonly Histogram<double> RequestDurationMsHistogram = new Meter(AppOptions.ResourceName).CreateHistogram<double>("request_duration_ms");
    private static readonly Counter<int> RequestsCount = new Meter(AppOptions.ResourceName).CreateCounter<int>("requests_count");

    private readonly ILogger<RequestLoggingMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var path = $"{request.Path.Value}{request.QueryString.Value}";

        _logger.LogInformation(
            "Request started: {protocol} {method} {path}",
            request.Protocol,
            request.Method,
            path
        );

        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        var elapsedMs = stopwatch.Elapsed.TotalMilliseconds;

        _logger.LogInformation(
            "Request finished: {protocol} {method} {path}. Code: {code}. {elapsedMs}ms",
            request.Protocol,
            request.Method,
            path,
            context.Response.StatusCode,
            elapsedMs
        );

        var controller = context.GetRouteValue("controller")?.ToString();
        var action = context.GetRouteValue("action")?.ToString();

        if (controller is not null && action is not null)
        {
            RecordRequestMetrics(elapsedMs, controller, action);
        }
    }

    private static void RecordRequestMetrics(double elapsedMs, string controller, string action)
    {
        RequestDurationMsHistogram.Record(
            elapsedMs,
            MetricsHelper.CreateTag("controller", controller),
            MetricsHelper.CreateTag("action", action)
        );

        RequestsCount.Add(
            1,
            MetricsHelper.CreateTag("controller", controller),
            MetricsHelper.CreateTag("action", action)
        );
    }
}