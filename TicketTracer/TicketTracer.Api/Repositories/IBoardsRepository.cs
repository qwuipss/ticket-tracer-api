using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Repositories;

internal interface IBoardsRepository : IBaseRepository<BoardEntity>
{
    Task<bool> IsExistAsync(string title, CancellationToken cancellationToken);

    Task<List<BoardEntity>> GetAllByProjectIdAsync(Guid projectId, int offset, int limit, CancellationToken cancellationToken);
}