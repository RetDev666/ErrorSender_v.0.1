using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.Models;
using ErrSendApplication.Proceses;
using Serilog;

namespace ErrSendApplication.Processes
{
    public class ErrorProcessor : IErrorProcessor
    {
        private readonly ILogger<ErrorProcessor> _logger;

        public ErrorProcessor(ILogger<ErrorProcessor> logger)
        {
            _logger = logger;
        }

        public async Task<ErrorMessage> ProcessErrorAsync(ErrorMessage error)
        {
            _logger.LogInformation($"Processing error from {error.Application}");

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

        public Task<ErrorMessage> ProcesssErrorAsync(ErrorMessage error)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateErrorMessageAsync(ErrorMessage error)
        {
            if (error == null)
            {
                _logger.LogWarning("Error message is null");
                return false;
            }

            if (string.IsNullOrEmpty(error.Application))
            {
                _logger.LogWarning("Application name is required");
                return false;
            }

            if (string.IsNullOrEmpty(error.Message))
            {
                _logger.LogWarning("Error message is required");
                return false;
            }

            return await Task.FromResult(true);
        }

        Task<ErrorMessage> IErrorProcessor.ValidateErrorMessageAsync(ErrorMessage error)
        {
            throw new NotImplementedException();
        }
    }
}
