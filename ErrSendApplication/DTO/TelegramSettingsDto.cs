using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrSendApplication.DTO
{
    public class TelegramSettingsDto
    {
        public bool Enables { get; set; }
        public required string ChatId { get; set; }
        public required string BotName { get; set; }
    }
}
