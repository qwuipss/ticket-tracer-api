using System.Security.Claims;
using AutoMapper;
using OneOf;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request.Accounts;
using TicketTracer.Api.Models.Response.Accounts;
using TicketTracer.Api.Repositories;
using TicketTracer.Api.Utilities;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Services;

internal class AccountsService(
    ILogger<AccountsService> logger,
    IUsersRepository usersRepository,
    IPasswordsManager passwordsManager,
    IMapper mapper
) : IAccountsService
{
    private readonly ILogger<AccountsService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IPasswordsManager _passwordsManager = passwordsManager;
    private readonly IUsersRepository _usersRepository = usersRepository;

    public async Task<OneOf<UserModel, UserExistsModel>> RegisterAsync(RegisterAccountModel model, CancellationToken cancellationToken)
    {
        var isUserExist = await _usersRepository.IsExistAsync(model.Email, cancellationToken);
        if (isUserExist)
        {
            _logger.LogWarning("User with specified email already exists");
            return new UserExistsModel { ConflictFieldName = nameof(model.Email), };
        }

        var entity = CreateUserEntity(model);
        var userId = await _usersRepository.AddAsync(entity, cancellationToken);

        _logger.LogInformation("New user with id ({id}) was created", userId);

        return _mapper.Map<UserEntity, UserModel>(entity);
    }

    public async Task<OneOf<UserModel, UserIncorrectCredentialsModel>> LoginAsync(LoginAccountModel model, CancellationToken cancellationToken)
    {
        var entity = await _usersRepository.GetByEmailAsync(model.Email, cancellationToken);

        if (entity is null)
        {
            _logger.LogWarning("User with specified email doesn't exist");
            return new UserIncorrectCredentialsModel();
        }

        _logger.LogInformation("Found user with id ({id})", entity.Id);

        if (_passwordsManager.Verify(model.Password, entity.PasswordHash, entity.PasswordSalt))
        {
            _logger.LogInformation("Stored and specified passwords are match");
            return _mapper.Map<UserEntity, UserModel>(entity);
        }

        _logger.LogWarning("Incorrect password specified for user");

        return new UserIncorrectCredentialsModel();
    }

    public ClaimsIdentity GetClaimsIdentity(UserModel model, string authenticationScheme)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, model.Id.ToString()),
            new(ClaimTypes.Email, model.Email!),
        };

        var claimsIdentity = new ClaimsIdentity(claims, authenticationScheme);

        return claimsIdentity;
    }

    private UserEntity CreateUserEntity(RegisterAccountModel model)
    {
        var passwordSalt = _passwordsManager.GenerateSalt();
        return new UserEntity
        {
            Email = model.Email,
            Name = model.Name,
            Surname = model.Surname,
            PasswordHash = _passwordsManager.Hash(model.Password, passwordSalt),
            PasswordSalt = passwordSalt,
        };
    }
}