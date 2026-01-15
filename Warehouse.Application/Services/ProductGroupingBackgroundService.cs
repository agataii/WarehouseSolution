using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Warehouse.Application.Interfaces;

namespace Warehouse.Application.Services
{
    public class ProductGroupingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProductGroupingBackgroundService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(5);

        public ProductGroupingBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<ProductGroupingBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ProductGroupingBackgroundService запущен");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var productGroupService = scope.ServiceProvider.GetRequiredService<IProductGroupService>();

                    await productGroupService.ProcessUnprocessedProductsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при выполнении группировки товаров");
                }

                await Task.Delay(_period, stoppingToken);
            }

            _logger.LogInformation("ProductGroupingBackgroundService остановлен");
        }
    }
}