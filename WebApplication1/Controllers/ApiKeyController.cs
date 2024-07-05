using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiKeyController(ApiKeyService apiKeyService, ILogger<ApiKeyController> logger) : ControllerBase
    {
        private readonly ApiKeyService _apiKeyService = apiKeyService;
        private readonly ILogger<ApiKeyController> _logger = logger;

        [HttpGet("key")]
        public IActionResult GetApiKey()
        {
            _logger.LogInformation("API Key requested.");
            return Ok(_apiKeyService.GetCurrentApiKey());
        }
    }
}

