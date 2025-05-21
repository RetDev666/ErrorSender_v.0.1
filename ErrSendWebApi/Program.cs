using Serilog.Events;
using Serilog;

namespace ErrSendWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
             .WriteTo.File("ErrorSender-.log", rollingInterval: RollingInterval.Day)
             .CreateLogger();
            // Запускаємо хостінг веб-застосунку
            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Налаштовуємо URL, який використовуватиме веб-сервер
                    webBuilder.UseUrls("http://localhost:5001");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
