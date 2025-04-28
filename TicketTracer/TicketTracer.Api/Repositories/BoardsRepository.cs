using Microsoft.EntityFrameworkCore;
using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Repositories;

internal class BoardsRepository(TicketTracerDbContext dbContext)
    : BaseRepository<BoardEntity>(dbContext), IBoardsRepository
{
    public async Task<bool> IsExistAsync(string title, CancellationToken cancellationToken)
    {
        return await DbContext.Boards.AnyAsync(p => p.Title == title, cancellationToken);
    }

    public async Task<List<BoardEntity>> GetAllByProjectIdAsync(Guid projectId, int offset, int limit, CancellationToken cancellationToken)
    {
        return await DbContext
                     .Boards.Where(b => b.ProjectId == projectId && !b.IsDeleted)
                     .OrderBy(b => b.CreatedAt)
                     .Skip(offset)
                     .Take(limit)
                     .ToListAsync(cancellationToken);
    }
}