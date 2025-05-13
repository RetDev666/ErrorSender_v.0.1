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
        public string ChatId { get; set; }
        public string BotName { get; set; }
    }
}
