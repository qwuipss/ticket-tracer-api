using System.ComponentModel.DataAnnotations;

namespace TicketTracer.Api.Models.Common.Abstract;

internal abstract class WithIdModel
{
    [Required]
    public required Guid Id { get; init; }
}