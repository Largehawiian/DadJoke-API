using Microsoft.AspNetCore.Mvc;

namespace DadJoke_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextController : ControllerBase
    {
        [HttpGet]
        [Produces("text/plain")]
        public IActionResult GetText()
        {
            string responseText = "Hello, this is your text response!";
            return Content(responseText, "text/plain");
        }
    }
}