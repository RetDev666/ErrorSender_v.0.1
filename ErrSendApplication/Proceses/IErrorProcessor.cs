using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace ErrSendApplication.Proceses
{
    public interface IErrorProcessor
    {
        Task<ErrorMessage> ProcesssErrorAsync(ErrorMessage error);
        Task<ErrorMessage> ValidateErrorMessageAsync(ErrorMessage error);
    }
}
