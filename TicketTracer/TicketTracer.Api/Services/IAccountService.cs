using System.Security.Claims;
using OneOf;
using TicketTracer.Api.Models.Request.Users;
using TicketTracer.Api.Models.Response.Users;

namespace TicketTracer.Api.Services;

internal interface IAccountService
{
    Task<OneOf<UserRegisteredModel, UserExistsModel>> RegisterUserAsync(RegisterUserModel model);
    Task<OneOf<UserLoggedInModel, UserIncorrectCredentialsModel>> LoginUserAsync(LoginUserModel model);
    Task<ClaimsIdentity> GetUserClaimsIdentityAsync(Guid userId, string authenticationScheme);
}