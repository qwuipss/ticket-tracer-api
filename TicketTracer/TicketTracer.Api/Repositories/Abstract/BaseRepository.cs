using TicketTracer.Data;
using TicketTracer.Data.Models.Abstract;

namespace TicketTracer.Api.Repositories.Abstract;

internal abstract class BaseRepository<TEntity>(TicketTracerDbContext dbContext, ILogger logger) : IBaseRepository<TEntity>
    where TEntity : BaseDbo
{
    protected readonly TicketTracerDbContext DbContext = dbContext;

    private readonly ILogger _logger = logger;

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        var entity = await DbContext.Set<TEntity>().FindAsync(id);

        if (entity?.IsDeleted is false)
        {
            return entity;
        }

        _logger.LogWarning("Entity with specified id ({id}) was found but marked as deleted", id);

        return null;
    }
}