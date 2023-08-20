using Microsoft.AspNetCore.Mvc;

namespace Medical.System.BackEnd.Controllers;

[Route("[controller]")]
[ApiController]
public class AliveController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API is alive!");
    }
}
