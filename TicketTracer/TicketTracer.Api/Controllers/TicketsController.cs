using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketTracer.Api.Controllers.Abstract;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request;
using TicketTracer.Api.Models.Request.Tickets;
using TicketTracer.Api.Models.Response.Boards;
using TicketTracer.Api.Services;

namespace TicketTracer.Api.Controllers;

[ApiController] [Route("[controller]")] [Authorize]
internal class TicketsController(
    ILogger<TicketsController> logger,
    ITicketsService ticketsService
) : AuthOpenApiControllerBase
{
    private readonly ILogger<TicketsController> _logger = logger;
    private readonly ITicketsService _ticketsService = ticketsService;

    #region Post

    [HttpPost] [Route("")]
    [ProducesResponseType<TicketModel>(StatusCodes.Status201Created)]
    [ProducesResponseType<BoardNotFoundModel>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTicketModel model)
    {
        _logger.LogInformation("Trying to create new ticket with title ({title}) for board with id ({id})", model.Title, model.BoardId);

        var result = await _ticketsService.CreateAsync(model, HttpContext.User, HttpContext.RequestAborted);

        return await result.Match<Task<IActionResult>>(
            ticketCreated =>
            {
                _logger.LogInformation("Ticket successfully created");
                return Task.FromResult<IActionResult>(CreatedAtRoute(ControllerRoutes.GetBoardById, new { ticketCreated.Id, }, ticketCreated));
            },
            boardNotFound => Task.FromResult<IActionResult>(NotFound(boardNotFound))
        );
    }

    #endregion

    #region Get

    [HttpGet] [Route("{id:guid}", Name = ControllerRoutes.GetTicketById)]
    [ProducesResponseType<TicketModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var model = await _ticketsService.GetByIdAsync(id, HttpContext.RequestAborted);
        return model is null ? NotFound() : Ok(model);
    }

    [HttpGet] [Route("all")]
    [ProducesResponseType<List<TicketModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] [Required] [Range(0, int.MaxValue)]
        int offset,
        [FromQuery] [Range(0, RequestModelsConstraints.ItemsPerPageLimit)]
        int limit = RequestModelsConstraints.ItemsPerPageLimit
    )
    {
        var models = await _ticketsService.GetAllAsync(offset, limit, HttpContext.RequestAborted);
        return models is null ? NotFound() : Ok(models);
    }

    [HttpGet] [Route("all-by-board")]
    [ProducesResponseType<List<TicketModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllByBoardIdAsync(
        [FromQuery] [Required]
        Guid boardId,
        [FromQuery] [Range(0, int.MaxValue)]
        int offset,
        [FromQuery] [Range(0, RequestModelsConstraints.ItemsPerPageLimit)]
        int limit = 50
    )
    {
        var models = await _ticketsService.GetAllByBoardIdAsync(boardId, offset, limit, HttpContext.RequestAborted);
        return models is null ? NotFound() : Ok(models);
    }

    #endregion
}