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
             .WriteTo.File("HybrisWebApi-.log", rollingInterval: RollingInterval.Day)
             .CreateLogger();
            //TODO: Структуа запуску має бути так, якщо необхідно буде підключити якийсь сервіс  ще.
            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //TODO: Поміняти на правильний порт подумати як із двома протоколами зробити.
                    //webBuilder.UseUrls("http://localhost:5001", "https://localhost:5002");
                    webBuilder.UseUrls("http://localhost:5001");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
