using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using TicketTracer.Api.Services;

namespace TicketTracer.Api.Middlewares;

public class ExceptionLoggingMiddleware(RequestDelegate next, ISentryService sentryService, ILogger<ExceptionLoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ISentryService _sentryService = sentryService;
    private readonly ILogger<ExceptionLoggingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, "An unhandled error occurred:");

            try
            {
                await _sentryService.ReportAsync(exc);
            }
            catch (Exception reportExc)
            {
                _logger.LogError(reportExc, "An error occured during Sentry exception report:");
            }

            context.Response.StatusCode = 500;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var response = new
            {
                TraceId = Activity.Current!.TraceId.ToString(),
                Message = "An error occured during request execution",
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}