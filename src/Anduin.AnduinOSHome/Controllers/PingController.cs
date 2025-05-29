using Aiursoft.WebTools.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Anduin.AnduinOSHome.Controllers;

public class PingController : ControllerBase
{
    [AiurNoCache]
    public IActionResult Index()
    {
        return Ok(new List<object>());
    }
}
