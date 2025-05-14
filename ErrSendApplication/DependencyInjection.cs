using ErrSendApplication.Authorization;
using ErrSendApplication.Behaviors;
using ErrSendApplication.Proceses;
using ErrSendApplication.Processes;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ErrSendApplication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            //Реєстрація авторизації
            services.AddScoped<IAuthorizationService, ApiKeyAuthorizationService>();

            //Реєстрація процесів
            services.AddScoped<IErrorProcessor, ErrorProcessor>();

            return services;
        }
    }
}
