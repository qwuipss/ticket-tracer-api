namespace TicketTracer.Api.Middlewares.Extensions;

internal static class ExceptionLoggingMiddlewareExtensions
{
    public static void UseExceptionLoggingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionLoggingMiddleware>();
    }
}