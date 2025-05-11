using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketTracer.Data.Entities.Abstract;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace TicketTracer.Data.Entities;

[Table("attribute_values")]
public class AttributeValueEntity : BaseEntity
{
    [MaxLength(EntityConstraints.AttributeValue.ValueMaxLength)]
    [Column("value")]
    public required string Value { get; set; }

    [Column("attribute_id")]
    public required Guid AttributeId { get; set; }

    [ForeignKey(nameof(AttributeId))]
    public AttributeEntity Attribute { get; set; } = null!;

    [Column("ticket_id")]
    public required Guid TicketId { get; set; }

    [ForeignKey(nameof(TicketId))]
    public TicketEntity Ticket { get; set; } = null!;
}