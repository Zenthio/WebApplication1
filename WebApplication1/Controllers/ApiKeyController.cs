using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApiKeyController : ControllerBase
{
    private readonly ApiKeyService _apiKeyService;

    public ApiKeyController(ApiKeyService apiKeyService)
    {
        _apiKeyService = apiKeyService;
    }

    [HttpGet("key")]
    public IActionResult GetApiKey()
    {
        return Ok(_apiKeyService.GetCurrentApiKey());
    }
}