using TicketTracer.Api.Models.Common;

namespace TicketTracer.Api.Services;

internal interface IUsersService
{
    Task<UserModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}