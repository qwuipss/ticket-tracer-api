using OneOf;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request.Projects;
using TicketTracer.Api.Models.Response.Projects;

namespace TicketTracer.Api.Services;

internal interface IProjectsService
{
    Task<ProjectModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<List<ProjectModel>?> GetAllAsync(int offset, int limit, CancellationToken cancellationToken);

    Task<OneOf<ProjectModel, ProjectExistsModel>> CreateAsync(CreateProjectModel model, CancellationToken cancellationToken);
}