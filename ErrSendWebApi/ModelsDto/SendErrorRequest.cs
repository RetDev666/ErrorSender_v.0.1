using System.ComponentModel.DataAnnotations;

namespace ErrSendWebApi.ModelsDto
{
    public class SendErrorRequest
    {
        [Required(ErrorMessage = "Обов'язкова назва заявки")]
        public string Application { get; set; }
        public string Version { get; set; }
        public string Environment { get; set; }
        
        [Required(ErrorMessage = "Потрібне повідомлення про помилку")]
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string AdditionalData { get; set; }
        public DateTime? Time { get; set; }
    }
}
