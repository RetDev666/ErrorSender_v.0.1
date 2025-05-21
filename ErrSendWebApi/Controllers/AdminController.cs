using DocumentFormat.OpenXml.Drawing.Diagrams;
using ErrSendApplication.Authorization;
using ErrSendWebApi.Helpers;
using ErrSendWebApi.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Cryptography;

namespace ErrSendWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController : BaseController
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IConfiguration configuration;

        public AdminController(IAuthorizationService authorizationService, IConfiguration configuration)
        {
            this.authorizationService = authorizationService;
            this.configuration = configuration;
        }

        /// <summary>
        /// Генеруємо новий API ключ
        /// </summary>
        /// <returns>Новий API ключ</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Генеруємо новий API  ключ",
            Description = "Генеруємо новий API ключ для доступу до API",
            OperationId = "GenerateApiKey",
            Tags = new[] { "Admin" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "API ключ успішно згенеровано", typeof(ApiResponse<string>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Неавторизований доступ")]
        public ActionResult<ApiResponse<string>> GenerateApiKey([FromHeader(Name = "X-ADMIN-KEY")] string adminKey)
        {
            //Перевірка адміністративного ключа
            var validAdminKey = configuration.GetValue<string>("AdminKey");
            if (String.IsNullOrEmpty(adminKey) || adminKey != validAdminKey)
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid admin key"
                });
            }

            var newApiKey = authorizationService.GenerateApiKey();

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = newApiKey,
                Message = "Ключ API успішно згенеровано. Не забудьте додати його до своєї конфігурації."
            });
        }

        /// <summary>
        /// Перевірка валідності API ключа
        /// </summary>
        /// <param name="apiKey">API ключ для перевірки</param>
        /// <returns>Результат перевірки</returns>
        [HttpPost]
        [SwaggerOperation(
                Summary = "Перевірка валідності API ключа",
                Description = "Перевіряє чи є API ключ валідним",
                OperationId = "ValidateApiKey",
                Tags = new[] { "Admin" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Результат перевірки", typeof(ApiResponse<bool>))]
        public async Task<ActionResult<ApiResponse<bool>>> ValidateApiKey([FromBody] string apiKey)
        {
            var isValid = await authorizationService.ValidateApiKeyAsync(apiKey);

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Data = isValid,
                Message = isValid ? "API key is valid" : "API key is invalid"
            });
        }

        /// <summary>
        /// Отримати статус системи
        /// </summary>
        /// <returns>Статус системи</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Статус системи",
            Description = "Повертає поточний статус системи та налаштувань",
            OperationId = "GetSystemStatus",
            Tags = new[] { "Admin" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Статус системи", typeof(ApiResponse<object>))]
        public ActionResult<ApiResponse<object>> GetSystemStatus()
        {
            var status = new
            {
                Application = "ErrorSender",
                Version = "0.1",
                Status = "Running",
                TelegramEnabled = configuration.GetValue<bool>("Telegram:Enabled"),
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                ServerTime = DateTime.UtcNow
            };

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = status,
                Message = "System status retrieved successfully"
            });
        }

        /// <summary>
        /// Генеруємо новий JWT ключ
        /// </summary>
        /// <returns>Новий JWT ключ</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Генеруємо новий JWT ключ",
            Description = "Генеруємо новий JWT ключ для автентифікації",
            OperationId = "GenerateJwtKey",
            Tags = new[] { "Admin" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "JWT ключ успішно згенеровано", typeof(ApiResponse<string>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Неавторизований доступ")]
        public ActionResult<ApiResponse<string>> GenerateJwtKey([FromHeader(Name = "X-ADMIN-KEY")] string adminKey)
        {
            if (adminKey != configuration["AdminKey"])
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid admin key"
                });
            }

            // Генеруємо випадковий ключ
            byte[] randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            string clearKey = Convert.ToBase64String(randomBytes);

            // Шифруємо ключ
            string encryptedKey = PassHelper.Encrypt(clearKey, "HEnglish");

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = encryptedKey,
                Message = "JWT key generated successfully"
            });
        }
    }
}
