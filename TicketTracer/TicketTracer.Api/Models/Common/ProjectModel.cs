using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TicketTracer.Api.Models.Common.Abstract;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Models.Common;

internal class ProjectModel : WithIdModel
{
    [MaybeNull]
    [MaxLength(EntityConstraints.Project.TitleMaxLength)]
    public string Title { get; init; }
}