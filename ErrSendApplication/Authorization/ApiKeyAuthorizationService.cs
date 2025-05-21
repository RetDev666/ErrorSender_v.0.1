using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace ErrSendApplication.Authorization
{
    public class ApiKeyAuthorizationService : IAuthorizationService
    {
        private readonly IConfiguration configuration;

        public ApiKeyAuthorizationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<bool> ValidateApiKeyAsync(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                return Task.FromResult(false);

            // Get the array of API keys from the configuration
            var apiKeysSection = configuration.GetSection("ApiKeys");
            var validApiKeys = new List<string>();

            // Iterate through all elements in the section
            foreach (var child in apiKeysSection.GetChildren())
            {
                if (!string.IsNullOrEmpty(child.Value))
                {
                    validApiKeys.Add(child.Value);
                }
            }

            return Task.FromResult(validApiKeys.Contains(apiKey));
        }

        public string GenerateApiKey()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }
}