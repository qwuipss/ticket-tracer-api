using System.ComponentModel.DataAnnotations;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Models.Request.Boards;

internal class CreateBoardModel
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(EntityConstraints.Board.TitleMaxLength)]
    public string Title { get; init; } = null!;

    [Required]
    public Guid ProjectId { get; init; }
}