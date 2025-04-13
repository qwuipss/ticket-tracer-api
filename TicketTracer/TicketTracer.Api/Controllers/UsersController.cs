using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace TicketTracer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(ILogger<UsersController> logger) : ControllerBase
{
    private static readonly Counter<int> _meter = new Meter("ticket-tracer-api").CreateCounter<int>("users_controller");
    private readonly ILogger<UsersController> _logger = logger;
    
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> Get()
    {
        _meter.Add(1, KeyValuePair.Create<string, object?>("endpoint", "get"));
        _logger.LogInformation("hello world");
        return Ok("hello world");
    }
}