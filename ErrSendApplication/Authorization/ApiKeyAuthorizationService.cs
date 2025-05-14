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

            // Отримуємо масив API ключів з конфігурації
            var apiKeysSection = configuration.GetSection("ApiKeys");
            var validApiKeys = new List<string>();

            // Ітеруємося по всіх елементах секції
            for (int i = 0; ; i++)
            {
                var key = apiKeysSection.GetValue<string>($"{i}");
                if (key == null)
                    break;
                validApiKeys.Add(key);
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