using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Repositories;

internal interface ITicketsRepository : IBaseRepository<TicketEntity>
{
    Task<int> GetLastTicketNumberAsync(Guid boardId, CancellationToken cancellationToken);

    Task<List<TicketEntity>> GetAllByBoardIdAsync(Guid boardId, int offset, int limit, CancellationToken cancellationToken);
}