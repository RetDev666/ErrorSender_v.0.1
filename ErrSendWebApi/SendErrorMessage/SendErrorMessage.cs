using Domain.Models;
using ErrSendApplication.Proceses;
using ErrSendPersistensTelegram.Services;
using FluentValidation;
using MediatR;

namespace ErrSendApplication.Features.Telegram
{
    public class SendErrorMessage
    {
        public class Command : IRequest<ExecutionStatus>
        {
            public ErrorMessage ErrorMessage { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ErrorMessage).NotNull()
                    .WithMessage("ErrorMessage не може бути null");
                RuleFor(x => x.ErrorMessage.Application).NotEmpty()
                    .WithMessage("Назва додатку є обов'язковою");
                RuleFor(x => x.ErrorMessage.Message).NotEmpty()
                    .WithMessage("Повідомлення про помилку є обов'язковим");
            }
        }

        public class Handler : IRequestHandler<Command, ExecutionStatus>
        {
            private readonly TelegramBotService telegramBotService;
            private readonly IErrorProcessor errorProcessor;
            private readonly ILogger<Handler> logger;

            public Handler(TelegramBotService telegramBotService, ILogger<Handler> logger)
            {
                TelegramBotService telegramBotService,
                IErrorProcessor errorProcessor,
                ILogger<Handler> logger)
            {
                this.telegramBotService = telegramBotService ?? throw new ArgumentNullException(nameof(telegramBotService));
                this.errorProcessor = errorProcessor ?? throw new ArgumentNullException(nameof(errorProcessor));
                this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
                
            }

            public async Task<ExecutionStatus> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = new ExecutionStatus();

                try
                {
                    // Валідуємо повідомлення
                    var isValid = await errorProcessor.ValidateErrorMessageAsync(request.ErrorMessage);
                    if (!isValid)
                    {
                        result.Status = "ER";
                        result.Errors.Add("Повідомлення про помилку не пройшло валідацію");
                        result.ErrorCode = 400;
                        return result;
                    }

                    // Обробляємо помилку
                    var processedError = await errorProcessor.ProcessErrorAsync(request.ErrorMessage);

                    logger.LogInformation($"Початок відправки повідомлення про помилку: {processedError.Application}");

                    var success = await telegramBotService.SendErrorMessageAsync(request.ErrorMessage);

                    if (!success)
                    {
                        result.Status = "ER";
                        result.Errors.Add("Не вдалося відправити повідомлення про помилку в Telegram");
                        result.ErrorCode = 500;
                        logger.LogError("Не вдалося відправити повідомлення про помилку в Telegram");
                    }
                    else
                    {
                        logger.LogInformation("Повідомлення про помилку успішно відправлено в Telegram");
                    }

                }
                catch (Exception ex)
                {
                    result.Status = "ER";
                    result.Errors.Add($"Виникла помилка: {ex.Message}");
                    result.ErrorCode = 500;
                    logger.LogError(ex, "Виникла помилка при відправці повідомлення в Telegram");
                }

                return result;
            }
        }
    }
}