using AutoMapper;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Repositories;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Services;

internal class UsersService(ILogger<UsersService> logger, IUsersRepository usersRepository, IMapper mapper) : IUsersService
{
    private readonly ILogger<UsersService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IUsersRepository _usersRepository = usersRepository;

    public async Task<UserModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _usersRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        var model = _mapper.Map<UserEntity, UserModel>(entity);

        return model;
    }

    public async Task<List<UserEntity>?> GetAllAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        var entities = await _usersRepository.GetAllAsync(offset, limit, cancellationToken);

        _logger.LogInformation("Retrieved {count} tickets", entities.Count);

        if (entities.Count is 0)
        {
            return null;
        }

        var models = _mapper.Map<List<UserEntity>, List<UserEntity>>(entities);

        return models;
    }
}