using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data.Models;

namespace TicketTracer.Api.Repositories;

internal interface IUsersRepository : IBaseRepository<UserDbo>
{
    Task<Guid> AddUserAsync(UserDbo dbo);
    Task<bool> IsUserExistAsync(string email);
    Task<UserDbo?> GetUserByEmailAsync(string email);
}