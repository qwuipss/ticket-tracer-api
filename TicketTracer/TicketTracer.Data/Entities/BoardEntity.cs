using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TicketTracer.Data.Entities.Abstract;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace TicketTracer.Data.Entities;

[Table("boards")]
[Index(nameof(Title), IsUnique = true)]
public class BoardEntity : BaseEntity
{
    [Column("title")]
    [MaxLength(EntityConstraints.Board.TitleMaxLength)]
    public required string Title { get; set; }

    [Column("project_id")]
    public required Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public ProjectEntity Project { get; set; } = null!;
}