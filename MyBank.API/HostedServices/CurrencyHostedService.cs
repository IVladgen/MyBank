using ILogger = Serilog.ILogger;
using MyBank.Domain.Interfaces.Services;
using MyBank.Domain.ValueObjects;

namespace MyBank.API.HostedServices
{
    public class CurrencyHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;

        public CurrencyHostedService(IServiceScopeFactory scopeFactory, ILogger logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();

                    await currencyService.RefreshCurrencyDataCache();
                }

                _logger.Information("обновление кэша");
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}
