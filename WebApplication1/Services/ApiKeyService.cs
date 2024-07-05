using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using System;
using System.Timers;

namespace WebApplication1.Services
{
    public class ApiKeyService
    {
        private string _currentApiKey;
        private readonly SecretClient _secretClient;
        private System.Timers.Timer _timer;

        public ApiKeyService(IConfiguration configuration)
        {
            _secretClient = new SecretClient(new Uri(configuration["KeyVault:Url"]), new DefaultAzureCredential());
            _currentApiKey = GetApiKeyFromVault().Result;
            _timer = new System.Timers.Timer(3600000); // Rotate API key every hour (3600000 milliseconds)
            _timer.Elapsed += RotateApiKey;
            _timer.Start();
        }

        public string GetCurrentApiKey()
        {
            return _currentApiKey;
        }

        private async void RotateApiKey(object sender, ElapsedEventArgs e)
        {
            _currentApiKey = GenerateApiKey();
            await _secretClient.SetSecretAsync(new KeyVaultSecret("ApiKey", _currentApiKey));
            Console.WriteLine($"API Key rotated to: {_currentApiKey}");
        }

        private string GenerateApiKey()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        private async Task<string> GetApiKeyFromVault()
        {
            var secret = await _secretClient.GetSecretAsync("ApiKey");
            return secret.Value.Value;
        }
    }
}
