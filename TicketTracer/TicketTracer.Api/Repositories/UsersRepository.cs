using Microsoft.EntityFrameworkCore;
using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data;
using TicketTracer.Data.Models;

namespace TicketTracer.Api.Repositories;

internal class UsersRepository(TicketTracerDbContext dbContext, ILogger<UsersRepository> logger)
    : BaseRepository<UserDbo>(dbContext, logger), IUsersRepository
{
    public async Task<Guid> AddUserAsync(UserDbo dbo)
    {
        var entry = await DbContext.Users.AddAsync(dbo);
        await DbContext.SaveChangesAsync();
        return entry.Entity.Id;
    }

    public async Task<bool> IsUserExistAsync(string email)
    {
        return await DbContext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<UserDbo?> GetUserByEmailAsync(string email)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }
}