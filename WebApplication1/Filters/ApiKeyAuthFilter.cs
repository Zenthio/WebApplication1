using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Services;

namespace WebApplication1.Filters
{
    public class ApiKeyAuthFilter : IAsyncActionFilter
    {
        private readonly ApiKeyService _apiKeyService;

        public ApiKeyAuthFilter(ApiKeyService apiKeyService)
        {
            _apiKeyService = apiKeyService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!_apiKeyService.GetCurrentApiKey().Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
