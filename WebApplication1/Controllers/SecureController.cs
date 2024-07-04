using Microsoft.AspNetCore.Mvc;
using WebApplication1.Filters;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecureController : ControllerBase
    {
        private readonly ApiKeyService _apiKeyService;

        public SecureController(ApiKeyService apiKeyService)
        {
            _apiKeyService = apiKeyService;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiKeyAuthFilter))]
        public IActionResult Get()
        {
            return Ok("Secure data");
        }
    }
}
