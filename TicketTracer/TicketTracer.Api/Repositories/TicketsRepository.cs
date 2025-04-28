using Microsoft.EntityFrameworkCore;
using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Repositories;

internal class TicketsRepository(TicketTracerDbContext dbContext) : BaseRepository<TicketEntity>(dbContext), ITicketsRepository
{
    public async Task<int> GetLastTicketNumberAsync(Guid boardId, CancellationToken cancellationToken)
    {
        try
        {
            return await DbContext.Tickets.Where(t => t.BoardId == boardId).MaxAsync(ticket => ticket.Number, cancellationToken);
        }
        catch (InvalidOperationException)
        {
            return 0;
        }
    }

    public async Task<List<TicketEntity>> GetAllByBoardIdAsync(Guid boardId, int offset, int limit, CancellationToken cancellationToken)
    {
        return await DbContext
                     .Tickets.Where(t => t.BoardId == boardId && !t.IsDeleted)
                     .OrderByDescending(ticket => ticket.CreatedAt)
                     .Skip(offset)
                     .Take(limit)
                     .ToListAsync(cancellationToken);
    }
}