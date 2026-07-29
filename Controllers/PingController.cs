using Microsoft.AspNetCore.Mvc;

namespace TannersWebsiteTemplate.Controllers
{
    // Routing is pretty simple, api/[controller] is api/Ping as "Controller" is omitted.
    // Removing api/ would make it /Ping, blanking the route it would only work if the below HttpGet is set to something.
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        // if this had a name, the route would be api/Ping/(the name)
        [HttpGet("")]
        public IActionResult Ping()
        {
            long time = DateTime.Now.Ticks;
            long ping = DateTime.Now.Ticks - time;
            return new JsonResult(new { Ping = ping + "ms" });
        }
    }
}
