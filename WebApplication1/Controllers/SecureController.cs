using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecureController(ILogger<SecureController> logger) : ControllerBase
    {
        private readonly ILogger<SecureController> _logger = logger;

        [HttpGet]
        public IActionResult GetSecureData()
        {
            _logger.LogInformation("Secure endpoint requested.");
            return Ok("Secure data");
        }
    }
}
