using System.Security.Claims;
using OneOf;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request.Accounts;
using TicketTracer.Api.Models.Response.Accounts;

namespace TicketTracer.Api.Services;

internal interface IAccountsService
{
    Task<OneOf<UserModel, UserExistsModel>> RegisterAsync(RegisterAccountModel model, CancellationToken cancellationToken);

    Task<OneOf<UserModel, UserIncorrectCredentialsModel>> LoginAsync(LoginAccountModel model, CancellationToken cancellationToken);

    ClaimsIdentity GetClaimsIdentity(UserModel model, string authenticationScheme);
}