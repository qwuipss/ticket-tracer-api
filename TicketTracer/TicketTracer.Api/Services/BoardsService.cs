using AutoMapper;
using OneOf;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request.Boards;
using TicketTracer.Api.Models.Response.Boards;
using TicketTracer.Api.Models.Response.Projects;
using TicketTracer.Api.Repositories;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Services;

internal class BoardsService(
    ILogger<BoardsService> logger,
    IBoardsRepository boardsRepository,
    IProjectsRepository projectsRepository,
    IMapper mapper
) : IBoardsService
{
    private readonly IBoardsRepository _boardsRepository = boardsRepository;
    private readonly ILogger<BoardsService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IProjectsRepository _projectsRepository = projectsRepository;

    public async Task<BoardModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _boardsRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        var model = _mapper.Map<BoardEntity, BoardModel>(entity);

        return model;
    }

    public async Task<List<BoardModel>?> GetAllAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        var entities = await _boardsRepository.GetAllAsync(offset, limit, cancellationToken);

        _logger.LogInformation("Retrieved {count} boards", entities.Count);

        if (entities.Count is 0)
        {
            return null;
        }

        var models = _mapper.Map<List<BoardEntity>, List<BoardModel>>(entities);

        return models;
    }

    public async Task<List<BoardModel>?> GetAllByProjectIdAsync(Guid projectId, int offset, int limit, CancellationToken cancellationToken)
    {
        var entities = await _boardsRepository.GetAllByProjectIdAsync(projectId, offset, limit, cancellationToken);

        _logger.LogInformation("Retrieved {count} boards", entities.Count);

        if (entities.Count is 0)
        {
            return null;
        }

        var models = _mapper.Map<List<BoardEntity>, List<BoardModel>>(entities);

        return models;
    }

    public async Task<List<AttributeModel>?> GetAttributesAsync(Guid id, CancellationToken cancellationToken)
    {
        var entities = await _boardsRepository.GetAttributesAsync(id, cancellationToken);

        _logger.LogInformation("Retrieved {count} attributes", entities.Count);

        if (entities.Count is 0)
        {
            return null;
        }

        var models = _mapper.Map<List<AttributeEntity>, List<AttributeModel>>(entities);

        return models;
    }

    public async Task<OneOf<BoardModel, BoardExistsModel, ProjectNotFoundModel>> CreateAsync(CreateBoardModel model, CancellationToken cancellationToken)
    {
        var isProjectExist = await _projectsRepository.IsExistAsync(model.ProjectId, cancellationToken);
        if (!isProjectExist)
        {
            _logger.LogWarning("Project with specified id not found");
            return new ProjectNotFoundModel();
        }

        var isBoardExist = await _boardsRepository.IsExistAsync(model.Title, cancellationToken);
        if (isBoardExist)
        {
            _logger.LogWarning("Board with specified title already exists");
            return new BoardExistsModel { ConflictFieldName = nameof(model.Title), };
        }

        var entity = CreateBoardEntity(model);
        var boardId = await _boardsRepository.AddAsync(entity, cancellationToken);

        _logger.LogInformation("New board with id ({id}) was created", boardId);

        return _mapper.Map<BoardEntity, BoardModel>(entity);
    }

    private static BoardEntity CreateBoardEntity(CreateBoardModel model)
    {
        return new BoardEntity
        {
            Title = model.Title,
            ProjectId = model.ProjectId,
        };
    }
}