using System.ComponentModel.DataAnnotations;

namespace ErrSendWebApi.ModelsDto
{
    public class SendErrorRequest
    {
        [Required(ErrorMessage = "Обов'язкова назва заявки")]
        public required string Application { get; set; }
        public string? Version { get; set; }
        public string? Environment { get; set; }
        
        [Required(ErrorMessage = "Потрібне повідомлення про помилку")]
        public required string Message { get; set; }
        public string? StackTrace { get; set; }
        public string? AdditionalInfo { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
