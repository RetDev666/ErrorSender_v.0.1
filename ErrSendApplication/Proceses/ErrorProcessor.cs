using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.Models;
using ErrSendApplication.Proceses;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ErrSendApplication.Processes
{
    public class ErrorProcessor : IErrorProcessor
    {
        private readonly ILogger<ErrorProcessor> logger;

        public ErrorProcessor(ILogger<ErrorProcessor> logger)
        {
            this.logger = logger;
        }

        public ILogger<ErrorProcessor> Logger => logger;

        public async Task<ErrorMessage> ProcessErrorAsync(ErrorMessage error)
        {
            Logger.LogInformation($"Processing error from {error.Application}");

            // Додаткова обробка помилки
            if (string.IsNullOrEmpty(error.Environment))
            {
                error.Environment = "Unknown";
            }

            if (error.Time == default)
            {
                error.Time = DateTime.UtcNow;
            }

            if (error.Timestamp == null)
            {
                error.Timestamp = error.Time;
            }

            // Обрізаємо stack trace якщо він занадто довгий
            if (!string.IsNullOrEmpty(error.StackTrace) && error.StackTrace.Length > 1000)
            {
                error.StackTrace = error.StackTrace.Substring(0, 1000) + "...";
            }

            return await Task.FromResult(error);
        }

        public async Task<ErrorMessage> ProcesssErrorAsync(ErrorMessage error)
        {
            return await ProcessErrorAsync(error);
        }

        public async Task<ErrorMessage> ValidateErrorMessageAsync(ErrorMessage error)
        {
            if (error == null)
            {
                Logger.LogWarning("Error message is null");
                return null;
            }

            if (string.IsNullOrEmpty(error.Application))
            {
                Logger.LogWarning("Application name is required");
                return null;
            }

            if (string.IsNullOrEmpty(error.Message))
            {
                Logger.LogWarning("Error message is required");
                return null;
            }

            return error;
        }
    }
}
