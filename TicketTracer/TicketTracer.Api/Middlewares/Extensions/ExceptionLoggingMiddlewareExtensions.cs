namespace TicketTracer.Api.Middlewares.Extensions;

public static class ExceptionLoggingMiddlewareExtensions
{
    public static void UseExceptionLoggingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionLoggingMiddleware>();
    }
}