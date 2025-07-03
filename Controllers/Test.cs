using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DadJoke_API.Controllers
{
    [ApiController]
    [Route("[controller]/Jokes")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IJokeService _jokeService;

        public TestController(ILogger<TestController> logger, IJokeService jokeService)
        {
            _logger = logger;
            _jokeService = jokeService;
        }

        [HttpGet("icanhazdadjoke")]
        [Produces("text/plain")]
        public async Task<IActionResult> GetDadJoke()
        {
            _logger.LogDebug("Received GET request at /Test/Jokes/icanhazdadjoke from {RemoteIpAddress}", HttpContext.Connection.RemoteIpAddress);

            string? joke = await _jokeService.GetJokeAsync(1);

            if (string.IsNullOrWhiteSpace(joke))
            {
                _logger.LogWarning("No joke found for /Test/Jokes/icanhazdadjoke");
                return NotFound("No joke found.");
            }

            _logger.LogDebug("Returning joke for /Test/Jokes/icanhazdadjoke");
            return Content(joke, "text/plain", System.Text.Encoding.UTF8);
        }

        [HttpGet("jokeapi-v2")]
        [Produces("text/plain")]
        public async Task<IActionResult> GetAPIJoke()
        {
            _logger.LogDebug("Received GET request at /Test/Jokes/jokeapi-v2 from {RemoteIpAddress}", HttpContext.Connection.RemoteIpAddress);

            string? joke = await _jokeService.GetJokeAsync(2);

            if (string.IsNullOrWhiteSpace(joke))
            {
                _logger.LogWarning("No joke found for /Test/Jokes/jokeapi-v2");
                return NotFound("No joke found.");
            }

            _logger.LogDebug("Returning joke for /Test/Jokes/jokeapi-v2");
            return Content(joke, "text/plain", System.Text.Encoding.UTF8);
        }
    }
}