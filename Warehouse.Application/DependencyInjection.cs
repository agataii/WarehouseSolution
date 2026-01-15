using Microsoft.Extensions.DependencyInjection;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Mapping;
using Warehouse.Application.Services;

namespace Warehouse.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            // Сервисы
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductGroupService, ProductGroupService>();

            // Background Service
            services.AddHostedService<ProductGroupingBackgroundService>();

            return services;
        }
    }
}
