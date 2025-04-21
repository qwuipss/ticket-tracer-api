namespace TicketTracer.Api.Middlewares.Extensions;

public static class RequestLoggingMiddlewareExtensions
{
    public static void UseRequestLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
    }
}