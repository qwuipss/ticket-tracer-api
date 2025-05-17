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
    
    [Required]
    public TicketType Type { get; set; }

    [Required]
    public Guid BoardId { get; set; }
}