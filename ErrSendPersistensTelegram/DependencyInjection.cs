using ErrSendApplication.Interfaces;
using ErrSendPersistensTelegram.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ErrSendPersistensTelegram
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceTelegram(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IHttpClientWr, StandartHttpClient>()
                            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                             {
                                 ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                             })
                            .ConfigureHttpClient(client =>
                            {
                                var serverUrl = configuration.GetValue<string>("serverUrl");
                                if (!string.IsNullOrEmpty(serverUrl))
                                {
                                    client.BaseAddress = new Uri(serverUrl);
                                }
                            });

            // Реєстрація TelegramBotService
            services.AddSingleton<TelegramBotService>();

            return services;
        }
    }
}
