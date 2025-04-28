using Microsoft.EntityFrameworkCore;
using TicketTracer.Api.Repositories.Abstract;
using TicketTracer.Data;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Repositories;

internal class ProjectsRepository(TicketTracerDbContext dbContext)
    : BaseRepository<ProjectEntity>(dbContext), IProjectsRepository
{
    public async Task<bool> IsExistAsync(string title, CancellationToken cancellationToken)
    {
        return await DbContext.Projects.AnyAsync(p => p.Title == title, cancellationToken);
    }
}