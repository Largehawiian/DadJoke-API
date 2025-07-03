using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace DadJoke_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppControlController : ControllerBase
    {
        private readonly ILogger<AppControlController> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        public AppControlController(ILogger<AppControlController> logger, IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _appLifetime = appLifetime;
        }
        [HttpPost("shutdown")]
        public IActionResult Shutdown()
        {
            _appLifetime.StopApplication();
            _logger.LogDebug("Shutting down the application...");
            return Ok("Application is shutting down...");
        }
        [HttpGet("ping")]
        public IActionResult Ping() => Ok("pong");
    }
}
