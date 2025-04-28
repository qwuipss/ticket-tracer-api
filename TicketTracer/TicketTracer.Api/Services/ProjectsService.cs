using AutoMapper;
using OneOf;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request.Projects;
using TicketTracer.Api.Models.Response.Projects;
using TicketTracer.Api.Repositories;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Services;

internal class ProjectsService(
    ILogger<ProjectsService> logger,
    IProjectsRepository projectsRepository,
    IMapper mapper
) : IProjectsService
{
    private readonly ILogger<ProjectsService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IProjectsRepository _projectsRepository = projectsRepository;

    public async Task<ProjectModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _projectsRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        var model = _mapper.Map<ProjectEntity, ProjectModel>(entity);

        return model;
    }

    public async Task<List<ProjectModel>?> GetAllAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        var entities = await _projectsRepository.GetAllAsync(offset, limit, cancellationToken);

        _logger.LogInformation("Retrieved {count} projects", entities.Count);

        if (entities.Count is 0)
        {
            return null;
        }

        var models = _mapper.Map<List<ProjectEntity>, List<ProjectModel>>(entities);

        return models;
    }

    public async Task<OneOf<ProjectModel, ProjectExistsModel>> CreateAsync(CreateProjectModel model, CancellationToken cancellationToken)
    {
        var isProjectExist = await _projectsRepository.IsExistAsync(model.Title, cancellationToken);
        if (isProjectExist)
        {
            _logger.LogWarning("Project with specified title already exists");
            return new ProjectExistsModel { ConflictFieldName = nameof(model.Title), };
        }

        var entity = CreateProjectEntity(model);
        var projectId = await _projectsRepository.AddAsync(entity, cancellationToken);

        _logger.LogInformation("New project with id ({id}) was created", projectId);

        return _mapper.Map<ProjectEntity, ProjectModel>(entity);
    }

    private static ProjectEntity CreateProjectEntity(CreateProjectModel model)
    {
        return new ProjectEntity
        {
            Title = model.Title,
        };
    }
}