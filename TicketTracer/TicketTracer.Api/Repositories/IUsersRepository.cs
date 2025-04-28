using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Repositories;

internal interface IUsersRepository : IBaseRepository<UserEntity>
{
    Task<bool> IsExistAsync(string email, CancellationToken cancellationToken);

    Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}