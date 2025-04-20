namespace TicketTracer.Api.Middlewares.Extensions;

public static class TraceContextPropagatingMiddlewareExtensions
{
    public static void UseTraceContextPropagatingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<TraceContextPropagatingMiddleware>();
    }
}