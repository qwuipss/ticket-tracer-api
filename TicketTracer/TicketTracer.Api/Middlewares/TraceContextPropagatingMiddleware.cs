using System.Diagnostics;
using Serilog.Context;

namespace TicketTracer.Api.Middlewares;

public class TraceContextPropagatingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    
    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = Activity.Current?.TraceId.ToString();
        using (LogContext.PushProperty("TraceId", traceId))
        {
            await _next(context);
        }
    }
}