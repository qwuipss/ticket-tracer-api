using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketTracer.Api.Controllers.Abstract;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request;
using TicketTracer.Api.Services;

namespace TicketTracer.Api.Controllers;

[ApiController] [Route("[controller]")] [Authorize]
internal class UsersController(ILogger<UsersController> logger, IUsersService usersService) : AuthOpenApiControllerBase
{
    private readonly ILogger<UsersController> _logger = logger;
    private readonly IUsersService _usersService = usersService;

    #region Get

    [HttpGet] [Route("{id:guid}", Name = ControllerRoutes.GetUserById)]
    [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var model = await _usersService.GetByIdAsync(id, HttpContext.RequestAborted);
        return model is null ? NotFound() : Ok(model);
    }

    [HttpGet] [Route("all")]
    [ProducesResponseType<List<UserModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] [Required] [Range(0, int.MaxValue)]
        int offset,
        [FromQuery] [Range(0, RequestModelsConstraints.ItemsPerPageLimit)]
        int limit = RequestModelsConstraints.ItemsPerPageLimit
    )
    {
        var models = await _usersService.GetAllAsync(offset, limit, HttpContext.RequestAborted);
        return models is null ? NotFound() : Ok(models);
    }

    #endregion
}