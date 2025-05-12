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
        public string Application { get; set; }
       
        /// <summary>
        /// Версія додатку
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// Середовище виконання (Development, Production)
        /// </summary>
        public string Environment { get; set; }
        
        /// <summary>
        /// Повідомлення про помилку
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Стек виклику 
        /// <summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Додаткова інформація 
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Час вивникнення помилки 
        /// <summary>
        public DateTime Time { get; set; }
        public object Timestamp { get; set; }
    }
}
