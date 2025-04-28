using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using TicketTracer.Api.Models.Response;

namespace TicketTracer.Api.Controllers.Abstract;

[Consumes(MediaTypeNames.Application.Json)] [Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType<UnhandledExceptionModel>(StatusCodes.Status500InternalServerError)]
internal abstract class NoAuthOpenApiControllerBase : ControllerBase
{
}