using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBank.Domain.Interfaces.Context;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Interfaces.Services;
using MyBank.Infrastructure.Context;
using MyBank.Infrastructure.Implement.Auth;
using MyBank.Infrastructure.Repository;
using MyBank.Infrastructure.Services;

namespace MyBank.Infrastructure.DependensyInjection
{
    public static class DependensyInjection
    {
        public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgreSQL");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddHttpClient();
            services.InitRepositories();
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
        }

        public static void InitRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransferRepository, TransferRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
