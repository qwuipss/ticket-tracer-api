namespace TicketTracer.Api.Repositories.Abstract;

internal interface IBaseRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(Guid id);
}