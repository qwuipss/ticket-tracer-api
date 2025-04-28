using Microsoft.EntityFrameworkCore;
using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Repositories;

internal class UsersRepository(TicketTracerDbContext dbContext)
    : BaseRepository<UserEntity>(dbContext), IUsersRepository
{
    public async Task<bool> IsExistAsync(string email, CancellationToken cancellationToken)
    {
        return await DbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await DbContext.Users.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
    }
}