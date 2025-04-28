using Microsoft.EntityFrameworkCore;
using TicketTracer.Data;
using TicketTracer.Data.Entities.Abstract;

namespace TicketTracer.Api.Repositories.Abstract;

internal abstract class BaseRepository<TEntity>(TicketTracerDbContext dbContext) : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly TicketTracerDbContext DbContext = dbContext;

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Set<TEntity>().Where(e => e.Id == id && !e.IsDeleted).SingleAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        return await DbContext
                     .Set<TEntity>()
                     .OrderBy(p => p.CreatedAt)
                     .Skip(offset)
                     .Take(limit)
                     .ToListAsync(cancellationToken);
    }

    public async Task<Guid> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var entry = await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return entry.Entity.Id;
    }

    public async Task<bool> IsExistAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Set<TEntity>().Where(e => e.Id == id && !e.IsDeleted).AnyAsync(cancellationToken);
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}