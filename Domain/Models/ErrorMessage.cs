namespace Domain.Models
{
    /// <summary>
    /// модель для передачі інформації про помилку
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// Назва додатку, в якому виникла помилка
        /// </summary>
        public required string Application { get; set; }
       
        /// <summary>
        /// Версія додатку
        /// </summary>
        public required string Version { get; set; }
        
        /// <summary>
        /// Середовище виконання (Development, Production)
        /// </summary>
        public required string Environment { get; set; }
        
        /// <summary>
        /// Повідомлення про помилку
        /// </summary>
        public required string Message { get; set; }
        
        /// <summary>
        /// Стек виклику 
        /// <summary>
        public required string StackTrace { get; set; }

        /// <summary>
        /// Додаткова інформація 
        /// </summary>
        public required string AdditionalInfo { get; set; }

        /// <summary>
        /// Час вивникнення помилки 
        /// <summary>
        public DateTime Time { get; set; }
        public required object Timestamp { get; set; }
    }
}
