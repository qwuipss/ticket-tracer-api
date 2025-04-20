using Microsoft.AspNetCore.Mvc;

namespace TicketTracer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DevController : ControllerBase
{
    [Route("[action]")]
    public void Throw()
    {
        throw new Exception("A development exception");
    }
}