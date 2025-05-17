using System.Security.Claims;
using AutoMapper;
using OneOf;
using TicketTracer.Api.Extensions;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request.Tickets;
using TicketTracer.Api.Models.Response.Boards;
using TicketTracer.Api.Repositories;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Services;

internal class TicketsService(
    ILogger<BoardsService> logger,
    ITicketsRepository ticketsRepository,
    IBoardsRepository boardsRepository,
    IMapper mapper
) : ITicketsService
{
    private readonly IBoardsRepository _boardsRepository = boardsRepository;
    private readonly ILogger<BoardsService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly ITicketsRepository _ticketsRepository = ticketsRepository;

    public async Task<TicketModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _ticketsRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        var model = _mapper.Map<TicketEntity, TicketModel>(entity);

        return model;
    }

    public async Task<List<TicketModel>?> GetAllAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        var entities = await _ticketsRepository.GetAllAsync(offset, limit, cancellationToken);

        _logger.LogInformation("Retrieved {count} tickets", entities.Count);

        if (entities.Count is 0)
        {
            return null;
        }

        var models = _mapper.Map<List<TicketEntity>, List<TicketModel>>(entities);

        return models;
    }

    public async Task<List<TicketModel>?> GetAllByBoardIdAsync(Guid boardId, int offset, int limit, CancellationToken cancellationToken)
    {
        var entities = await _ticketsRepository.GetAllByBoardIdAsync(boardId, offset, limit, cancellationToken);

        _logger.LogInformation("Retrieved {count} tickets", entities.Count);

        if (entities.Count is 0)
        {
            return null;
        }

        var models = _mapper.Map<List<TicketEntity>, List<TicketModel>>(entities);

        return models;
    }

    public async Task<OneOf<TicketModel, BoardNotFoundModel>> CreateAsync(CreateTicketModel model, ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var isBoardExist = await _boardsRepository.IsExistAsync(model.BoardId, cancellationToken);
        if (!isBoardExist)
        {
            _logger.LogWarning("Board with specified id not found");
            return new BoardNotFoundModel();
        }

        var entity = await CreateTicketEntityAsync(model, user, cancellationToken);
        var ticketId = await _ticketsRepository.AddAsync(entity, cancellationToken);

        _logger.LogInformation("New ticket with id ({id}) was created", ticketId);

        return _mapper.Map<TicketEntity, TicketModel>(entity);
    }

    private async Task<TicketEntity> CreateTicketEntityAsync(CreateTicketModel model, ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var lastTicketNumber = await _ticketsRepository.GetLastTicketNumberAsync(model.BoardId, cancellationToken);
        return new TicketEntity
        {
            Title = model.Title,
            Description = model.Description,
            Type = model.Type,
            BoardId = model.BoardId,
            AuthorId = user.GetIdOrThrow(),
            Number = lastTicketNumber + 1,
        };
    }
}