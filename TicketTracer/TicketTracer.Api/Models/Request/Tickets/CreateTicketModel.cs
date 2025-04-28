using System.ComponentModel.DataAnnotations;
using TicketTracer.Data.Entities;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace TicketTracer.Api.Models.Request.Tickets;

internal class CreateTicketModel
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(EntityConstraints.Ticket.TitleMaxLength)]
    public string Title { get; set; } = null!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(EntityConstraints.Ticket.DescriptionMaxLength)]
    public string Description { get; set; } = null!;

    public Guid BoardId { get; set; }
}