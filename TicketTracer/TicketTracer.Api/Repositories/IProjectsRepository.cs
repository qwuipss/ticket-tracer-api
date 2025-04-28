using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Repositories;

internal interface IProjectsRepository : IBaseRepository<ProjectEntity>
{
    Task<bool> IsExistAsync(string title, CancellationToken cancellationToken);
}