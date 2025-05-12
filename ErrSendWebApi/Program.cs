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
            //TODO: �������� ������� �� ���� ���, ���� ��������� ���� ��������� ������ �����  ��.
            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //TODO: ������� �� ���������� ���� �������� �� �� ����� ����������� �������.
                    //webBuilder.UseUrls("http://localhost:5001", "https://localhost:5002");
                    webBuilder.UseUrls("http://localhost:5001");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
