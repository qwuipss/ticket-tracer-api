using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TicketTracer.Api.Models.Common.Abstract;
using TicketTracer.Data.Entities;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace TicketTracer.Api.Models.Common;

internal class BoardModel : WithIdModel
{
    [MaybeNull]
    [MaxLength(EntityConstraints.Board.TitleMaxLength)]
    public string Title { get; set; }

    public Guid ProjectId { get; set; }
}