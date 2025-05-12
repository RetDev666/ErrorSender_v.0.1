using Domain.Models;
using ErrSendApplication.Features.Telegram;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ErrSendWebApi.Controllers
{
    public class TelegramController : BaseController
    {
        /// <summary>
        /// Відправка повідомлення про помилку в Telegram
        /// </summary>
        /// <param name="errorMessage">Інформація про помилку</param>
        /// <returns>Результат виконання</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Відправка повідомлення про помилку в Telegram",
            Description = "Відправляє інформацію про помилку в налаштований Telegram канал/групу",
            OperationId = "SendErrorToTelegram",
            Tags = new[] { "Telegram" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Повідомлення успішно відправлено", typeof(ExecutionStatus))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Помилка валідації", typeof(ExecutionStatus))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Помилка при відправці повідомлення", typeof(ExecutionStatus))]
        public async Task<ActionResult<ExecutionStatus>> SendError([FromBody] ErrorMessage errorMessage)
        {
            if (errorMessage == null)
            {
                var result = new ExecutionStatus
                {
                    Status = "ER",
                    ErrorCode = 400
                };
                result.Errors.Add("Відсутні дані помилки");
                return BadRequest(result);
            }

            var command = new SendErrorMessage.SendErrorMessage { ErrorMessage = errorMessage };
            var response = await Mediator.Send(command);

            if (response.Status == "ER")
            {
                return StatusCode(response.ErrorCode, response);
            }

            return Ok(response);
        }
    }
}