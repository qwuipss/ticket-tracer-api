using System.Diagnostics;
using System.Net.Mime;
using TicketTracer.Api.Models.Response;
using TicketTracer.Api.Utilities;

namespace TicketTracer.Api.Middlewares;

internal class ExceptionLoggingMiddleware(RequestDelegate next, ISentry sentry, ILogger<ExceptionLoggingMiddleware> logger)
{
    private readonly ILogger<ExceptionLoggingMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;
    private readonly ISentry _sentry = sentry;

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
                await _sentry.ReportAsync(exc);
            }
            catch (Exception reportExc)
            {
                _logger.LogError(reportExc, "An error occured during Sentry exception report:");
            }

            context.Response.StatusCode = 500;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var response = new UnhandledExceptionModel
            {
                TraceId = Activity.Current!.TraceId.ToString(),
                Message = "An error occured during request execution",
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}