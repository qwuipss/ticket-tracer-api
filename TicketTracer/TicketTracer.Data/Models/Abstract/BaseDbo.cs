// ReSharper disable PropertyCanBeMadeInitOnly.Global

using Microsoft.EntityFrameworkCore;

namespace TicketTracer.Data.Models.Abstract;

[PrimaryKey(nameof(Id))]
public abstract class BaseDbo
{
    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }
}