using System.ComponentModel.DataAnnotations;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Models.Request.Projects;

internal class CreateProjectModel
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(EntityConstraints.Project.TitleMaxLength)]
    public string Title { get; init; } = null!;
}