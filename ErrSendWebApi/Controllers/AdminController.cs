using DocumentFormat.OpenXml.Drawing.Diagrams;
using ErrSendApplication.Authorization;
using ErrSendWebApi.ModelsDto;
using Microsoft.AspNetCore.Mvc;

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
            this.authorizationService = authorizationService ;
            this.configuration = configuration ;
        }

        /// <summary>
        /// Генеруємо новий API ключ
        /// </summary>
        /// <returns>Новий API ключ</returns>\
        [HttpPost]
        [SwaggerOperation(
            Summary = "Генеруємо новий API  ключ",
            Description = "Генеруємо новий API ключ для доступу до API",
            OperationId = "GenerateApiKey",
            TagsAttribute = new[] { "Admin" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "API ключ успішно згенеровано", typeof(ApiResponse<string>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Неавторизований доступ"]
        public ActionResult<ApiResponse<string>> GenerateApiKey([FromHeader(Name = "X-ADMIN-KEY")] string adminKey)
        {
            //Перевірка адміністративного ключа
            var validAdminKey = configuration.GetValue<string>("AdminKey");
            if (String.IsNullOrEmpty(adminKey) || adminKey != validAdminKey)
            {
                Success = false;
                Message = "Invalid admin key";
            });
        }

        var newApiKey = authorizationService.GetApiKey();

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
    /// <param name="apiKey">API ключ для перевірки</param>"
    /// <returns>Результат перевірки</returns>
    [HttpPost]
    [SwaggerOperation(
            Summary = "Перевірка валідності API ключа",
            Description = "Перевіряє чи є API ключ валідним",
            OperationId = "ValidateApiKey",
            TagsAttribute = new[] { "Admin" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Результат перевірки", typeof(ApiResponse<bool>))]
        public async Task<ActionResult<ApiResponse<bool>>> ValidateApiKey([FromBody] string apiKey)
        {
            var isValid = await _authorizationService.ValidateApiKeyAsync(apiKey);

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
                TelegramEnabled = _configuration.GetValue<bool>("Telegram:Enabled"),
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
    }
}
