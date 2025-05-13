using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrSendApplication.Authorization
{
    public interface IAuthorizationService
    {
        Task<bool> ValidateApiKeyAsync(string apiKey);
        string GenerateApiKey();
    }
}
