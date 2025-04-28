#if DEBUG
using Microsoft.AspNetCore.Mvc;

namespace TicketTracer.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
internal class DevController : ControllerBase
{
    [HttpGet]
    public void Throw()
    {
        throw new Exception("A development exception");
    }

    [HttpGet]
    public async Task<ActionResult> Delay()
    {
        await Task.Delay(Random.Shared.Next(100, 500));
        return Ok();
    }
}
#endif