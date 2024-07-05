using System;
using System.Timers;

namespace WebApplication1.Services
{
    public class ApiKeyService
    {
        private string _currentApiKey;
        private System.Timers.Timer _timer;

        public ApiKeyService()
        {
            _currentApiKey = GenerateApiKey();
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
            _currentApiKey = GenerateApiKey();
            Console.WriteLine($"API Key rotated to: {_currentApiKey}");
        }

        private string GenerateApiKey()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
