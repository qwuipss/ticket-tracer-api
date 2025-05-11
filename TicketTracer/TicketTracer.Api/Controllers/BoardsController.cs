using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketTracer.Api.Controllers.Abstract;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request;
using TicketTracer.Api.Models.Request.Boards;
using TicketTracer.Api.Models.Response.Boards;
using TicketTracer.Api.Models.Response.Projects;
using TicketTracer.Api.Services;

namespace TicketTracer.Api.Controllers;

[ApiController] [Route("[controller]")] [Authorize]
internal class BoardsController(
    ILogger<BoardsController> logger,
    IBoardsService boardsService
) : AuthOpenApiControllerBase
{
    private readonly IBoardsService _boardsService = boardsService;
    private readonly ILogger<BoardsController> _logger = logger;

    #region Post

    [HttpPost] [Route("")]
    [ProducesResponseType<BoardModel>(StatusCodes.Status201Created)]
    [ProducesResponseType<BoardExistsModel>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProjectNotFoundModel>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateBoardModel model)
    {
        _logger.LogInformation("Trying to create new board with title ({title}) for project with id ({id})", model.Title, model.ProjectId);

        var result = await _boardsService.CreateAsync(model, HttpContext.RequestAborted);

        return await result.Match<Task<IActionResult>>(
            boardModel =>
            {
                _logger.LogInformation("Board successfully created");
                return Task.FromResult<IActionResult>(CreatedAtRoute(ControllerRoutes.GetBoardById, new { boardModel.Id, }, boardModel));
            },
            boardExists => Task.FromResult<IActionResult>(Conflict(boardExists)),
            projectNotFound => Task.FromResult<IActionResult>(NotFound(projectNotFound))
        );
    }

    #endregion

    #region Get

    [HttpGet] [Route("{id:guid}", Name = ControllerRoutes.GetBoardById)]
    [ProducesResponseType<BoardModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromQuery] [Required] Guid id)
    {
        var model = await _boardsService.GetByIdAsync(id, HttpContext.RequestAborted);
        return model is null ? NotFound() : Ok(model);
    }

    [HttpGet] [Route("all")]
    [ProducesResponseType<List<BoardModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] [Required] [Range(0, int.MaxValue)]
        int offset,
        [FromQuery] [Range(0, RequestModelsConstraints.ItemsPerPageLimit)]
        int limit = RequestModelsConstraints.ItemsPerPageLimit
    )
    {
        var models = await _boardsService.GetAllAsync(offset, limit, HttpContext.RequestAborted);
        return models is null ? NotFound() : Ok(models);
    }

    [HttpGet] [Route("all-by-project")]
    [ProducesResponseType<List<BoardModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllByProjectIdAsync(
        [FromQuery] [Required]
        Guid projectId,
        [FromQuery] [Required] [Range(0, int.MaxValue)]
        int offset,
        [FromQuery] [Range(0, RequestModelsConstraints.ItemsPerPageLimit)]
        int limit = 50
    )
    {
        var models = await _boardsService.GetAllByProjectIdAsync(projectId, offset, limit, HttpContext.RequestAborted);
        return models is null ? NotFound() : Ok(models);
    }

    [HttpGet] [Route("{id:guid}/attributes")]
    [ProducesResponseType<List<AttributeModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAttributesAsync([FromRoute] Guid id)
    {
        var models = await _boardsService.GetAttributesAsync(id, HttpContext.RequestAborted);
        return models is null ? NotFound() : Ok(models);
    }

    #endregion
}