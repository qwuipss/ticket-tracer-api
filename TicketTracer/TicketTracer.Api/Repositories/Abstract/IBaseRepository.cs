namespace TicketTracer.Api.Repositories.Abstract;

internal interface IBaseRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<List<TEntity>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken);

    Task<Guid> AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task<bool> IsExistAsync(Guid id, CancellationToken cancellationToken);
}