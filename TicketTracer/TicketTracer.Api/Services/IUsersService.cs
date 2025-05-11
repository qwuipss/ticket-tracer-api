using TicketTracer.Api.Models.Common;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Services;

internal interface IUsersService
{
    Task<UserModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<List<UserEntity>?> GetAllAsync(int offset, int limit, CancellationToken cancellationToken);
}