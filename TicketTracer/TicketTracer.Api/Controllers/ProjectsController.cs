using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketTracer.Api.Controllers.Abstract;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request;
using TicketTracer.Api.Models.Request.Projects;
using TicketTracer.Api.Models.Response.Projects;
using TicketTracer.Api.Services;

namespace TicketTracer.Api.Controllers;

[ApiController] [Route("[controller]")] [Authorize]
internal class ProjectsController(
    ILogger<ProjectsController> logger,
    IProjectsService projectsService
) : AuthOpenApiControllerBase
{
    private readonly ILogger<ProjectsController> _logger = logger;
    private readonly IProjectsService _projectsService = projectsService;

    #region Post

    [HttpPost] [Route("")]
    [ProducesResponseType<ProjectModel>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProjectExistsModel>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProjectModel model)
    {
        _logger.LogInformation("Trying to create new project with title ({title})", model.Title);

        var result = await _projectsService.CreateAsync(model, HttpContext.RequestAborted);

        return await result.Match<Task<IActionResult>>(
            projectModel =>
            {
                _logger.LogInformation("Project successfully created");
                return Task.FromResult<IActionResult>(CreatedAtRoute(ControllerRoutes.GetProjectById, new { projectModel.Id, }, projectModel));
            },
            projectExists => Task.FromResult<IActionResult>(Conflict(projectExists))
        );
    }

    #endregion

    #region Get

    [HttpGet] [Route("{id:guid}", Name = ControllerRoutes.GetProjectById)]
    [ProducesResponseType<ProjectModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var model = await _projectsService.GetByIdAsync(id, HttpContext.RequestAborted);
        return model is null ? NotFound() : Ok(model);
    }

    [HttpGet] [Route("all")]
    [ProducesResponseType<List<ProjectModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] [Required] [Range(0, int.MaxValue)]
        int offset,
        [FromQuery] [Range(0, RequestModelsConstraints.ItemsPerPageLimit)]
        int limit = RequestModelsConstraints.ItemsPerPageLimit
    )
    {
        var models = await _projectsService.GetAllAsync(offset, limit, HttpContext.RequestAborted);
        return models is null ? NotFound() : Ok(models);
    }

    #endregion
}