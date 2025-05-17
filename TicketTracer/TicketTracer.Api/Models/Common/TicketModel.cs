using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TicketTracer.Api.Models.Common.Abstract;
using TicketTracer.Data.Entities;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace TicketTracer.Api.Models.Common;

internal class TicketModel : WithIdModel
{
    [MaybeNull]
    [MaxLength(EntityConstraints.Ticket.TitleMaxLength)]
    public string Title { get; set; }

    [MaybeNull]
    [MaxLength(EntityConstraints.Ticket.DescriptionMaxLength)]
    public string Description { get; set; }

    [Range(0, int.MaxValue)]
    public int Number { get; set; }

    public TicketType Type { get; set; }

    public Guid BoardId { get; set; }

    public Guid AuthorId { get; set; }
}