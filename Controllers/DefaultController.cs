using Microsoft.AspNetCore.Mvc;

namespace DadJoke_API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("{*url}", Order = int.MaxValue)] // Catch-all route, lowest priority
    public class DefaultController : ControllerBase
    {
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpPatch, HttpOptions, HttpHead]
        public IActionResult CatchAll()
        {
            string reply = "Invalid Endpoint";
            // Return 404 Not Found with custom content
            return NotFound(reply);
        }
    }
}
