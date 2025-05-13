using AutoMapper;
using Domain.Models;
using ErrSendApplication.DTO;
using ErrSendApplication.Features.Telegram;
using ErrSendWebApi.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ErrSendWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TelegramController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public TelegramController(IAuthorizationService authorizationService, IMapper mapper)
        {
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Відправка повідомлення про помилку в Telegram
        /// </summary>
        /// <param name="request">Інформація про помилку</param>
        /// <returns>Результат виконання</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Відправка повідомлення про помилку в Telegram",
            Description = "Відправляє інформацію про помилку в налаштований Telegram канал/групу",
            OperationId = "SendErrorToTelegram",
            Tags = new[] { "Telegram" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Повідомлення успішно відправлено", typeof(ApiResponse<ExecutionStatus>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Помилка валідації", typeof(ApiResponse<ExecutionStatus>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Помилка авторизації", typeof(ApiResponse<ExecutionStatus>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Помилка при відправці повідомлення", typeof(ApiResponse<ExecutionStatus>))]
        public async Task<ActionResult<ApiResponse<ExecutionStatus>>> SendError(
            [FromHeader(Name = "X-API-KEY")] string apiKey,
            [FromBody] SendErrorRequest request)
        {
            // Перевірка API ключа
            if (!await _authorizationService.ValidateApiKeyAsync(apiKey))
            {
                return Unauthorized(new ApiResponse<ExecutionStatus>
                {
                    Success = false,
                    Message = "Invalid API key",
                    Errors = new List<string> { "Authorization failed" }
                });
            }

            if (request == null)
            {
                return BadRequest(new ApiResponse<ExecutionStatus>
                {
                    Success = false,
                    Message = "Request body is required",
                    Errors = new List<string> { "Відсутні дані помилки" }
                });
            }

            var errorMessage = new ErrorMessage
            {
                Application = request.Application,
                Version = request.Version,
                Environment = request.Environment,
                Message = request.Message,
                StackTrace = request.StackTrace,
                AdditionalInfo = request.AdditionalInfo,
                Time = request.Timestamp ?? DateTime.UtcNow,
                Timestamp = request.Timestamp ?? DateTime.UtcNow
            };

            var command = new SendErrorMessage.Command { ErrorMessage = errorMessage };
            var response = await Mediator.Send(command);

            if (response.Status == "ER")
            {
                return StatusCode(response.ErrorCode, new ApiResponse<ExecutionStatus>
                {
                    Success = false,
                    Data = response,
                    Message = "Failed to send error to Telegram",
                    Errors = response.Errors
                });
            }

            return Ok(new ApiResponse<ExecutionStatus>
            {
                Success = true,
                Data = response,
                Message = "Error message sent successfully"
            });
        }

        /// <summary>
        /// Отримати поточні налаштування Telegram
        /// </summary>
        /// <returns>Налаштування Telegram</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Отримати налаштування Telegram",
            Description = "Повертає поточні налаштування Telegram бота",
            OperationId = "GetTelegramSettings",
            Tags = new[] { "Telegram" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Налаштування отримано", typeof(ApiResponse<TelegramSettingsDto>))]
        public ActionResult<ApiResponse<TelegramSettingsDto>> GetSettings(
            [FromHeader(Name = "X-API-KEY")] string apiKey)
        {
            if (!_authorizationService.ValidateApiKeyAsync(apiKey).Result)
            {
                return Unauthorized(new ApiResponse<TelegramSettingsDto>
                {
                    Success = false,
                    Message = "Invalid API key"
                });
            }

            var configuration = HttpContext.RequestServices.GetService<IConfiguration>();
            var settings = new TelegramSettingsDto
            {
                Enabled = configuration.GetValue<bool>("Telegram:Enabled"),
                ChatId = configuration.GetValue<string>("Telegram:ChatId"),
                BotName = configuration.GetValue<string>("Telegram:BotName", "ErrorSenderBot")
            };

            return Ok(new ApiResponse<TelegramSettingsDto>
            {
                Success = true,
                Data = settings,
                Message = "Settings retrieved successfully"
            });
        }
    }
}