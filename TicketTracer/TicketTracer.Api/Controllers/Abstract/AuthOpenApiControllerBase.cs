using Microsoft.AspNetCore.Mvc;

namespace TicketTracer.Api.Controllers.Abstract;

[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
internal class AuthOpenApiControllerBase : NoAuthOpenApiControllerBase
{
}