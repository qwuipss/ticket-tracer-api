using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TicketTracer.Data.Entities.Abstract;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace TicketTracer.Data.Entities;

[Table("tickets")]
[Index(nameof(BoardId), nameof(Number), IsUnique = true)]
public class TicketEntity : BaseEntity
{
    [Column("title")]
    [MaxLength(EntityConstraints.Ticket.TitleMaxLength)]
    public required string Title { get; set; }

    [Column("description")]
    [MaxLength(EntityConstraints.Ticket.DescriptionMaxLength)]
    public required string Description { get; set; }

    [Column("type")]
    public required TicketType Type { get; set; }
    
    [Column("number")]
    public required int Number { get; set; }

    [Column("board_id")]
    public required Guid BoardId { get; set; }

    [ForeignKey(nameof(BoardId))]
    public BoardEntity Board { get; set; } = null!;

    [Column("author_id")]
    public required Guid AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public UserEntity Author { get; set; } = null!;
}

public enum TicketType
{
    Epic,
    Story,
    Task,
    SubTask,
    Bug,
}