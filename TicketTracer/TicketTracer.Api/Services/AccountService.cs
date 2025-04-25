using System.Security.Claims;
using OneOf;
using TicketTracer.Api.Models.Request.Users;
using TicketTracer.Api.Models.Response.Users;
using TicketTracer.Api.Repositories;
using TicketTracer.Api.Utilities;
using TicketTracer.Data.Models;

namespace TicketTracer.Api.Services;

internal class AccountService(
    ILogger<AccountService> logger,
    IUsersRepository usersRepository,
    IPasswordsManager passwordsManager,
    IGuidFactory guidFactory
) : IAccountService
{
    private readonly ILogger<AccountService> _logger = logger;
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly IPasswordsManager _passwordsManager = passwordsManager;
    private readonly IGuidFactory _guidFactory = guidFactory;

    public async Task<OneOf<UserRegisteredModel, UserExistsModel>> RegisterUserAsync(RegisterUserModel model)
    {
        var isUserExist = await _usersRepository.IsUserExistAsync(model.Email);
        if (isUserExist)
        {
            _logger.LogWarning("User with specified email already exists");
            return new UserExistsModel { ConflictFieldName = nameof(model.Email), };
        }

        var userDbo = CreateUserDbo(model);
        var userId = await _usersRepository.AddUserAsync(userDbo);

        _logger.LogInformation("New user with id ({id}) was created", userId);

        return new UserRegisteredModel { Id = userId, };
    }

    public async Task<OneOf<UserLoggedInModel, UserIncorrectCredentialsModel>> LoginUserAsync(LoginUserModel model)
    {
        var userDbo = await _usersRepository.GetUserByEmailAsync(model.Email);

        if (userDbo is null)
        {
            _logger.LogWarning("User with specified email doesn't exist");
            return new UserIncorrectCredentialsModel();
        }

        _logger.LogInformation("Found user with id ({id})", userDbo.Id);
        
        if (_passwordsManager.Verify(model.Password, userDbo.PasswordHash, userDbo.PasswordSalt))
        {
            _logger.LogInformation("Stored and specified passwords are match");
            return new UserLoggedInModel { Id = userDbo.Id, };
        }

        _logger.LogWarning("Incorrect password specified for user");

        return new UserIncorrectCredentialsModel();
    }

    public async Task<ClaimsIdentity> GetUserClaimsIdentityAsync(Guid userId, string authenticationScheme)
    {
        var userDbo = await _usersRepository.GetByIdAsync(userId);

        if (userDbo is null)
        {
            throw new ArgumentNullException(nameof(userId), $"User with id ({userId}) unexpectedly doesn't exist or has been deleted");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, userDbo.Email),
        };

        var claimsIdentity = new ClaimsIdentity(claims, authenticationScheme);

        return claimsIdentity;
    }

    private UserDbo CreateUserDbo(RegisterUserModel model)
    {
        var passwordSalt = _passwordsManager.GenerateSalt();
        return new UserDbo
        {
            Id = _guidFactory.Create(),
            Email = model.Email,
            Name = model.Name,
            Surname = model.Surname,
            PasswordHash = _passwordsManager.Hash(model.Password, passwordSalt),
            PasswordSalt = passwordSalt,
        };
    }
}