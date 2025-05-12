using Domain.Models;
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
                RuleFor(x => x.ErrorMessage).NotNull();
                RuleFor(x => x.ErrorMessage.Application).NotEmpty();
                RuleFor(x => x.ErrorMessage.Message).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, ExecutionStatus>
        {
            private readonly TelegramBotService telegramBotService;
            private readonly ILogger<Handler> logger;

            public Handler(TelegramBotService telegramBotService, ILogger<Handler> logger)
            {
                this.telegramBotService = telegramBotService;
                this.logger = logger;
            }

            public async Task<ExecutionStatus> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = new ExecutionStatus();

                var success = await telegramBotService.SendErrorMessageAsync(request.ErrorMessage);

                if (!success)
                {
                    result.Status = "ER";
                    result.Errors.Add("Не вдалося відправити повідомлення про помилку в Telegram");
                    result.ErrorCode = 500;
                    logger.LogError("Не вдалося відправити повідомлення про помилку в Telegram");
                }

                return result;
            }
        }

        internal class SendErrorMessage : IRequest<object>
        {
            public ErrorMessage ErrorMessage { get; set; }
        }
    }
}