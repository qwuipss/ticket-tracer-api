using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketTracer.Api.Controllers.Abstract;
using TicketTracer.Api.Extensions;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request.Accounts;
using TicketTracer.Api.Models.Response.Accounts;
using TicketTracer.Api.Services;

namespace TicketTracer.Api.Controllers;

[ApiController] [Route("[controller]")]
internal class AccountsController(
    ILogger<AccountsController> logger,
    IAccountsService accountsService
) : NoAuthOpenApiControllerBase
{
    private readonly IAccountsService _accountsService = accountsService;
    private readonly ILogger<AccountsController> _logger = logger;

    [HttpPost] [Route("register")] [AllowAnonymous]
    [ProducesResponseType<UserModel>(StatusCodes.Status201Created)]
    [ProducesResponseType<UserExistsModel>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterAccountModel model)
    {
        _logger.LogInformation("Trying to register new account with email ({email})", model.Email);

        var result = await _accountsService.RegisterAsync(model, HttpContext.RequestAborted);

        return await result.Match<Task<IActionResult>>(
            async userModel =>
            {
                var claimsIdentity = _accountsService.GetClaimsIdentity(userModel, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                _logger.LogInformation("Account successfully registered. Auth cookie was written");

                return CreatedAtRoute(ControllerRoutes.GetUserById, new { userModel.Id, }, userModel);
            },
            userExists => Task.FromResult<IActionResult>(Conflict(userExists))
        );
    }

    [HttpPost] [Route("login")] [AllowAnonymous]
    [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginAccountModel model)
    {
        _logger.LogInformation("Trying to login account with email ({email})", model.Email);

        var result = await _accountsService.LoginAsync(model, HttpContext.RequestAborted);

        return await result.Match<Task<IActionResult>>(
            async userModel =>
            {
                var claimsIdentity = _accountsService.GetClaimsIdentity(userModel, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                _logger.LogInformation("Account successfully logged in. Auth cookie was written");

                return Ok(userModel);
            },
            _ => Task.FromResult<IActionResult>(Unauthorized())
        );
    }

    [HttpPost] [Route("logout")] [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _logger.LogInformation("Logout account with email ({email}). Auth cookie has been erased", HttpContext.User.GetEmail() ?? "?");
        return NoContent();
    }
}