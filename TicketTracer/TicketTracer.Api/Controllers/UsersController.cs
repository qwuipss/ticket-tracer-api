using Microsoft.AspNetCore.Mvc;

namespace TicketTracer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(ILogger<UsersController> logger) : ControllerBase
{
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> Get()
    {
        return Ok("hello world");
    }
}