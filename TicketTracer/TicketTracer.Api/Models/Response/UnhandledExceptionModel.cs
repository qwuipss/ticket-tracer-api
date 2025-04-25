namespace TicketTracer.Api.Models.Response;

internal class UnhandledExceptionModel
{
    public required string TraceId { get; init; } = null!;
    public required string Message { get; init; } = null!;
}