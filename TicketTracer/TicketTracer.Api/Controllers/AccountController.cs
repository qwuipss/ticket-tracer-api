using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketTracer.Api.Models.Request.Users;
using TicketTracer.Api.Models.Response;
using TicketTracer.Api.Models.Response.Users;
using TicketTracer.Api.Services;

namespace TicketTracer.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]/[action]")]
[ProducesResponseType<UnhandledExceptionModel>(StatusCodes.Status500InternalServerError)]
internal class AccountController(
    ILogger<AccountController> logger,
    IAccountService accountService
) : ControllerBase
{
    private readonly ILogger<AccountController> _logger = logger;
    private readonly IAccountService _accountService = accountService;

    [HttpPost]
    [ProducesResponseType<UserRegisteredModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<UserExistsModel>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
    {
        _logger.LogInformation("Trying to register new user with email ({email})", model.Email);

        var result = await _accountService.RegisterUserAsync(model);

        return await result.Match<Task<IActionResult>>(
            async userRegistered =>
            {
                var claimsIdentity = await _accountService.GetUserClaimsIdentityAsync(userRegistered.Id, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                
                _logger.LogInformation("User successfully registered. Auth cookie was written");
                
                return Ok(userRegistered);
            },
            userExists => Task.FromResult<IActionResult>(Conflict(userExists))
        );
    }

    [HttpPost]
    [ProducesResponseType<UserLoggedInModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<UserIncorrectCredentialsModel>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginUserModel model)
    {
        _logger.LogInformation("Trying to login user with email ({email})", model.Email);

        var result = await _accountService.LoginUserAsync(model);

        return await result.Match<Task<IActionResult>>(
            async userLoggedIn =>
            {
                var claimsIdentity = await _accountService.GetUserClaimsIdentityAsync(userLoggedIn.Id, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                
                _logger.LogInformation("User successfully logged in. Auth cookie was written");
                
                return Ok(userLoggedIn);
            },
            userIncorrectCredentials => Task.FromResult<IActionResult>(Unauthorized(userIncorrectCredentials))
        );
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("Logout");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}