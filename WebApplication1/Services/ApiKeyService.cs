using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Timers;

namespace WebApplication1.Services
{
    public class ApiKeyService
    {
        private string _currentApiKey;
        private System.Timers.Timer _timer;
        private readonly SecretClient _secretClient;
        private readonly string _secretName = "ApiKey"; // El nombre del secreto en Key Vault
        private readonly ILogger<ApiKeyService> _logger;

        public ApiKeyService(IConfiguration configuration, ILogger<ApiKeyService> logger)
        {
            var keyVaultEndpoint = configuration["KeyVault:Endpoint"];
            if (string.IsNullOrEmpty(keyVaultEndpoint))
            {
                throw new ArgumentNullException(nameof(keyVaultEndpoint), "The Key Vault endpoint is not configured.");
            }
            
            _secretClient = new SecretClient(new Uri(keyVaultEndpoint), new DefaultAzureCredential());
            _logger = logger;
            _currentApiKey = GetSecretValue();
            _timer = new System.Timers.Timer(180000); // 3 minutos
            _timer.Elapsed += RotateApiKey;
            _timer.Start();
            
        }

        public string GetCurrentApiKey()
        {
            return _currentApiKey;
        }

        private void RotateApiKey(object sender, ElapsedEventArgs e)
        {
            /*
            _currentApiKey = GetSecretValue();
            Console.WriteLine($"API Key rotated to: {_currentApiKey}");
            */
            _currentApiKey = GenerateNewApiKey();
            UpdateSecretValue(_currentApiKey);
            _logger.LogInformation($"API Key rotated to: {_currentApiKey}");
        }

        private string GetSecretValue()
        {
            try
            {
                KeyVaultSecret secret = _secretClient.GetSecret(_secretName);
                return secret.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving secret from Key Vault: {ex.Message}");
                throw;
            }
        }

        private string GenerateNewApiKey()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        private void UpdateSecretValue(string newApiKey)
        {
            try
            {
                _secretClient.SetSecret(new KeyVaultSecret(_secretName, newApiKey));
                _logger.LogInformation("API Key updated successfully in Key Vault: {_secretName}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating secret in Key Vault: {ex.Message}");
                throw;
            }
        }
    }

    
}
