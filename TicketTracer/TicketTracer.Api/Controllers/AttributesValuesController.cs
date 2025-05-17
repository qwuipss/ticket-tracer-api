using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketTracer.Api.Controllers.Abstract;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Response.AttributesValues;
using TicketTracer.Api.Services;

namespace TicketTracer.Api.Controllers;

[ApiController] [Route("[controller]")] [Authorize]
internal class AttributesValuesController(ILogger<AttributesValuesController> logger, IAttributesValuesService attributesValuesService) : AuthOpenApiControllerBase
{
    private readonly IAttributesValuesService _attributesValuesService = attributesValuesService;
    private readonly ILogger<AttributesValuesController> _logger = logger;

    #region Get

    [HttpGet] [Route("{ticketId:guid}")]
    [ProducesResponseType<List<AttributeValueModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAttributesValuesAsync([FromRoute] Guid ticketId)
    {
        var models = await _attributesValuesService.GetAttributesValuesAsync(ticketId, HttpContext.RequestAborted);
        return models is null ? NotFound() : Ok(models);
    }

    #endregion

    #region Patch

    [HttpPatch] [Route("")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<List<InvalidAttributeValueModel>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutAttributeValueAsync([FromBody] AttributeValueModel model)
    {
        var result = await _attributesValuesService.PutAsync(model, HttpContext.RequestAborted);
        return await result.Match<Task<IActionResult>>(
            attributeValuePatched =>
            {
                _logger.LogInformation("Attribute value successfully patched");
                return Task.FromResult<IActionResult>(NoContent());
            },
            attributeValueNotFound => Task.FromResult<IActionResult>(NotFound()),
            invalidAttributeValue => Task.FromResult<IActionResult>(BadRequest(invalidAttributeValue))
        );
    }

    #endregion
}