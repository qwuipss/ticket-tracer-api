using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TicketTracer.Data.Entities.Abstract;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace TicketTracer.Data.Entities;

[Table("attributes")]
[Index(nameof(BoardId), nameof(Name), IsUnique = true)]
public class AttributeEntity : BaseEntity
{
    [MaxLength(EntityConstraints.Attribute.NameMaxLength)]
    [Column("name")]
    public required string Name { get; set; } = null!;

    [Column("type")]
    public required AttributeType Type { get; set; }

    [Column("board_id")]
    public required Guid BoardId { get; set; }

    [ForeignKey(nameof(BoardId))]
    public BoardEntity Board { get; set; } = null!;
}

public enum AttributeType
{
    User = 1 << 0,
    TicketStage = 1 << 1,
}

public enum TicketStage
{
    Rejected = 1 << 0,
    Backlog = 1 << 1,
    ToDo = 1 << 2,
    Doing = 1 << 3,
    Review = 1 << 4,
    Testing = 1 << 5,
    Done = 1 << 6,
}