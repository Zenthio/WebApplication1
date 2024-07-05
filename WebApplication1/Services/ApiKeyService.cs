using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System.Timers;

namespace WebApplication1.Services
{
    public class ApiKeyService
    {
        private string _currentApiKey;
        private System.Timers.Timer _timer;
        private readonly SecretClient _secretClient;
        private readonly string _secretName = "ApiKey"; // El nombre del secreto en Key Vault

        public ApiKeyService(IConfiguration configuration)
        {
            var keyVaultEndpoint = configuration["KeyVault:Endpoint"];
            if (string.IsNullOrEmpty(keyVaultEndpoint))
            {
                throw new ArgumentNullException(nameof(keyVaultEndpoint), "The Key Vault endpoint is not configured.");
            }
            _secretClient = new SecretClient(new Uri(keyVaultEndpoint), new DefaultAzureCredential());
            _currentApiKey = GetSecretValue();
            _timer = new System.Timers.Timer(3600000); // Rotate API key every hour (3600000 milliseconds)
            _timer.Elapsed += RotateApiKey;
            _timer.Start();
        }

        public string GetCurrentApiKey()
        {
            return _currentApiKey;
        }

        private void RotateApiKey(object sender, ElapsedEventArgs e)
        {
            _currentApiKey = GetSecretValue();
            Console.WriteLine($"API Key rotated to: {_currentApiKey}");
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
                Console.WriteLine($"Error retrieving secret from Key Vault: {ex.Message}");
                throw;
            }
        }
    }
}
