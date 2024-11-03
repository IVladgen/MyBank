using Microsoft.Extensions.DependencyInjection;
using MyBank.Application.Behaviors;
using MyBank.Application.Services;
using MyBank.Domain.Interfaces.Cache;
using MyBank.Domain.Interfaces.Services;
using MyBank.Infrastructure.Cache;
using System.Reflection;


namespace MyBank.Application.DependensyInjection
{
    public static class DependensyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                options.AddOpenBehavior(typeof(UnitOfWorkBehaviour<,>));
                options.AddOpenBehavior(typeof(ValidationBehavior<,>));
                options.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379"; // Вам нужно указать конфигурацию вашего Redis-сервиса
                options.InstanceName = "SampleApp"; // Имя экземпляра кэша
            });

            InitServices(services);
        }

        public static void InitServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));
            services.AddTransient<ITokenService, TokenService>();
        }
    }
}
