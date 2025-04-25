namespace TicketTracer.Api.Models.Response.Abstract;

internal abstract class ConflictModel
{
    public required string ConflictFieldName { get; init; }
}