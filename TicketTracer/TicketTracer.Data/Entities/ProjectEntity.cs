using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TicketTracer.Data.Entities.Abstract;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace TicketTracer.Data.Entities;

[Table("projects")]
[Index(nameof(Title), IsUnique = true)]
public class ProjectEntity : BaseEntity
{
    [Column("title")]
    [MaxLength(EntityConstraints.Project.TitleMaxLength)]
    public required string Title { get; set; }
}