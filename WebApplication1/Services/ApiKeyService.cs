using System;

namespace WebApplication1.Services
{
    public class ApiKeyService
    {
        private readonly IConfiguration _configuration;
        private string _currentApiKey;

        public ApiKeyService(IConfiguration configuration)
        {
            _configuration = configuration;
            _currentApiKey = GenerateApiKey();
        }

        public string GetCurrentApiKey()
        {
            return _currentApiKey;
        }

        public void RotateApiKey()
        {
            _currentApiKey = GenerateApiKey();
        }

        private string GenerateApiKey()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
