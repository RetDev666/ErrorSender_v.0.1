using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ErrSendApplication.Authorization
{
    public class ApiKeyAuthorizationService : IAuthorizationService
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthorizationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> ValidateApiKeyAsync(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                return Task.FromResult(false);

            var validApiKeys = _configuration.GetSection("ApiKeys").Get<List<string>>() ?? new List<string>();
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