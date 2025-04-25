namespace TicketTracer.Api.Middlewares.Extensions;

internal static class RequestLoggingMiddlewareExtensions
{
    public static void UseRequestLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
    }
}