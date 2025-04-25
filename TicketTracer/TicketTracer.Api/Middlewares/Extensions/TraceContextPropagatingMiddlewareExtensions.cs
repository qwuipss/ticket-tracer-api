namespace TicketTracer.Api.Middlewares.Extensions;

internal static class TraceContextPropagatingMiddlewareExtensions
{
    public static void UseTraceContextPropagatingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<TraceContextPropagatingMiddleware>();
    }
}