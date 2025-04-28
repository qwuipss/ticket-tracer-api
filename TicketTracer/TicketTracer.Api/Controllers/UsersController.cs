using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketTracer.Api.Controllers.Abstract;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Services;

namespace TicketTracer.Api.Controllers;

[ApiController] [Route("[controller]")] [Authorize]
internal class UsersController(ILogger<UsersController> logger, IUsersService usersService) : AuthOpenApiControllerBase
{
    private readonly ILogger<UsersController> _logger = logger;
    private readonly IUsersService _usersService = usersService;

    #region Get

    [HttpPost] [Route("{id:guid}", Name = ControllerRoutes.GetUserById)]
    [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var model = await _usersService.GetByIdAsync(id, HttpContext.RequestAborted);
        return model is null ? NotFound() : Ok(model);
    }

    #endregion
}